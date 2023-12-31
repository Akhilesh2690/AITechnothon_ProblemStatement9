﻿using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Options;
using AITechnothon_ProblemStatement9.Utilities;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AntiVirus;
using Microsoft.Extensions.Options;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class S3ClientRepository : IS3ClientRepository
    {
        private readonly string? bucketName;
        private string uploadfilePath = Directory.GetCurrentDirectory() + "/Uploadfiles";
        private AmazonS3Client client;
        private readonly IFileScanner _fileScanner;
        private AWSDetailsOptions? _aWSOptions;

        public S3ClientRepository(IFileScanner fileScanner, IOptions<AWSDetailsOptions> awsOptions)
        {
            _aWSOptions = awsOptions?.Value;
            client = new AmazonS3Client(_aWSOptions?.s3ClientDetails?.AWSAccessKey, _aWSOptions?.s3ClientDetails?.AWSSecretKey,
            RegionDetails.GetRegionEndpoint(_aWSOptions?.s3ClientDetails?.Region));
            bucketName = _aWSOptions?.s3ClientDetails?.BucketName;
            _fileScanner = fileScanner;
        }

        /// <summary>
        /// Get file from S3 client
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<GetObjectResponse> GetS3ClientDocument(string fileName)
        {
            try
            {
                var transferUtility = new TransferUtility(client);
                var response = await transferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = fileName
                });

                return response;
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                       (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                              amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Exception occured in S3ClientRepository.GetS3ClientDocument. Please check the AWS Credentials.");
                }
                else
                {
                    throw new Exception("Exception occured in S3ClientRepository.GetS3ClientDocument. " + amazonS3Exception.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in S3ClientRepository.GetS3ClientDocument: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Get PreSigned Url
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetPreSignedUrl(string fileName)
        {
            GetPreSignedUrlRequest preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddDays(1)
            };

            return client.GetPreSignedURL(preSignedUrlRequest);
        }

        /// <summary>
        /// Upload file in S3 Bucket
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task<(bool, bool)> UploadFileAsync(IFormFile formFile)
        {
            bool isSaveSuccess = false;
            bool isVirusFound = false; ;
            try
            {
                await UploadFile(formFile);
                string filePath = Path.Combine(uploadfilePath, formFile.FileName);
                var result = _fileScanner.ScanFile(filePath);

                if (result == ScanResult.VirusNotFound)
                {
                    var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);

                    if (!bucketExists)
                    {
                        var bucketRequest = new PutBucketRequest()
                        {
                            BucketName = bucketName,
                            UseClientRegion = true
                        };
                        await client.PutBucketAsync(bucketRequest);
                    }

                    var objectRequest = new PutObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = formFile.FileName,
                        InputStream = formFile.OpenReadStream(),
                        StorageClass = S3StorageClass.Standard
                    };

                    var response = await client.PutObjectAsync(objectRequest);
                    if (response != null && (int)response.HttpStatusCode == 200)
                    {
                        isSaveSuccess = true;
                    }
                }
                else if (result == ScanResult.VirusFound)
                {
                    isVirusFound = true;
                }
                DeleteFile(formFile.FileName);

                return (isSaveSuccess, isVirusFound);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in S3ClientRepository.UploadFileAsync: " + ex.Message);
                return (isSaveSuccess, isVirusFound); ;
            }
        }

        /// <summary>
        /// Delete file from s3 bucket
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var response = await client.DeleteObjectAsync(bucketName, fileName);
            if (response != null)
            {
                return true;
            }
            else { return false; }
        }

        /// <summary>
        /// Upload file in project directory
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private async Task UploadFile(IFormFile file)
        {
            string filePath = Path.Combine(uploadfilePath, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// Delete file from project directory
        /// </summary>
        /// <param name="file"></param>
        private void DeleteFile(string file)
        {
            string filePath = Path.Combine(uploadfilePath, file);
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
        }
    }
}
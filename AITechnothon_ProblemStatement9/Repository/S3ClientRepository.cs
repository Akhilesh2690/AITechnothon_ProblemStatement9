using AITechnothon_ProblemStatement9.Domain;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using AntiVirus;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class S3ClientRepository : IS3ClientRepository
    {
        private readonly string AWSAccessKey = "AKIA5WAEWOPO3GQKB2F4";
        private readonly string AWSSecretKey = "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";
        private readonly string bucketName = "s3bucket-demo-test";
        private string uploadfilePath = Directory.GetCurrentDirectory() + "/Uploadfiles";
        private AmazonS3Client client;
        private readonly IFileScanner _fileScanner;

        public S3ClientRepository(IFileScanner fileScanner)
        {
            client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.APSouth1);
            _fileScanner = fileScanner;
        }

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

        public async Task<bool> UploadFileAsync(IFormFile formFile)
        {
            bool isSaveSuccess = false;
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
                    DeleteFile(formFile.FileName);
                    return isSaveSuccess;
                }

                return isSaveSuccess;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured in S3ClientRepository.UploadFileAsync: " + ex.Message);
                return isSaveSuccess;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var response = await client.DeleteObjectAsync(bucketName, fileName);
            if (response != null)
            {
                return true;
            }
            else { return false; }
        }

        private async Task UploadFile(IFormFile file)
        {
            string filePath = Path.Combine(uploadfilePath, file.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

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
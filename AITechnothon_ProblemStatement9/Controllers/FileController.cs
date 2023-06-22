using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Mvc;

namespace AITechnothon_ProblemStatement9.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        public string bucketName = "s3bucket-demo-test";

        [HttpGet("getfile")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var client = new AmazonS3Client("AKIA5WAEWOPO3GQKB2F4", "WWCZ73PICuLKTx9hvSzQAVQbw+UAL6qXFAzHKunc", RegionEndpoint.APSouth1);
            var file = await client.GetObjectAsync(bucketName, fileName);
            return File(file.ResponseStream, file.Headers.ContentType);
        }

        [HttpGet("getfiles")]
        public async Task<IActionResult> GetFiles(string prefix)
        {
            try
            {
                var client = new AmazonS3Client("AKIA5WAEWOPO3GQKB2F4", "WWCZ73PICuLKTx9hvSzQAVQbw+UAL6qXFAzHKunc", RegionEndpoint.APSouth1);
                var request = new ListObjectsV2Request()
                {
                    BucketName = bucketName,
                    Prefix = prefix
                };
                var response = await client.ListObjectsV2Async(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost("upload")]
        public async Task UploadFile(IFormFile formFile)
        {
            try
            {
                var client = new AmazonS3Client("AKIA5WAEWOPO3GQKB2F4", "WWCZ73PICuLKTx9hvSzQAVQbw+UAL6qXFAzHKunc", RegionEndpoint.APSouth1);
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
                    Key = $"{DateTime.Now:yyyyMMhhmmss} {formFile.FileName}",
                    InputStream = formFile.OpenReadStream(),
                    StorageClass = S3StorageClass.Standard
                };

                var response = await client.PutObjectAsync(objectRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {
                var client = new AmazonS3Client("AKIA5WAEWOPO3GQKB2F4", "WWCZ73PICuLKTx9hvSzQAVQbw+UAL6qXFAzHKunc", RegionEndpoint.APSouth1);
                {
                    var transferUtility = new TransferUtility(client);
                    var response = await transferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
                    {
                        BucketName = bucketName,
                        Key = fileName
                    });

                    if (response.ResponseStream == null)
                    {
                        return NotFound();
                    }
                    return File(response.ResponseStream, response.Headers.ContentType, fileName);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                       (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                              amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    throw new Exception("Please check the AWS Credentials.");
                }
                else
                {
                    throw new Exception(amazonS3Exception.Message);
                }
            }
        }
    }
}
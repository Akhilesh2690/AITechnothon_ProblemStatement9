using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Microsoft.AspNetCore.Mvc;

namespace AITechnothon_ProblemStatement9.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        public string bucketName = "s3bucket-demo-test";

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("getfile")]
        public async Task<IActionResult> GetFile(string fileName)
        {
            var client = new AmazonS3Client();
            var response = await client.GetObjectAsync(bucketName, fileName);
            return Ok(response);
        }

        [HttpGet("getfiles")]
        public async Task<IActionResult> GetFiles(string fileName)
        {
            var client = new AmazonS3Client();
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                //Prefix = prefix
            };
            var response = await client.ListObjectsV2Async(request);
            return Ok();
        }

        [HttpPost]
        public async Task UploadFile(IFormFile formFile)
        {

            var client = new AmazonS3Client();
           // var client = new AmazonS3Client("ACCESS_KEY_ID", "SECRET_KEY_ID", RegionEndpoint.APSoutheast2);
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
            // objectRequest.Metadata.Add("test", "MetaData");

            var response = await client.PutObjectAsync(objectRequest);
        }
    }
}
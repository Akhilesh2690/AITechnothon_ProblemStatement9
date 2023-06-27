using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Models;
using AITechnothon_ProblemStatement9.Options;
using AITechnothon_ProblemStatement9.Utilities;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Microsoft.Extensions.Options;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class DynamoDBClientConfig : IDynamoDBClientConfig
    {
        private readonly AmazonDynamoDBClient _client;
        private DynamoDBClientDetails? _dynamoDBClientDetails;
        public DynamoDBClientConfig(IOptions<AWSDetailsOptions> aWSDetailsOptions)
        {
            _dynamoDBClientDetails = aWSDetailsOptions.Value?.dynamoDBClientDetails;
            var credentials = new BasicAWSCredentials(_dynamoDBClientDetails?.AWSAccessKey, _dynamoDBClientDetails?.AWSSecretKey);
            var config = new AmazonDynamoDBConfig()
            {
                RegionEndpoint = RegionDetails.GetRegionEndpoint(_dynamoDBClientDetails?.Region)
            };
            _client = new AmazonDynamoDBClient(credentials, config);
        }

        public AmazonDynamoDBClient GetDynamoDBClient()
        {
            return _client;
        }
    }
}

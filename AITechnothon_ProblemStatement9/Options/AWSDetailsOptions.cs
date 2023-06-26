using AITechnothon_ProblemStatement9.Models;

namespace AITechnothon_ProblemStatement9.Options
{
    public class AWSDetailsOptions
    {
       public S3ClientDetails? s3ClientDetails { get; set; }
        public DynamoDBClientDetails? dynamoDBClientDetails { get; set; }
    }
}

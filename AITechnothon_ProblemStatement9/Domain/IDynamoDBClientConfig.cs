using Amazon.DynamoDBv2;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IDynamoDBClientConfig
    {
        AmazonDynamoDBClient GetDynamoDBClient();
    }
}

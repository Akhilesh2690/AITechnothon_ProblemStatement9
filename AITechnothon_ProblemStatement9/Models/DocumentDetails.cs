using Amazon.DynamoDBv2.DataModel;

namespace AITechnothon_ProblemStatement9.Models
{
    [DynamoDBTable("DocumentDetails")]
    public class DocumentDetails
    {
        [DynamoDBHashKey("ApplicationId")]
        public int? ApplicationId { get; set; }

        [DynamoDBProperty("ClientId")]
        public int? ClientId { get; set; }

        [DynamoDBProperty("FileName")]
        public string? FileName { get; set; }

        [DynamoDBProperty("CreationDate")]
        public DateTime CreationDate { get; set; }

        [DynamoDBProperty("DocumentId")]
        public int? DocumentId { get; set; }
    }
}
using Amazon.DynamoDBv2.DataModel;

namespace AITechnothon_ProblemStatement9.Models
{
    [DynamoDBTable("DocumentDetails")]
    public class DocumentDetails
    {
        [DynamoDBProperty("ApplicationId")]
        public int? ApplicationId { get; set; }

        [DynamoDBProperty("ClientId")]
        public int? ClientId { get; set; }

        [DynamoDBProperty("FileName")]
        public string? FileName { get; set; }

        [DynamoDBProperty("CreationDate")]
        public string? CreationDate { get; set; }

        [DynamoDBHashKey("DocumentId")]
        public int? DocumentId { get; set; }

        [DynamoDBProperty("Description")]
        public string? Description { get; set; }
    }
}
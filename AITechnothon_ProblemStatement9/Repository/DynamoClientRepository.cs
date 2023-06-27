using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Models;
using AITechnothon_ProblemStatement9.Options;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Microsoft.Extensions.Options;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class DynamoClientRepository : IDynamoClientRepository
    {
        private readonly IDynamoDBContext _context;
        private readonly AppDetailsOptions _appDetailsOptions;

        public DynamoClientRepository(IDynamoDBContext context, IOptions<AppDetailsOptions> appDetailsOptions)
        {
            _context = context;
            _appDetailsOptions = appDetailsOptions.Value;
        }

        public async Task<List<DocumentDetails>> GetDocumentDetails(int documentId = 0, int applicationId = 0, int clientId = 0, string documentName = "" , bool isSearchByFileNameContains = false)
        {
            try
            {
                List<ScanCondition> conditions = new List<ScanCondition>();
                if (documentId > 0)
                {
                    conditions.Add(new ScanCondition("DocumentId", ScanOperator.Equal, documentId));
                }
                if (applicationId > 0)
                {
                    conditions.Add(new ScanCondition("ApplicationId", ScanOperator.Equal, applicationId));
                }
                if (clientId > 0)
                {
                    conditions.Add(new ScanCondition("ClientId", ScanOperator.Equal, clientId));
                }

                if (!string.IsNullOrEmpty(documentName))
                {
                    if(isSearchByFileNameContains)
                    {
                        conditions.Add(new ScanCondition("FileName", ScanOperator.Contains, documentName));
                    }
                    else
                    {
                        conditions.Add(new ScanCondition("FileName", ScanOperator.Equal, documentName));
                    }
                }

                var documents = await _context.ScanAsync<DocumentDetails>(
                   conditions
                   ).GetRemainingAsync();

                return documents;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured in DynamoClientRepository.GetDocumentDetails: {ex.Message}");
                throw;
            }
        }

        public async Task<(bool, int)> SaveRecordDyanmoDB(string fileName, string description, int documentId = 0)
        {
           
            bool isRecordInsertedDynamoDB = false;
            if(documentId == 0)
            {
                Random rnd = new Random();
                documentId = rnd.Next();
            }
            
            try
            {
                DocumentDetails doc = new DocumentDetails()
                {
                    ApplicationId = _appDetailsOptions.ApplicationId,
                    ClientId = _appDetailsOptions.ClientId,
                    FileName = fileName,
                    CreationDate = DateTime.Now.ToString(),
                    DocumentId = documentId,
                    Description= description
                };

                await _context.SaveAsync(doc);
                isRecordInsertedDynamoDB = true;
                return (isRecordInsertedDynamoDB, documentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occure while inserting record in DynamoDb{0} ", ex.Message);
                return (isRecordInsertedDynamoDB, 0);
            }
        }
    }
}
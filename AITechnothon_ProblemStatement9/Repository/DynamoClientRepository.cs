﻿using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Models;
using Amazon.Auth.AccessControlPolicy.ActionIdentifiers;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;

namespace AITechnothon_ProblemStatement9.Repository
{
    public class DynamoClientRepository : IDynamoClientRepository
    {
        private readonly IDynamoDBContext _context;

        public DynamoClientRepository(IDynamoDBContext context)
        {
            _context = context;
        }

        public async Task<List<DocumentDetails>> GetDocumentDetails(int documentId = 0, int clientId = 0, string documentName = "" , bool isSearchByFileNameContains = false)
        {
            try
            {
                List<ScanCondition> conditions = new List<ScanCondition>();
                if (documentId > 0)
                {
                    conditions.Add(new ScanCondition("DocumentId", ScanOperator.Equal, documentId));
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
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occured in DynamoClientRepository.GetDocumentDetails: {ex.Message}");
                throw;
            }
        }

        public async Task<(bool, int)> SaveRecordDyanmoDB(string fileName)
        {
           
            bool isRecordInsertedDynamoDB = false;
            Random rnd = new Random();
            int docId = rnd.Next();
            try
            {
                DocumentDetails doc = new DocumentDetails()
                {
                    ApplicationId = 3,
                    ClientId = 2,
                    FileName = fileName,
                    CreationDate = DateTime.Now.ToString(),
                    DocumentId = docId
                };

                await _context.SaveAsync(doc);
                isRecordInsertedDynamoDB = true;
                return (isRecordInsertedDynamoDB, docId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occure while inserting record in DynamoDb{0} ", ex.Message);
                return (isRecordInsertedDynamoDB, 0);
            }
        }
    }
}
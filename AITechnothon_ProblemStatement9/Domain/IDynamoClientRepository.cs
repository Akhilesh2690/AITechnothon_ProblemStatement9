using AITechnothon_ProblemStatement9.Models;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IDynamoClientRepository
    {
        Task<(bool, int)> SaveRecordDyanmoDB(string fileName, string description, int documentId = 0);

        Task<List<DocumentDetails>> GetDocumentDetails(int documentId, int applicationId, int clientId, string documentName, bool isSearchByFileNameContains = false);
    }
}
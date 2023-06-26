using AITechnothon_ProblemStatement9.Models;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IDynamoClientRepository
    {
        Task<(bool, int)> SaveRecordDyanmoDB(string fileName);

        Task<List<DocumentDetails>> GetDocumentDetails(int documentId, int clientId, string documentName,bool isSearchByFileNameContains = false);
    }
}
using AITechnothon_ProblemStatement9.Models;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IDocumentMapper
    {
        List<Documents> MapDocuments(List<DocumentDetails> documentDetails);
    }
}
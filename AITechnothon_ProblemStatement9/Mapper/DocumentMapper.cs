using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Models;

namespace AITechnothon_ProblemStatement9.Mapper
{
    public class DocumentMapper : IDocumentMapper
    {
        public List<Documents> MapDocuments(List<DocumentDetails> documentDetails)
        {
            List<Documents> documents = new List<Documents>();

            foreach (var documentsDetail in documentDetails)
            {
                var document = new Documents
                {
                    ApplicationId = documentsDetail.ApplicationId,
                    DocumentId = documentsDetail.DocumentId,
                    ClientId = documentsDetail.ClientId,
                    CreationDate = documentsDetail.CreationDate,
                    Description = documentsDetail.Description,
                    FileName = documentsDetail.FileName
                };

                documents.Add(document);
            }
            return documents;
        }
    }
}
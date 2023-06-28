using Amazon.S3.Model;

namespace AITechnothon_ProblemStatement9.Domain
{
    public interface IS3ClientRepository
    {
        Task<GetObjectResponse> GetS3ClientDocument(string fileName);

        Task<(bool, bool)> UploadFileAsync(IFormFile file);

        Task<bool> DeleteFileAsync(string fileName);

        string GetPreSignedUrl(string fileName);
    }
}
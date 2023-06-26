using AITechnothon_ProblemStatement9.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AITechnothon_ProblemStatement9.Controllers
{
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class DocumentController : ControllerBase
    {
        private readonly IS3ClientRepository _s3ClientRepository;
        private readonly IDynamoClientRepository _dynamoClientRepository;

        public DocumentController(IS3ClientRepository s3ClientRepository, IDynamoClientRepository dynamoClientRepository)
        {
            _s3ClientRepository = s3ClientRepository;
            _dynamoClientRepository = dynamoClientRepository;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile)
        {
            bool isFileUploaded = false;
            var isRecordInsertedDynamoDB = false;
            int docId = 0;

            isFileUploaded = await _s3ClientRepository.UploadFileAsync(formFile);
            if (isFileUploaded)
            {
                (isRecordInsertedDynamoDB, docId) = await _dynamoClientRepository.SaveRecordDyanmoDB(formFile.FileName);
                if (!isRecordInsertedDynamoDB)
                {
                    await _s3ClientRepository.DeleteFileAsync(formFile.FileName);
                }
            }
            if (isFileUploaded && isRecordInsertedDynamoDB)
            {
                return Ok($"Document with documentId {docId} uploaded on S3Client bucket & meatadata inserted in DynamoDb successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error occured whlile uploading document on S3Client or inserting data into DynamoDB. Please check the logs");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile(int documentId, string fileName)
        {
            try
            {
                var documents = await _dynamoClientRepository.GetDocumentDetails(documentId, 0, fileName);

                if (documents == null || documents?.Count == 0)
                    return NotFound();

                var response = await _s3ClientRepository.GetS3ClientDocument(fileName);

                if (response.ResponseStream == null)
                {
                    return NotFound();
                }
                return File(response.ResponseStream, response.Headers.ContentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Exception occured in DownloadFile file for documentId {documentId} fileName {fileName}.: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSearchByDocumentName(string documentName = "", int applicationId = 0, int clientId = 0)
        {
            try
            {
                var documents = await _dynamoClientRepository.GetDocumentDetails(applicationId, clientId, documentName, true);

                if (documents?.Count == 0)
                    return NotFound();
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured whlile fetching metadata from DynamoDB for applicationId {applicationId}," +
                    $" clientId {clientId} and documentname {documentName} : {ex.Message}");
            }
        }
    }
}
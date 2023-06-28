using AITechnothon_ProblemStatement9.Domain;
using AITechnothon_ProblemStatement9.Models;
using AITechnothon_ProblemStatement9.Options;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AITechnothon_ProblemStatement9.Controllers
{
    [Route("[controller]")]
    [EnableCors("CorsPolicy")]
    public class DocumentController : ControllerBase
    {
        private readonly IS3ClientRepository _s3ClientRepository;
        private readonly IDynamoClientRepository _dynamoClientRepository;
        private readonly AppDetailsOptions _appDetailsOptions;
        private readonly IDocumentMapper _documentMapper;

        public DocumentController(IS3ClientRepository s3ClientRepository, IDynamoClientRepository dynamoClientRepository,
            IOptions<AppDetailsOptions> appDetailsOptions, IDocumentMapper documentMapper)
        {
            _s3ClientRepository = s3ClientRepository;
            _dynamoClientRepository = dynamoClientRepository;
            _appDetailsOptions = appDetailsOptions.Value;
            _documentMapper = documentMapper;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFileAsync(IFormFile formFile, string description = "")
        {
            bool isFileUploaded = false;
            bool isVirusFound = false;
            var isRecordInsertedDynamoDB = false;
            int docId = 0;

            if (formFile.FileName == null)
                return BadRequest("Please upload valid file.");

            (isFileUploaded, isVirusFound) = await _s3ClientRepository.UploadFileAsync(formFile);

            if (isVirusFound)
            {
                return StatusCode(StatusCodes.Status502BadGateway, $"{formFile.FileName} affetced by virus");
            }

            if (isFileUploaded)
            {
                var documents = await _dynamoClientRepository.GetDocumentDetails(0, _appDetailsOptions.ApplicationId, _appDetailsOptions.ClientId,
                formFile.FileName, false);
                int documentId = 0;
                int existingDocId = Convert.ToInt32(documents?.FirstOrDefault()?.DocumentId);

                if (existingDocId > 0)
                {
                    documentId = existingDocId;
                }
                (isRecordInsertedDynamoDB, docId) = await _dynamoClientRepository.SaveRecordDyanmoDB(formFile.FileName, description,
                    documentId);
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
                    $"Error occured whlile uploading {formFile.FileName} document on S3Client or inserting data into DynamoDB." +
                    $"Please check the logs");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile(int documentId, string fileName)
        {
            try
            {
                var documents = await _dynamoClientRepository.GetDocumentDetails(documentId, _appDetailsOptions.ApplicationId,
                    _appDetailsOptions.ClientId, fileName);

                if (documents == null || documents?.Count == 0)
                    return NotFound($"Document with documentId: {documentId} and filename: {fileName} not found");

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
                    $"Exception occured while downloading file for documentId {documentId} fileName {fileName}.: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSearchByDocumentName(string documentName = "", int applicationId = 0, int clientId = 0)
        {
            try
            {
                var documentDetails = await _dynamoClientRepository.GetDocumentDetails(0, applicationId, clientId, documentName, true);

                if (documentDetails?.Count == 0)
                    return NotFound();

                List<Documents> documents = new List<Documents>();
                if (documentDetails != null)
                {
                    documents = _documentMapper.MapDocuments(documentDetails);
                    documents.ForEach(x => x.FileURL = _s3ClientRepository.GetPreSignedUrl(x?.FileName));
                }
                return Ok(documents);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error occured whlile fetching documentfor applicationId {applicationId}," +
                    $" clientId {clientId} and documentname {documentName} : {ex.Message}");
            }
        }
    }
}
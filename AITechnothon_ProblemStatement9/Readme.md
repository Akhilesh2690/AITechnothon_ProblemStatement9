#Region DocumentAPI Details

Project Name: Document API
 This Document API is for REST API that allows users to upload documents along with metadata. The API includes endpoints for searching and downloading
 documents. This API includes leverage AWS services, utilizing services like S3 and DynamoDB for document storage and metadata management 
 and also virus scanning functionality implemented during document upload.The metadata includes relevant information such as application ID, client ID, 
 description, etc.

Table of Contents:
  - Installation
  - Usage
  - Endpoint Information
  - Contributing
  - License

  Installation:
   1. Clone the respository: 'https://github.com/Akhilesh2690/AITechnothon_ProblemStatement9.git'
   2. Install .Net SDK 6.0.400 on your machine (https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
   3. Open the project in your prefered IDE(e.g. Visual Studio)
   4. Build the solution to restore nuget packages.

  Endpoint Information:
   
    Get document information Endpoint:
      Request Type: GET
      Request endpoint: /Document
      Endpoint Information: This api end point will get the file metadata information from DyanmoDb.We are passing parameters documentName,applicationId and clientId
                             based on these parameter we can fetch the document details.If we are not passing any parameter,it will fetch all the document details
                             from Dynamo Db.

    Upload document Endpoint:
      Request Type: POST
      Request endpoint: /Document/upload
      Endpoint Information: This api end point will uploading the file in S3 bucket and storing the file meta data in dynamoDb.
                            During uploading first we are scanning the file for malware,virus , if file is affected by any virus or malware,
                            then we are not able to  upload that file. If file is not affected by any virus or malware then only we are able 
                            upload the file in s3 Db and storing file meta data in dynamoDb.

    Download document Endpoint:
      Request Type: POST
      Request endpoint: /Document
      Endpoint Information: This api end point is able to  download file , from S3 Db by using two parameters documentId and fileName.
                            If documentId and fileName is valid , then first we are searching the file details in  DyanmoDb,
                            if file details exists then only we can download the file from S3 Db.
      
                            
  Contributing:
      1. Fork the repository
      2. Create new branch
      3. Make your changes and commit them.
      4. Push to branch.
      5. Submit pull request which needs to point out to master branch.

    License:
      This project is licensed under the Team57 Technothon License.

#EndRegion

#Region Release Versions
   * Release 1.0.0
      - Added implementation of Upload, Download and Get document details endpoints.
#EndRegion


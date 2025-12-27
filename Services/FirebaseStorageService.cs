using e_commerce_basic.Dtos.File;
using e_commerce_basic.Interfaces;
using FirebaseAdmin;
using Google.Cloud.Storage.V1;

namespace e_commerce_basic.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly string _bucketName;
        private readonly StorageClient _storageClient;

        public FirebaseStorageService(IConfiguration config)
        {
            if (FirebaseApp.DefaultInstance == null)
                throw new Exception("FirebaseApp not yet create");

            _bucketName = config["FIREBASE:FIREBASE_BUCKET"]
                          ?? throw new Exception("FIREBASE_BUCKET env missing");

            var googleCredential = FirebaseAdmin.FirebaseApp.DefaultInstance.Options.Credential;
            _storageClient = StorageClient.Create(googleCredential);
        }

        public async Task<UploadedFileDto> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("File stream is empty", nameof(fileStream));

            var objectName = $"{Guid.NewGuid()}_{fileName}";

            var obj = await _storageClient.UploadObjectAsync(
                bucket: _bucketName,
                objectName: objectName,
                contentType: contentType,
                source: fileStream
            );
            await _storageClient.UpdateObjectAsync(
                obj,
                new UpdateObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }
            );

            var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{obj.Name}";
            return new UploadedFileDto
            {
                FileName = fileName,
                Url = publicUrl,
                ContentType = contentType,
                Size = fileStream.Length,
                UploadedAt = DateTime.UtcNow
            }; ;
        }
    }
}

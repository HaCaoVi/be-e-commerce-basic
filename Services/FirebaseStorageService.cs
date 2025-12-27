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

        public async Task DeleteFile(IEnumerable<string>? filesToDelete = null)
        {
            if (filesToDelete != null)
            {
                foreach (var oldFile in filesToDelete)
                {
                    try
                    {
                        await _storageClient.DeleteObjectAsync(_bucketName, oldFile);
                    }
                    catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to delete file '{oldFile}': {ex.Message}");
                    }
                }
            }
        }

        public async Task<List<UploadedFileDto>> UploadFilesAsync(
      IEnumerable<(Stream Stream, string FileName, string ContentType)> files,
      IEnumerable<string>? filesToDelete = null
  )
        {
            if (files == null || !files.Any())
                throw new ArgumentException("No files to upload", nameof(files));

            var uploadedFiles = new List<UploadedFileDto>();

            foreach (var file in files)
            {
                if (file.Stream == null || file.Stream.Length == 0)
                    continue;

                var objectName = $"{Guid.NewGuid()}_{file.FileName}";

                var obj = await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: objectName,
                    contentType: file.ContentType,
                    source: file.Stream
                );

                await _storageClient.UpdateObjectAsync(
                    obj,
                    new UpdateObjectOptions { PredefinedAcl = PredefinedObjectAcl.PublicRead }
                );

                var publicUrl = $"https://storage.googleapis.com/{_bucketName}/{obj.Name}";

                uploadedFiles.Add(new UploadedFileDto
                {
                    FileName = file.FileName,
                    Url = publicUrl,
                    ContentType = file.ContentType,
                    Size = file.Stream.Length,
                    UploadedAt = DateTime.UtcNow
                });
            }

            // Xóa file cũ nếu có
            if (filesToDelete != null)
                await DeleteFile(filesToDelete);

            return uploadedFiles;
        }
    }
}

using e_commerce_basic.Dtos.File;

namespace e_commerce_basic.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<UploadedFileDto> UploadFileAsync(Stream fileStream, string fileName, string contentType);
    }
}
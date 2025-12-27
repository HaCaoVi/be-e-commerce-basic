using e_commerce_basic.Dtos.File;

namespace e_commerce_basic.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<List<UploadedFileDto>> UploadFilesAsync(IEnumerable<(Stream Stream, string FileName, string ContentType)> files, IEnumerable<string>? filesToDelete = null);
    }
}
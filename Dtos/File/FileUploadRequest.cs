namespace e_commerce_basic.Dtos.File
{
    public class FileUploadRequest
    {
        public IFormFile File { get; set; } = default!;
    }
}
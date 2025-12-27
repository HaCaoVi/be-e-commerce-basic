namespace e_commerce_basic.Dtos.File
{
    public class UploadedFileDto
    {
        public string FileName { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    }

}
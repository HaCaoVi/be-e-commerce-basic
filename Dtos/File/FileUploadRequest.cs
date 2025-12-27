using System.ComponentModel.DataAnnotations;

namespace e_commerce_basic.Dtos.File
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFileCollection Files { get; set; } = default!;
        public List<string>? FilesToDelete { get; set; } = null;
    }
}
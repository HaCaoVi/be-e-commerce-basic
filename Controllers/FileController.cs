using e_commerce_basic.Dtos.File;
using e_commerce_basic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce_basic.Controllers
{
    [ApiController]
    [Route("api/file")]
    public class FileController : ControllerBase
    {
        private readonly IFirebaseStorageService _firebaseStorage;

        public FileController(IFirebaseStorageService firebaseStorage)
        {
            _firebaseStorage = firebaseStorage;
        }

        [AllowAnonymous]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] FileUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is empty");

            using var stream = request.File.OpenReadStream();
            var url = await _firebaseStorage.UploadFileAsync(stream, request.File.FileName, request.File.ContentType);

            return Ok(new { url });
        }
    }
}
using e_commerce_basic.Common;
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
            if (request.Files == null || request.Files.Count == 0)
                return BadRequest("No files uploaded");

            var filesToUpload = request.Files
                .Select(f => (f.OpenReadStream(), f.FileName, f.ContentType));

            var uploadedFiles = await _firebaseStorage.UploadFilesAsync(filesToUpload, request.FilesToDelete);

            return Ok(ApiResponse<List<UploadedFileDto>>.Ok(uploadedFiles));
        }

    }
}
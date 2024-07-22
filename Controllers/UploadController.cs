using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ava.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
                return BadRequest("No files uploaded.");

            var file = files[0];
            if (file.Length == 0)
                return BadRequest("Empty file.");

            var allowedExtensions = new[] { ".jpg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (Array.IndexOf(allowedExtensions, extension) < 0)
                return BadRequest("Unsupported file extension.");

            var newFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/avatars", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName = newFileName });
        }
    }
}
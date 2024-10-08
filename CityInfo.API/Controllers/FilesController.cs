﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/files")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;
        //parameter injection
        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(
                nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        //give deprecated a lower version..
        [ApiVersion(0.1, Deprecated = true)]
        public ActionResult GetFile(string fileId)
        {
            //We have many types? FileContentResult, FileStreamResult, PhysicalFileResult, VirtualFileResult
            //more convenient to return File()
            //need copy pdf to output 
            //for pdf this will fail because we need a proper filetype
            // add a middleware for this!
            var pathToFile = "getting-acquainted-with-aspnet-core-slides.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if(!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                //catchall for unknown format
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            //for adding files we have a few constraints.
            if(file.Length == 0 || file.Length > 20971520
                || file.ContentType != "application/pdf")
            {
                return BadRequest("No file or invalid input");
            }
            //create file path.
            //Note: Avoid using file.FileName...
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf"); 

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been uploaded successfully.");
        }
    }
}

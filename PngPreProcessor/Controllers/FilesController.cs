using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PngPreProcessor.Models;
using PngPreProcessor.Services.Interfaes;

namespace PngPreProcessor.Controllers
{
    [Route("[controller]")]
    public class FilesController : Controller
    {
        IFileService FileService;
        
        public FilesController(IFileService fileService)
        {
            FileService = fileService;
        }

        [HttpGet("[action]")]
        public List<FileModel> getFiles() => FileService.getFiles();

        [HttpPost("[action]")]
        public IActionResult uploadFile([FromForm(Name = "uploadedFile")] IFormFile uploadedFile)
        {
            FileService.uploadFile(uploadedFile);
            return Ok();
        }

        [HttpGet("[action]")]
        public List<FileModel> getFilesInProcess() => FileService.getFilesInProcess();

        [HttpPost("[action]")]
        public IActionResult cancelProcess(string id)
        {
            FileService.cancelProcess(Convert.ToInt32(id));
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PngPreProcessor.Models;
using PngPreProcessor.Services.Interfaes;
using System;
using System.Collections.Generic;

namespace PngPreProcessor.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        IFileService FileService;

        public HomeController(IFileService fileService)
        {
            FileService = fileService;
        }

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return View(FileService.getFiles());
        }

        [HttpGet("[action]")]
        public List<FileModel> getFiles() => FileService.getFiles();

        [HttpPost("[action]")]
        public IActionResult uploadFile([FromForm(Name = "uploadedFile")] IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                FileService.uploadFile(uploadedFile);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("[action]")]
        public List<FileModel> getFilesInProcess() => FileService.getFilesInProcess();

        [HttpPost("[action]")]
        public IActionResult cancelProcess(string id)
        {
            FileService.cancelProcess(Convert.ToInt32(id));
            return RedirectToAction("Index", "Home");
        }
    }
}

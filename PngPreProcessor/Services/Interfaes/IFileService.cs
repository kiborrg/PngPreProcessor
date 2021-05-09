using Microsoft.AspNetCore.Http;
using PngPreProcessor.Models;
using System.Collections.Generic;

namespace PngPreProcessor.Services.Interfaes
{
    public interface IFileService
    {
        public List<FileModel> getFiles();
        public void uploadFile(IFormFile uploadedFile);
        public List<FileModel> getFilesInProcess();
        public void cancelProcess(int fileId);
    }
}

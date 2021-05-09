using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PngPreProcessor.Models;
using PngPreProcessor.Models.Context.Interfaces;
using PngPreProcessor.Services.Interfaes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PngPreProcessor.Services
{
    public class FileService : IFileService
    {
        IAppContext _context;
        IWebHostEnvironment _appEnvironment;

        public FileService(IAppContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public List<FileModel> getFiles()
        {
            if (_context.Files == null)
            {
                _context.Files = new List<FileModel>();
            }
            return _context.Files;
        }

        public List<FileModel> getFilesInProcess()
        {
            if (_context.Files == null)
            {
                _context.Files = new List<FileModel>();
            }
            return _context.Files.Where(f => f.Status == StatusEnum.InProcess).ToList();
        }

        public void addFile(FileModel file)
        {
            if (_context.Files == null)
            {
                _context.Files = new List<FileModel>();
            }

            _context.Files.Add(file);
        }

        public void uploadFile(IFormFile uploadedFile)
        {
            string path = "/files/" + uploadedFile.FileName;
            int id;
            if (_context.Files == null)
                id = 1;
            else
                id = _context.Files.Count + 1;
            FileModel file = new FileModel(id, uploadedFile.FileName, path, _appEnvironment.WebRootPath + path, uploadedFile); 
            addFile(file);

            file = _context.Files.Find(f => f.Id == file.Id);
            try
            {
                file.startThread();
            }
            catch (Exception e)
            {

                file.setError(e.Message);
                file.abortThread();
            }
            finally
            {
                if (!file.isThreadAlive())
                {
                    file.removeThread();
                }
            }
        }

        public void cancelProcess(int fileId)
        {
            FileModel file = _context.Files.Find(f => f.Id == fileId);

            if (file != null)
            {
                file.disposeProcessing();
                file.Status = StatusEnum.Canceled;
            }
        }
    }
}

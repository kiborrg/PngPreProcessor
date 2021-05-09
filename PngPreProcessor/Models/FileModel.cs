using ImageProcessor;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;

namespace PngPreProcessor.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullPath { get; set; }
        public StatusEnum Status { get; set; }
        public double Progress { get; set; }
        public string Error { get; set; }

        protected IFormFile File { get; set; }
        protected Thread Thread { get; set; }
        protected PngProcessor pngProcessor { get; set; }
        protected CancellationTokenSource CancellationTokenSource { get; set; }
        protected CancellationToken CancellationToken { get; set; }

        public bool isThreadAlive()
        {
            return Thread.IsAlive;
        }

        public FileModel(int id, string name, string path, string fullPath, IFormFile file)
        {
            Id = id;
            Name = name;
            Path = path;
            FullPath = fullPath;
            File = file;

            Status = StatusEnum.Created;
            Thread = new Thread(new ThreadStart(startUpload));

            CancellationTokenSource = new CancellationTokenSource();
            CancellationToken = CancellationTokenSource.Token;
        }

        public void startUpload()
        {
            using (var fileStream = new FileStream(FullPath, FileMode.Create))
            {
                try
                {
                    File.CopyToAsync(fileStream);
                    Status = StatusEnum.InQueueToProcess;
                }
                catch (Exception e)
                {
                    setError(e.Message);
                }
            }

            startProcessing();
        }

        public void startProcessing()
        {
            Status = StatusEnum.InProcess;
            using (pngProcessor = new PngProcessor())
            {
                try
                {
                    pngProcessor.ProgressChanged += on_ProgressChanged;
                    pngProcessor.Process(Path, CancellationToken);
                    Status = StatusEnum.Finished;
                }
                catch(NotSupportedException e)
                {
                    if (e.Message.Contains("The collection type 'Microsoft.AspNetCore.Http.IHeaderDictionary' on 'Microsoft.AspNetCore.Http.FormFile.Headers' is not supported"))
                    {
                        Error = e.Message;
                    }
                }
                catch(Exception e)
                {
                    pngProcessor.Dispose();
                    if (!e.Message.Contains("The operation was canceled."))
                        setError(e.Message);
                }
            }
        }

        private void on_ProgressChanged(double obj)
        {
            Progress = obj;
        }

        public void disposeProcessing()
        {
            CancellationTokenSource.Cancel();
        }

        public void setError(string error)
        {
            Status = StatusEnum.Error;
            Error = error;
        }

        public void startThread()
        {
            Thread.Start();
        }

        public void abortThread()
        {
            Thread.Interrupt();
            removeThread();
        }

        public void removeThread()
        {
            Thread = null;
        }
    }
}

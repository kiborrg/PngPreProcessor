using PngPreProcessor.Models.Context.Interfaces;
using System.Collections.Generic;

namespace PngPreProcessor.Models
{
    public class FilesContext : IAppContext
    {
        public List<FileModel> Files { get; set; }

        public void Add(FileModel file)
        {
            Files.Add(file);
        }
    }
}

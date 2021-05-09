using System.Collections.Generic;

namespace PngPreProcessor.Models.Context.Interfaces
{
    public interface IAppContext
    {
        List<FileModel> Files { get; set; }

        void Add(FileModel file);
    }
}

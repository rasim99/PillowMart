

using Microsoft.AspNetCore.Http;

namespace Core.Utilities.File
{
    public interface IFileService
    {
        string Upload(IFormFile file, string root,string folder);
        void Delete(string root, string folder, string fileName);
        bool IsImage(string contentType);

        bool IsAvailableSize(long length, long maxLength = 250);
    }
}


using Microsoft.AspNetCore.Http;

namespace Core.Utilities.File
{
    public class FileService : IFileService
    {
 

        public string Upload(IFormFile file, string root, string folder)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(root, folder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                file.CopyTo(fileStream);
            }
            return fileName ;
        }

        public void Delete(string root, string folder,string fileName)
        {
            var filePath = Path.Combine(root, folder, fileName);

            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);    
        }

        public bool IsImage(string contentType)
        {
            if (contentType.Contains("image/")) return true;
            
            return false;
        }
        public bool IsAvailableSize(long length,long maxLength=24)
        {
            if(length/1024 <= maxLength) return true;
            return false;
        }

      
    }
}

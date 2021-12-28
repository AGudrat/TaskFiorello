using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Fiorello.Utilities
{
    public static class Extension
    {
        public static bool CheckFileType(this IFormFile file,string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool CheckFileSize(this IFormFile file,int kb)
        {
            return file.Length / 1024 <= kb;
        }
        public static async Task<string> SaveFileAsync(this IFormFile file,string root,string folder)
        {
            string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string resultPath = Path.Combine(root, folder, fileName);
            using (FileStream stream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}

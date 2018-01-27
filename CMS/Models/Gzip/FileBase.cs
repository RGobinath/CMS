using System.IO;

namespace CMS.Models.Gzip
{
    public class FileBase : IFileBase
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
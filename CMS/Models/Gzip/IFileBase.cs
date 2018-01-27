namespace CMS.Models.Gzip
{
    public interface IFileBase
    {
        /// <summary> 
        /// Reads all of the bytes from a file into an array. 
        /// </summary> 
        /// <param name="path">The path of the file.</param> 
        byte[] ReadAllBytes(string path);

        /// <summary> 
        /// Reads all of the text from a file into a string. 
        /// </summary> 
        /// <param name="path">The path of the file.</param> 
        string ReadAllText(string path);
    }
}

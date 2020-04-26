using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.Infrastructure
{
    public interface IFileAccessor
    {
        string ReadFile(string filePath);
        string[] ReadFileLines(string filePath);
        Task<string> ReadFileAsync(string filePath);
        void WriteFile(string filePath, string content);
        Task WriteFileAsync(string filePath, string content);
        FileAttributes GetAttributes(string filePath);
        void SetAttributes(string filePath, FileAttributes attributes);
        bool Exists(string filePath);
        IEnumerable<string> EnumerateDirectories(string folderPath);
        IEnumerable<string> GetDirectories(string folderPath);
        bool DirectoryExists(string path);
        string GetTempFileName();
        StreamWriter CreateText(string filePath);
    }

    public class FileAccessor : IFileAccessor
    {
        public virtual StreamWriter CreateText(string filePath)
        {
            return File.CreateText(filePath);
        }

        public virtual string ReadFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }

        public virtual string[] ReadFileLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }

        public virtual async Task<string> ReadFileAsync(string filePath)
        {
            using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            {
                var buff = new byte[file.Length];
                await file.ReadAsync(buff, 0, (int)file.Length);
                return Encoding.Default.GetString(buff);
            }
        }

        public virtual async Task WriteFileAsync(string filePath, string content)
        {
            var encodedText = Encoding.Default.GetBytes(content);

            using (var sourceStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };

        }

        public virtual void WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public virtual FileAttributes GetAttributes(string filePath)
        {
            return File.GetAttributes(filePath);
        }

        public virtual void SetAttributes(string filePath, FileAttributes attributes)
        {
            File.SetAttributes(filePath, attributes);
        }

        public virtual bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public virtual bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public virtual IEnumerable<string> EnumerateDirectories(string folderPath)
        {
            return Directory.EnumerateDirectories(folderPath).Select(d => new DirectoryInfo(d).Name);
        }

        public virtual IEnumerable<string> GetDirectories(string folderPath)
        {
            return Directory.GetDirectories(folderPath);
        }

        public virtual string GetTempFileName()
        {
            return Path.GetTempFileName();
        }
    }
}

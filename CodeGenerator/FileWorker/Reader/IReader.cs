using System;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeGenerator.FileWorker.Reader
{
    public interface IReader
    {
        string[] Read(string path);
        void Write(string path, string info);
    }


    public class FileReader : IReader
    {
        public string[] Read(string path)
        {
            return File.ReadAllLines(path);
        }

        public void Write(string path, string data)
        {
            using (FileStream fs = File.Create(path))
            {
                fs.Write(new UTF8Encoding(true).GetBytes(data), 0, data.Length);
            }
        }
    }
}

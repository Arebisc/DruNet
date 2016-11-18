using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DruNet_WPF.Core
{
    class FileSystemOperator
    {
        private readonly string RootPath;

        public FileSystemOperator()
        {
            var currentDir = AppDomain.CurrentDomain.BaseDirectory;
            RootPath = Path.Combine(currentDir, "Root");
            Directory.CreateDirectory(RootPath);
        }

        public List<string> ListDirectory(string path = "")
        {
            string absolutePath = RootPath;
            List<string> filesList = new List<string>();

            if (path != String.Empty)
                absolutePath = Path.Combine(RootPath, path);

            foreach (var element in Directory.GetDirectories(absolutePath))
            {
                filesList.Add(Path.GetFileName(element) + "\\");
            }

            foreach (var element in Directory.GetFiles(absolutePath))
            {
                filesList.Add(Path.GetFileName(element));
            }

            return filesList;
        }

        public void CreateDirectory(string directoryName)
        {
            string absolutePath = RootPath;
            absolutePath = Path.Combine(RootPath, directoryName);

            Directory.CreateDirectory(absolutePath);
        }

        public string ReadFile(string fileName)
        {
            var absolutePath = Path.Combine(RootPath, fileName);
            StreamReader sr = new StreamReader(absolutePath);

            string result = sr.ReadToEnd();
            sr.Close();

            return result;
        }

        public void CreateFile(string fileName, string content = "")
        {
            string absolutePath = Path.Combine(RootPath, fileName);
            StreamWriter sr = new StreamWriter(absolutePath);

            sr.Write(content);
            sr.Flush();
            sr.Close();
        }
    }
}

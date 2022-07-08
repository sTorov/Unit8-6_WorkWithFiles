using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = string.Empty;

            Console.WriteLine("Укажите путь к папке:");
            path = Console.ReadLine();

            DirectoryInfo dir = new DirectoryInfo(path);

            if (dir.Exists)
            {
                GetDirs(dir);
            }
            else
                Console.WriteLine("NaN");

        }
        static void GetDirs(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                Console.WriteLine(dir.Name);
                GetDirs(dir);
            }
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                Console.WriteLine("\t" + file.Name);
            }
        }
    }
}

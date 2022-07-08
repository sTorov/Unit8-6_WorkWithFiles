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
            path = @"D:\!!!Test";

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
            try
            {
                DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                foreach (var dir in dirs)
                {
                    TimeSpan sub = DateTime.Now - dir.CreationTime;
                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдалено");
                        dir.Delete(true);
                    }

                    GetDirs(dir);
                }

                if (directoryInfo.Exists)
                {
                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach (var file in files)
                    {
                        TimeSpan sub = DateTime.Now - file.LastAccessTime;
                        if (sub > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine(file.FullName + "\t" + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдалёно");
                            file.Delete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }                             
        }
    }
}

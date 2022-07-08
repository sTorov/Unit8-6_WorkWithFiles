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
                DelFile_30min(dir);
                DelDir_30min(dir);
            }
            else
                Console.WriteLine("NaN");

        }
        static void DelDir_30min(DirectoryInfo directoryInfo)
        {
            try
            {
                DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                foreach (var dir in dirs)
                {
                    TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдалено");
                        dir.Delete(true);
                    }
                    
                    if(dir.Exists)
                        DelDir_30min(dir);
                }

                if (directoryInfo.Exists)
                {
                    DelFile_30min(directoryInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }                             
        }
        static void DelFile_30min(DirectoryInfo directoryInfo)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}

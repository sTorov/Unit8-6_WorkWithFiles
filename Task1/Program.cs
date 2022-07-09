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

            try
            {
                DelFile_30min(dir);
                DelDir_30min(dir);
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Ошибка: " + ex.Message);
                Console.ResetColor();
            }

        }
        static void DelDir_30min(DirectoryInfo directoryInfo)
        {            
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelFile_30min(dir);

                if (dir.Exists)
                    DelDir_30min(dir);

                TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                if (sub > TimeSpan.FromMinutes(30))
                {
                    if(dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю пустую папку");
                        dir.Delete();
                    }
                    else
                    {
                        Console.WriteLine($"В папке {dir.Name} есть файлы и папки, использовавшиеся в прошедшие 30 минут\nУдалить папку? (y / n)");

                        while (true)
                        {
                            var tap = Console.ReadKey(true);
                            switch (tap.KeyChar)
                            {
                                case 'y':
                                    dir.Delete(true);
                                    break;
                                case 'n':
                                    break;                                
                                default:
                                    Console.WriteLine("Удалить папку? (y / n)");
                                    continue;
                            }
                            break;
                        }
                    }
                }                
            }
        }
        static void DelFile_30min(DirectoryInfo directoryInfo)
        {
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (var file in files)
            {
                TimeSpan sub = DateTime.Now - file.LastAccessTime;
                if (sub > TimeSpan.FromMinutes(30))
                {
                    Console.WriteLine(file.FullName + "\t" + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю");
                    file.Delete();
                }
            }            
        }
    }
}

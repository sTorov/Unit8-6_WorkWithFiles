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
            Console.WriteLine();

            DirectoryInfo dir = new DirectoryInfo(path);

            if(dir.Exists)
            {
                Console.WriteLine("Выберете режим удаления\n1. Не удалять не пустые директории\n2. Ручной выбор\n3. Удалять все автоматически\n4. Удалить только файлы");
                try
                {
                    while (true)
                    {
                        Console.WriteLine("\nНажмите '1' '2' '3' '4' соответственно");
                        var click = Console.ReadKey(true).KeyChar;

                        switch (click)
                        {
                            case '1':
                                DelFile_30min(dir);
                                DelDir_30min_NoDel(dir);
                                break;
                            case '2':
                                DelFile_30min(dir);
                                DelDir_30min_Manual(dir);
                                break;
                            case '3':
                                DelFile_30min(dir);
                                DelDir_30min_Auto(dir);
                                break;
                            case '4':
                                DelFile_30min(dir);
                                DelDir_30min_FileIn(dir);
                                break;
                            default:
                                continue;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Ошибка: " + ex.Message);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Такой директории не существует!");
                Console.ResetColor();
            }
        }
        static void DelDir_30min_Manual(DirectoryInfo directoryInfo)
        {            
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelFile_30min(dir);

                DelDir_30min_Manual(dir);

                TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                if (sub > TimeSpan.FromMinutes(30))
                {
                    if(dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовалась {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю пустую папку");
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
        static void DelDir_30min_Auto(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelFile_30min(dir);

                DelDir_30min_Auto(dir);

                TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                if (sub > TimeSpan.FromMinutes(30))
                {
                    Console.WriteLine(dir.Name + " " + $"Не использовалась {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю папку");
                    dir.Delete(true);
                }
            }
        }
        static void DelDir_30min_NoDel(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelFile_30min(dir);

                if (dir.Exists)
                    DelDir_30min_NoDel(dir);

                TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                if (sub > TimeSpan.FromMinutes(30))
                {
                    if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю пустую папку");
                        dir.Delete();
                    }
                }
            }
        }
        static void DelDir_30min_FileIn(DirectoryInfo directoryInfo)
        {
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelFile_30min(dir);

                DelDir_30min_FileIn(dir);
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
                    Console.WriteLine(file.FullName + "\t" + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю файл");
                    file.Delete();
                }
            }            
        }
    }
}

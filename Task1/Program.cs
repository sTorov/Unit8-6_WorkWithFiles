﻿using System;
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
                Console.WriteLine("Не верно указан путь!");

        }
        static void DelDir_30min(DirectoryInfo directoryInfo)
        {
            try
            {
                DirectoryInfo[] dirs = directoryInfo.GetDirectories();
                foreach (var dir in dirs)
                {
                    DelFile_30min(dir);

                    TimeSpan sub = DateTime.Now - dir.LastAccessTime;
                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю");
                        dir.Delete(true);
                    }
                    
                    if(dir.Exists)
                        DelDir_30min(dir);
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
                        Console.WriteLine(file.FullName + "\t" + $"Не использовался {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю");
                        file.Delete();
                    }
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
    }
}

namespace Task3
{
    enum Mode
    {
        Manual = 0,
        Full,
        Empty,
        Files
    }

    class Program
    {
        public static int DirCount;
        public static int FileCount;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF7;
            
            long size = 0, afterDelSize = 0;
            string path = string.Empty;

            Console.WriteLine("Укажите путь к папке:");
            path = Console.ReadLine();

            DirectoryInfo dir = new DirectoryInfo(path);

            if (dir.Exists)
            {
                Console.WriteLine($"Исходный размер папки: {GetDirSize(dir, ref size)} байт\nПапки: {DirCount}\tФайлы: {FileCount}");
                
                int dirBeforeDel = DirCount;
                int fileBeforeDel = FileCount;
                Console.WriteLine("----------------------------------------------------------------------");

                Console.WriteLine("Выберете режим удаления:\n\n1. Удалять только пустые директории\n2. Ручной выбор\n3. Удалять все папки, включая вложенные файлы, использованные в прошедшие 30 минут\n4. Удалить только файлы");
                try
                {
                    while (true)
                    {
                        Console.WriteLine("\nНажмите '1' '2' '3' '4' соответственно");
                        var click = Console.ReadKey(true).KeyChar;

                        switch (click)
                        {
                            case '1':
                                DelDir_30min(dir, Mode.Empty);
                                break;
                            case '2':
                                DelDir_30min(dir, Mode.Manual);
                                break;
                            case '3':
                                DelDir_30min(dir, Mode.Full);
                                break;
                            case '4':
                                DelDir_30min(dir, Mode.Files);
                                break;
                            default:
                                continue;
                        }

                        DirCount = 0;
                        FileCount = 0;

                        Console.WriteLine("\n----------------------------------------------------------------------");
                        Console.WriteLine($"Текущий размер папки: {GetDirSize(dir, ref afterDelSize)} байт");
                        Console.WriteLine($"Освобождено: {size - afterDelSize} байт\nУдалено папок: {dirBeforeDel - DirCount}\tУдалено файлов: {fileBeforeDel - FileCount}");

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
        static void DelDir_30min(DirectoryInfo directoryInfo, Mode mode)
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

            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            foreach (var dir in dirs)
            {
                DelDir_30min(dir, mode);

                TimeSpan sub = DateTime.Now - dir.LastAccessTime;

                if (mode == Mode.Manual)
                {
                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                        {
                            Console.WriteLine(dir.Name + " " + $"Не использовалась {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю пустую папку");
                            dir.Delete();
                        }
                        else
                        {
                            Console.WriteLine($"В папке {dir.FullName} есть файлы и папки, использовавшиеся в прошлые 30 минут\nУдалить папку? (y / n)");

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
                if (mode == Mode.Full)
                {
                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(dir.Name + " " + $"Не использовалась {sub.Hours}:{sub.Minutes}:{sub.Seconds}" + "\tУдаляю папку");
                        dir.Delete(true);
                    }
                }
                if (mode == Mode.Empty)
                {
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
        }
        static long GetDirSize(DirectoryInfo dir, ref long filesize)
        {
            var files = dir.GetFiles();
            FileCount += dir.GetFiles().Length;

            foreach (var file in files)
                filesize += file.Length;

            var directories = dir.GetDirectories();
            DirCount += dir.GetDirectories().Length;

            foreach (var direct in directories)
                GetDirSize(direct, ref filesize);

            return filesize;
        }
    }
}

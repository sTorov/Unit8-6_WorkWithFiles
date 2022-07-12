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
            DirectoryInfo info = new DirectoryInfo(path);

            var Infos = (files : new List<FileInfo>(), dirs : new List<DirectoryInfo>());


            if (info.Exists)
            {
                Infos = GetInfos(path, Infos.files, Infos.dirs);

                size = GetDirSize(Infos.files, size);
                Console.WriteLine("" + size + " байт");

                Console.WriteLine("----------------------------------------------------------------------");

                Console.WriteLine("Выберете режим удаления:\n\n1. Удалять только пустые директории\n2. Ручной выбор\n3. Удалять все папки, включая вложенные файлы, использованные в прошедшие 30 минут\n4. Удалить только файлы");
                
                while (true)
                {
                    Console.WriteLine("\nНажмите '1' '2' '3' '4' соответственно");
                    var click = Console.ReadKey(true).KeyChar;

                    switch (click)
                    {
                        case '1':
                            DelDir_30min(Mode.Empty, Infos.dirs, Infos.files);
                            break;
                        case '2':
                            DelDir_30min(Mode.Manual, Infos.dirs, Infos.files);
                            break;
                        case '3':
                            DelDir_30min(Mode.Full, Infos.dirs, Infos.files);
                            break;
                        case '4':
                            DelDir_30min(Mode.Files, Infos.dirs, Infos.files);
                            break;
                        default:
                            continue;
                    }
                    //Console.WriteLine("\n----------------------------------------------------------------------");
                    //Console.WriteLine($"Текущий размер папки: {GetDirSize(dir, ref afterDelSize)} байт");
                    //Console.WriteLine($"Освобождено: {size - afterDelSize} байт\nУдалено папок: {dirBeforeDel - DirCount}\tУдалено файлов: {fileBeforeDel - FileCount}");

                    break;
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
        static void DelDir_30min(Mode mode, List<DirectoryInfo> dirs, List<FileInfo> files)
        {
            foreach (var file in files)
            {
                try
                {
                    TimeSpan sub = DateTime.Now - file.LastAccessTime;

                    if (sub > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(file.FullName + $"\t\tНе использовался более 30 минут\t\tУдаляю файл");
                        file.Delete();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            foreach (var dir in dirs)
            {
                try
                {
                    TimeSpan sub = DateTime.Now - dir.LastAccessTime;

                    if (mode == Mode.Manual)
                    {
                        if (sub > TimeSpan.FromMinutes(30))
                        {
                            if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                            {
                                Console.WriteLine(dir.FullName + $"\t\tНе использовалась более 30 минут\tУдаляю пустую папку");
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
                                            Console.WriteLine($"Удаляю {dir.FullName}");
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
                            Console.WriteLine(dir.FullName + $"\t\tНе использовалась более 30 минут\tУдаляю папку");
                            dir.Delete(true);
                        }
                    }
                    if (mode == Mode.Empty)
                    {
                        if (sub > TimeSpan.FromMinutes(30))
                        {
                            if (dir.GetFiles().Length == 0 && dir.GetDirectories().Length == 0)
                            {
                                Console.WriteLine(dir.FullName + $"\t\tНе использовалась более 30 минут\tУдаляю пустую папку");
                                dir.Delete();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }                
            }
        }
        static long GetDirSize(List<FileInfo> files, long filesize)
        {
            foreach (var file in files)
            {
                filesize += file.Length;
                Console.WriteLine(file.FullName + "\t" + file.Length + " байт");
            }
            
            return filesize;
        }
        static (List<FileInfo> lsFile, List<DirectoryInfo> lsDir) GetInfos(string path, List<FileInfo> lsFileInfos, List<DirectoryInfo> lsDirInfo)
        {
            try
            {
                var DirInfo = new DirectoryInfo(path);
                var Files = DirInfo.GetFiles();

                foreach (var File in Files)
                    lsFileInfos.Add(File);

                var Dirs = DirInfo.GetDirectories();

                foreach (var Dir in Dirs)
                {
                    lsDirInfo.Add(Dir);
                    GetInfos(Dir.FullName, lsFileInfos, lsDirInfo);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return (lsFileInfos, lsDirInfo);
        }
    }
}

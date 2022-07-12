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
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF7;

            long size = 0, afterDelSize = 0;
            string path = string.Empty;

            Console.WriteLine("Укажите путь к папке:");
            path = Console.ReadLine();
            DirectoryInfo info = new DirectoryInfo(path);

            var Infos = (files: new List<FileInfo>(), dirs: new List<DirectoryInfo>());


            if (info.Exists)
            {
                Infos = GetInfos(path, Infos.files, Infos.dirs);
                int dirs = Infos.dirs.Count;
                int files = Infos.files.Count;

                size = GetDirSize(Infos.files, size);
                Console.WriteLine("Исходный размер папки: " + size + " байт");
                Console.WriteLine($"Папки: {dirs}\tФайлы: {files}");

                Console.WriteLine("----------------------------------------------------------------------");

                Console.WriteLine("Выберете режим удаления:\n\n1. Удалять только пустые директории\n2. Ручной выбор\n3. Удалять все папки, включая вложенные файлы, использованные в прошедшие 30 минут\n4. Удалить только файлы");

                while (true)
                {
                    Console.WriteLine("\nНажмите '1' '2' '3' '4' соответственно\n");
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
                    Console.WriteLine("\n----------------------------------------------------------------------");

                    Infos.files.Clear();
                    Infos.dirs.Clear();
                    Infos = GetInfos(path, Infos.files, Infos.dirs);
                    afterDelSize = GetDirSize(Infos.files, afterDelSize);

                    Console.WriteLine($"Текущий размер папки: {afterDelSize} байт");
                    Console.WriteLine($"Освобождено: {size - afterDelSize} байт\nУдалено папок: {dirs - Infos.dirs.Count}\tУдалено файлов: {files - Infos.files.Count}");

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
            for (int i = files.Count - 1; i >= 0; i--)
            {
                try
                {
                    if ((DateTime.Now - files[i].LastAccessTime) > TimeSpan.FromMinutes(30))
                    {
                        Console.WriteLine(files[i].FullName + $"\t\tНе использовался более 30 минут\t\tУдаляю файл");
                        files[i].Delete();
                    }
                }
                catch (Exception e)
                {
                    PrintException(e);
                }
            }

            for (int i = dirs.Count - 1; i >= 0; i--)
            {
                if (mode == Mode.Manual)
                {
                    try
                    {
                        if ((DateTime.Now - dirs[i].LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            if (dirs[i].GetFiles().Length == 0 && dirs[i].GetDirectories().Length == 0)
                            {
                                Console.WriteLine(dirs[i].FullName + $"\t\tНе использовалась более 30 минут\tУдаляю пустую папку");
                                dirs[i].Delete();
                            }
                            else
                            {
                                Console.WriteLine($"В папке {dirs[i].FullName} есть файлы и папки, использовавшиеся в прошлые 30 минут\nУдалить папку? (y / n)");

                                while (true)
                                {
                                    var tap = Console.ReadKey(true);
                                    switch (tap.KeyChar)
                                    {
                                        case 'y':
                                            dirs[i].Delete(true);
                                            Console.WriteLine($"Удаляю {dirs[i].FullName}");
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
                    catch (Exception e)
                    {
                        PrintException(e);
                    }
                }
                if (mode == Mode.Full)
                {
                    try
                    {
                        if ((DateTime.Now - dirs[i].LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            Console.WriteLine(dirs[i].FullName + $"\t\tНе использовалась более 30 минут\tУдаляю папку");
                            dirs[i].Delete(true);
                        }
                    }
                    catch (Exception e)
                    {
                        PrintException(e);
                    }
                }
                if (mode == Mode.Empty)
                {
                    try
                    {
                        if ((DateTime.Now - dirs[i].LastAccessTime) > TimeSpan.FromMinutes(30))
                        {
                            if (dirs[i].GetFiles().Length == 0 && dirs[i].GetDirectories().Length == 0)
                            {
                                Console.WriteLine(dirs[i].FullName + $"\t\tНе использовалась более 30 минут\tУдаляю пустую папку");
                                dirs[i].Delete();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        PrintException(e);
                    }
                }
            }
        }
        static long GetDirSize(List<FileInfo> files, long filesize)
        {
            foreach (var file in files)
                filesize += file.Length;

            return filesize;
        }
        static (List<FileInfo> lsFile, List<DirectoryInfo> lsDir) GetInfos(string path, List<FileInfo> lsFileInfos, List<DirectoryInfo> lsDirInfo)
        {
            var DirInfo = new DirectoryInfo(path);

            try
            {
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
                PrintException(e);
            }

            return (lsFileInfos, lsDirInfo);
        }

        static void PrintException(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка: " + e.Message);
            Console.ResetColor();
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        long size = 0;
        Console.WriteLine("Укажите путь к папке:");
        string path = Console.ReadLine();
        Console.WriteLine();

        DirectoryInfo directory = new DirectoryInfo(path);

        if(directory.Exists)
        {
            try
            {
                size = GetDirSize(directory, ref size);
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Ошибка: " + ex.Message);
                Console.ResetColor();
            }

            Console.WriteLine("Размер: " + size + " байт");
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Такой директории не существует!");
            Console.ResetColor();
        }
    }
    static long GetDirSize(DirectoryInfo dir, ref long filesize)
    {
        filesize = GetFileSize(dir, filesize);

        var directories = dir.GetDirectories();

        foreach (var direct in directories)
        {
            Console.WriteLine(direct.Name);
            GetDirSize(direct, ref filesize);
        }

        return filesize;
    }
    static long GetFileSize(DirectoryInfo dir, long filesize)
    {
        var files = dir.GetFiles();

        foreach (var file in files)
        {
            Console.WriteLine($"\t{file.Name}\t{file.Length}");
            filesize += file.Length;
        }

        return filesize;
    }
}
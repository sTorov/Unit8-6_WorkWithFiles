class Program
{
    static void Main(string[] args)
    {
        long size = 0;

        Console.WriteLine("Укажите путь к папке:");
        string path = Console.ReadLine();
        DirectoryInfo directory = new DirectoryInfo(path);

        var Info = new List<FileInfo>();

        if (directory.Exists)
        {
            Info = GetInfos(path, Info);
            size = GetDirSize(Info, size);

            Console.WriteLine("\nРазмер: " + size + " байт");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Такой директории не существует!");
            Console.ResetColor();
        }
    }
    /// <summary>
    /// Возвращает список всех файлов в указанной директории и её подпапках
    /// </summary>
    /// <param name="path"></param>
    /// <param name="lsFileInfos"></param>
    /// <returns></returns>
    static List<FileInfo> GetInfos(string path, List<FileInfo> lsFileInfos)
    {
        var DirInfo = new DirectoryInfo(path);

        try
        {
            var Files = DirInfo.GetFiles();

            foreach (var File in Files)
                lsFileInfos.Add(File);

            var Dirs = DirInfo.GetDirectories();

            foreach (var Dir in Dirs)
                GetInfos(Dir.FullName, lsFileInfos);
        }
        catch (Exception e)
        {
            PrintException(e);
        }

        return lsFileInfos;
    }
    /// <summary>
    /// Считает размер файлов в указанной директории и её подпапках
    /// </summary>
    /// <param name="files"></param>
    /// <param name="filesize"></param>
    /// <returns></returns>
    static long GetDirSize(List<FileInfo> files, long filesize)
    {
        foreach (var file in files)
            filesize += file.Length;

        return filesize;
    }
    /// <summary>
    /// Меняет цвет сообщения об ошибке
    /// </summary>
    /// <param name="e"></param>
    static void PrintException(Exception e)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Ошибка: " + e.Message);
        Console.ResetColor();
    }
}
class Program
{
    static void Main(string[] args)
    {
        long size = 0;
        Console.WriteLine("Укажите путь к папке:");
        string path = @"D:\!!!Test";
        Console.WriteLine();

        DirectoryInfo directory = new DirectoryInfo(path);

        if(directory.Exists)
        {
            Console.WriteLine(directory.FullName);
            size = GetDirSize(directory, ref size);
            Console.WriteLine("Размер: " + size + " байт");
        }
    }
    static long GetDirSize(DirectoryInfo dir, ref long filesize)
    {
        filesize = GetFileSize(dir, filesize);

        var directories = dir.GetDirectories();

        foreach (var direct in directories)
            GetDirSize(direct, ref filesize);

        return filesize;
    }
    static long GetFileSize(DirectoryInfo dir, long filesize)
    {
        var files = dir.GetFiles();

        foreach (var file in files)
            filesize += file.Length;

        return filesize;
    }
}
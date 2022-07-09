class Program
{
    static void Main(string[] args)
    {
        string path = @"D:\!!!Test";
        DirectoryInfo directory = new DirectoryInfo(path);

        if(directory.Exists)
            Console.WriteLine(directory.FullName);
            GetDirSize(directory);
    }
    static void GetDirSize(DirectoryInfo dir)
    {
        GetFileSize(dir);

        var directories = dir.GetDirectories();
        foreach (var direct in directories)
        {
            Console.WriteLine(direct.FullName);

            GetDirSize(direct);
        }
    }
    static void GetFileSize(DirectoryInfo dir)
    {
        var files = dir.GetFiles();
        foreach (var file in files)
        {
            Console.WriteLine("\t" + file.Name + " " + file.Length + " байт");
        }
    }
}
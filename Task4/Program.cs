[Serializable]
class Student
{
    public string Name { get; set; }
    public string Group { get; set; }
    public DateTime DateOfBirth { get; set; }
}
class Program
{
    public string dat = Path.Combine(@"C:\Users", Environment.UserName, "Desktop", "Students.dat");
    static void Main()
    {
        string path = Path.Combine(@"C:\Users", Environment.UserName, "Desktop", "Students");

        if(Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
}
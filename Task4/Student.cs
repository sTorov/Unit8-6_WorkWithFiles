namespace FinalTask
{
    [Serializable]
    public class Student
    {
        public string Name { get; set; }
        public string Group { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Student(string name, string group, DateTime date)
        {
            Name = name;
            Group = group;
            DateOfBirth = date;
        }
    }
}

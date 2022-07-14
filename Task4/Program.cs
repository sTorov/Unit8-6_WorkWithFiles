﻿using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{    
    class Program
    {
        static void Main()
        {
            string dat = Path.Combine(@"C:\Users", Environment.UserName, "Desktop", "Students.dat");
            string path = Path.Combine(@"C:\Users", Environment.UserName, "Desktop", "Students");

            if (Directory.Exists(path))
                Directory.CreateDirectory(path);

            BinaryFormatter bf = new BinaryFormatter();
            using (var fs = new FileStream(dat, FileMode.OpenOrCreate))
            {
                var students = (Student[])bf.Deserialize(fs);
                foreach(var student in students)
                {
                    Console.WriteLine($"{student.Name}\t\t{student.Group}\t{student.DateOfBirth}");
                }
            }
        }
    }
}
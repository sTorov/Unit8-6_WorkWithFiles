using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{    
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Укажите путь к файлу Student.dat:");

            try
            {
                while (true)
                {
                    string dat = Console.ReadLine();

                    if (Path.GetFileName(dat) == "Students.dat")
                    {
                        PrintStudentToTxt(dat);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Неверно указан путь! Попробуйте снова.");
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                PrintException(e);
                Console.WriteLine("Нажмите любую кнопку для завершения работы приложения");
            }


            Console.ReadKey();
        }
        /// <summary>
        /// Дисериализует файл Students.dat. Раскидывает студентов по файлам (каждый файл - отдельная группа), 
        /// в файле группы студенты перечислены построчно в формате "Имя, дата рождения".
        /// </summary>
        /// <param name="dat"></param>
        static void PrintStudentToTxt(string dat)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            string dir = CreateDirToFileName(dat);

            using (var fileStream = new FileStream(dat, FileMode.OpenOrCreate))
            {
                var students = (Student[])binaryFormatter.Deserialize(fileStream);

                foreach (var student in students)
                    File.Delete(Path.Combine(dir, student.Group + ".txt"));

                foreach (var student in students)
                {
                    if (!File.Exists(Path.Combine(dir, student.Group + ".txt")))
                    {
                        using (var stream = File.CreateText(Path.Combine(dir, student.Group + ".txt")))
                            stream.WriteLine($"{"Имя",-15} Дата рождения\n");
                    }

                    using (var sw = File.AppendText(Path.Combine(dir, student.Group + ".txt")))
                        sw.WriteLine($"{student.Name,-15} {student.DateOfBirth:D}");
                }

                Console.WriteLine("Процесс завершен. Нажмите любую кнопку.");
            }
        }
        /// <summary>
        /// Создаёт папку на рабочем столе с название, идентичным названию файла. Возвращает путь к этой папке.  
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        static string CreateDirToFileName(string dat)
        {
            string dir = Path.Combine(@"C:\Users", Environment.UserName, "Desktop", Path.GetFileNameWithoutExtension(dat));

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return dir;
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
}
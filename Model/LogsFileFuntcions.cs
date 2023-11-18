using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject.Model
{
    public class LogsFileFuntcions
    {
        string path;
        public string Path
        {
            get {  return path; }
            set { Path = value; }
        }
        public LogsFileFuntcions(string path)
        {
            Path = path;
        }

        public void WriteToLogsFile(string message)
        {
            try
            {
                if (!File.Exists(path)) { File.Create(path); }
                string time = DateTime.Now.ToString();
                StreamWriter streamWriter = new StreamWriter(path, true);
                streamWriter.WriteLine($"{time}: {message}");
                streamWriter.Close();
                return;
            }
            catch (UnauthorizedAccessException) { Console.WriteLine("Ошибка доступа к logsFile"); }
            catch (ArgumentException) { Console.WriteLine("Некорректный путь к logsFile"); }
            catch (Exception ex) { Console.WriteLine($"Ошибка {ex.GetType().Name} при обращении к logsFile"); }
            Console.WriteLine("Не удалось записать данные в logsFile");
            return;
        }
    }
}

using FormProject.Controller;
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
        public LogsFileFuntcions(string path)
        {
            this.path = path;
        }

        public void HelpWriteToLogsFile(string message)
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
        }

        public void WriteToLogsFile(string problem, string login, object access = null)
        {
            string message;
            LogsFileFuntcions logsFileFuntcions = new LogsFileFuntcions("logsFile.txt");
            if (access == null)
            {
                bool isAdmin = DBHelpFunctional.HelpIsAdmin(login, out string dbProblem);
                if (dbProblem != "")
                {
                    Console.WriteLine(dbProblem);
                    access = "error";
                    message = $"Пользователь: {login} | Права: {access} | Ошибка: {dbProblem}";
                    logsFileFuntcions.HelpWriteToLogsFile(message);
                }
                else access = Convert.ToUInt32(isAdmin);
            }
            message = $"Пользователь: {login} | Права: {access} | Ошибка: {problem}";
            logsFileFuntcions.HelpWriteToLogsFile(message);
        }
    }
}

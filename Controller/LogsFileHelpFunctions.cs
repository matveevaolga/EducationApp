using FormProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject.Controller
{
    public class LogsFileHelpFunctions
    {
        public void HelpWriteToLogsFile(string problem, string login)
        {
            LogsFileFuntcions logsFileFuntcions = new LogsFileFuntcions("logsFile.txt");
            bool isAdmin = DBHelpFunctional.HelpIsAdmin(login, out string dbProblem);
            if (dbProblem != "") { Console.WriteLine($"Ошибка {dbProblem} при обращении к бд. " +
                "Невозможно записать данные в logsFile"); return;}
            string message = $"Пользователь: {login} | Права: {Convert.ToUInt32(isAdmin)} | Ошибка: {problem}";
            logsFileFuntcions.WriteToLogsFile(message);
        }
    }
}

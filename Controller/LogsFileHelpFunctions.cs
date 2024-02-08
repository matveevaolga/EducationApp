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
        public static void HelpWriteToLogsFile(string problem, string login, object access = null)
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

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormProject.Controller
{
    public class DBHelpFunctional
    {
        private static void GetDBFunctions(out DBFunctions dBFunctions,
            string problemText, out string problem, string login)
        {
            dBFunctions = null;
            try
            {
                problem = "";
                dBFunctions = new DBFunctions();
            }
            catch (MySqlException ex)
            {
                string message = $"Ошибка при подключении к бд в ф-ции GetDBFunctions, номер ошибки {ex.Number}";
                Console.WriteLine(message);
                LogsFileHelpFunctions.HelpWriteToLogsFile(message, login);
                problem = problemText;
            }
        }

        private static void GetDBFunctions(out DBFunctions dBFunctions, string problemText, string login)
        {
            dBFunctions = null;
            try
            {
                dBFunctions = new DBFunctions();
            }
            catch (MySqlException ex)
            {
                string message = $"Ошибка при подключении к бд в ф-ции GetDBFunctions, номер ошибки {ex.Number}";
                Console.WriteLine(message);
                LogsFileHelpFunctions.HelpWriteToLogsFile(message, login);
            }
        }

        public static string HelpGetProfileField(string login, string field)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось получить поле", login);
            if (dBFunctions == null) { return ""; }
            return dBFunctions.GetProfileField(login, field);
        }

        public static string HelpGetStatsField(string login, string field)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось получить поле", login);
            if (dBFunctions == null) { return "Не удалось получить поле"; }
            return dBFunctions.GetStatsField(login, field);
        }

        public static bool HelpChangeField(string login, string table,
            string column, string value, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось изменить поле", out problem, login);
            if (dBFunctions == null) { return false; }
            return dBFunctions.ChangeField(login, table, column, value);
        }

        public static bool HelpIsRegistered(string login, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem, login);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsRegistered(login, out problem);
        }

        public static bool HelpRegister(string login, string password, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem, login);
            if (dBFunctions == null) { return false; }
            return dBFunctions.Register(login, password);
        }

        public static bool HelpIsPassCorrect(string login, string password, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem, login);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsPassCorrect(login, password, out problem);
        }

        public static bool HelpIsAdmin(string login, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem, login);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsAdmin(login, out problem);
        }

        public static List<Dictionary<string, string>> HelpGetExersices(out string problem, string login)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem, login);
            if (dBFunctions == null) { return null; }
            return dBFunctions.GetExercises(out problem, login);
        }

        public static void HelpIncreaseEXP(string login, int exp)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", login);
            if (dBFunctions == null) { return; }
            dBFunctions.IncreaseEXP(login, exp);
        }
    }
}

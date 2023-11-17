﻿using MySql.Data.MySqlClient;
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
        private static void GetDBFunctions(out DBFunctions dBFunctions, string problemText, out string problem)
        {
            dBFunctions = null;
            try
            {
                problem = "";
                dBFunctions = new DBFunctions();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка при подключении к бд в ф-ции GetDBFunctions, номер ошибки {ex.Number}");
                problem = problemText;
            }
        }

        private static void GetDBFunctions(out DBFunctions dBFunctions, string problemText)
        {
            dBFunctions = null;
            try
            {
                dBFunctions = new DBFunctions();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка при подключении к бд в ф-ции GetDBFunctions, номер ошибки {ex.Number}");
                return;
            }
        }

        public static string HelpGetProfileField(string login, string field)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось получить поле");
            if (dBFunctions == null) { return ""; }
            return dBFunctions.GetProfileField(login, field);
        }

        public static string HelpGetStatsField(string login, string field)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось получить поле");
            if (dBFunctions == null) { return "Не удалось получить поле"; }
            return dBFunctions.GetStatsField(login, field);
        }

        public static bool HelpChangeField(string login, string table,
            string column, string value, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Не удалось изменить поле", out problem);
            if (dBFunctions == null) { return false; }
            return dBFunctions.ChangeField(login, table, column, value);
        }

        public static bool HelpIsRegistered(string login, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsRegistered(login, out problem);
        }

        public static bool HelpRegister(string login, string password, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem);
            if (dBFunctions == null) { return false; }
            return dBFunctions.Register(login, password);
        }

        public static bool HelpIsPassCorrect(string login, string password, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsPassCorrect(login, password, out problem);
        }

        public static bool HelpIsAdmin(string login, out string problem)
        {
            DBFunctions dBFunctions;
            GetDBFunctions(out dBFunctions, "Произошла программная ошибка", out problem);
            if (dBFunctions == null) { return false; }
            return dBFunctions.IsAdmin(login, out problem);
        }
    }
}

using FormProject.Controller;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FormProject
{
    public class DBFunctions
    {
        DBConnection connectorToDb;
        DataTable table;
        MySqlDataAdapter cursor;
        const string dbException = "ошибка в функции {0} при обращении к бд" +
                    ", номер ошибки {1}";
        const string nullException = "ошибка в функции {0}, " +
                    "обращение к полю null, имя ошибки NullReferenceException";
        const string accessException = "ошибка доступа к бд в функции {0}," +
            " номер ошибки {1} (нверный логин/пароль)";
        const string dbDoesntExistException = "ошибка в функции {0} при обращении к бд," +
            " номер ошибки {1} (бд не найдена).";
        const string tableDoesntExistException = "ошибка в функции {0} при обращении к бд," +
            " номер ошибки {1} (таблица не найдена).";
        const string columnDoesntExistException = "ошибка в функции {0} при обращении к бд," +
        " номер ошибки {1} (колонка не найдена).";
        const string cantConnectException = "ошибка в функции {0} при обращении к бд," +
        " номер ошибки {1} (не вышло подключиться к бд).";

        public DBFunctions()
        {
            connectorToDb = new DBConnection();
            cursor = new MySqlDataAdapter();
        }

        string GetMysqlException(MySqlException ex)
        {
            string exception;
            switch (ex.Number)
            {
                case 0:
                    exception = cantConnectException;
                    break;
                case 1045:
                    exception = accessException;
                    break;
                case 1050:
                    exception = dbDoesntExistException;
                    break;
                case 1051:
                    exception = tableDoesntExistException;
                    break;
                case 1055:
                    exception = columnDoesntExistException;
                    break;
                default:
                    exception = dbException;
                    break;
            };
            return exception;
        }

        public bool IsPassCorrect(string login, string password, out string problem)
        {
            problem = "";
            try
            {
                table = new DataTable();
                connectorToDb.OpenConnection();
                MySqlCommand registered = new MySqlCommand("select pass from users " +
                    "where login = @uLogin limit 1;", connectorToDb.GetConnection());
                registered.Parameters.AddWithValue("@uLogin", login);
                object storedPass = registered.ExecuteScalar();
                connectorToDb.CloseConnection();
                return storedPass.ToString() == password;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "IsPassCorrect", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "IsPassCorrect", ex.Number), login);
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "IsPassCorrect");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "IsPassCorrect"), login);
                problem = "программная ошибка";
            }
            return false;
        }

        public bool IsRegistered(string login, out string problem)
        {
            problem = "";
            try
            {
                table = new DataTable();
                connectorToDb.OpenConnection();
                MySqlCommand registered = new MySqlCommand("select * from users " +
                    "where login = @uLogin;", connectorToDb.GetConnection());
                registered.Parameters.AddWithValue("@uLogin", login);
                cursor.SelectCommand = registered;
                cursor.Fill(table);
                connectorToDb.CloseConnection();
                return table.Rows.Count > 0;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(dbException, "IsRegistered", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(dbException, "IsRegistered", ex.Number), login);
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "IsRegistered");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "IsRegistered"), login);
                problem = "программная ошибка";
            }
            return true;
        }

        public bool Register(string login, string password) 
        {
            try
            {
                connectorToDb.OpenConnection();
                bool users = AddToUsersTable(login, password);
                connectorToDb.CloseConnection();
                return users;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "Regiser", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "Regiser", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "Regiser");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "Register"), login);
            }
            return false;
        }

        private void DeleteStats(int statsID, string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                Console.WriteLine("delete");
                MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @sID", connectorToDb.GetConnection());
                delete.Parameters.AddWithValue("@sID", statsID);
                delete.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "DeleteStats", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "DeleteStats", ex.Number), login);
            }
        }

        private void DeleteProfile(int profileID, string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @pID", connectorToDb.GetConnection());
                delete.Parameters.AddWithValue("@pID", profileID);
                delete.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "DeleteProfile", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "DeleteProfile", ex.Number), login);
            }
        }

        private bool AddToUsersTable(string login, string password)
        {
            int statsID = AddToStatsTable(login);
            int profileID = AddToProfilesTable(login);
            if (statsID == 0 | profileID == 0) 
            {
                if (statsID != 0) { DeleteStats(statsID, login); }
                if (profileID != 0) { DeleteProfile(profileID, login); }
                return false; 
            }
            try
            {
                connectorToDb.OpenConnection();
                string insertLogPass = "insert into users (login, pass, idStats, idProfile) " +
                    "values (@uLogin, @uPass, @uStatsId, @uProfileId);";
                MySqlCommand register = new MySqlCommand(insertLogPass, connectorToDb.GetConnection());
                register.Parameters.AddWithValue("@uLogin", login);
                register.Parameters.AddWithValue("@uPass", password);
                register.Parameters.AddWithValue("@uStatsId", statsID);
                register.Parameters.AddWithValue("@uProfileId", profileID);
                register.ExecuteNonQuery();
                connectorToDb.CloseConnection();
                return true;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "AddToUsersTable", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "AddToUsersTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "AddToUsersTable");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "AddToUsersTable"), login);
            }
            return false;
        }

        private int AddToProfilesTable(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand idProfileInsert = new MySqlCommand("insert into profiles (about) values ('unfilled');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from profiles;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                connectorToDb.CloseConnection();
                return Convert.ToInt32(currentID.ToString());
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "AddToProfilesTable", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "AddToProfilesTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "AddToProfilesTable");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "AddToProfilesTable"), login);
            }
            return 0;
        }

        private int AddToStatsTable(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand idProfileInsert = new MySqlCommand("insert into stats (solved, unsolved, сoveredTopics) values ('', '', '');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from stats;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                connectorToDb.CloseConnection();
                return Convert.ToInt32(currentID.ToString());
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "AddToStatsTable", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "AddToStatsTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "AddToStatsTable");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "AddToStatsTable"), login);
            }
            return 0;
        }

        private int ProfileIDByLog(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                string getUserId = "select idProfile from users where login = @uLogin;";
                MySqlCommand commandID = new MySqlCommand(getUserId, connectorToDb.GetConnection());
                commandID.Parameters.AddWithValue("@uLogin", login);
                object userID = commandID.ExecuteScalar();
                connectorToDb.CloseConnection();
                if (userID == null) { return 0; }
                return (int)userID;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "ProfileIDByLog", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "ProfileIDByLog", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "ProfileIDByLog");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "ProfileIDByLog"), login);
            }
            return 0;
        }

        private int StatsIDByLog(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                string getUserId = "select idStats from users where login = @uLogin;";
                MySqlCommand commandID = new MySqlCommand(getUserId, connectorToDb.GetConnection());
                commandID.Parameters.AddWithValue("@uLogin", login);
                object userID = commandID.ExecuteScalar();
                connectorToDb.CloseConnection();
                if (userID == null) { return 0; }
                return (int)userID;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "StatsIDByLog", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "StatsIDByLog", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "StatsIDByLog");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "StatsIDByLog"), login);
            }
            return 0;
        }

        public string GetProfileField(string login, string column)
        {
            int profileId = ProfileIDByLog(login);
            if (profileId == 0) { return "ошибка"; };
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select {column} from profiles where idProfile = @uId;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.Parameters.AddWithValue("@uID", profileId);
                object field = command.ExecuteScalar();
                connectorToDb.CloseConnection();
                if (field == null) { return null; }
                return field.ToString();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetStatsField", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "GetStatsField", ex.Number), login);
                return "программная ошибка";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "GetStatsField");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "GetStatsField"), login);
                return "ошибка, не удалось получить поле";
            }
        }

        public string GetStatsField(string login, string column)
        {
            int statsId = StatsIDByLog(login);
            if (statsId == 0) { return "ошибка"; }
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select {column} from stats where idStats = @uId;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.Parameters.AddWithValue("@uID", statsId);
                object field = command.ExecuteScalar();
                connectorToDb.CloseConnection();
                if (field == null) { return null; }
                return field.ToString();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetStatsField", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "GetStatsField", ex.Number), login);
                return "программная ошибка";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "GetStatsField");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "GetStatsField"), login);
                return "ошибка, не удалось получить поле";
            }
        }

        public bool ChangeField(string login, string table, string column, string value)
        {
            int profileID = ProfileIDByLog(login);
            if (profileID == 0) 
            {
                Console.WriteLine("ChangeField");
                return false;
            }
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand insert = new MySqlCommand($"update {table} set {column} = '{value}'" +
                    $" where idProfile = {profileID};", connectorToDb.GetConnection());
                insert.ExecuteNonQuery();
                connectorToDb.CloseConnection();
                return true;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "ChangeField", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "ChangeField", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "ChangeField");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "ChangeField"), login);
            }
            return false;
        }

        public bool IsAdmin(string login, out string problem)
        {
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select isAdmin from users where login = @uLogin;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.Parameters.AddWithValue("@uLogin", login);
                bool field = Convert.ToBoolean(command.ExecuteScalar());
                connectorToDb.CloseConnection();
                problem = "";
                return field;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "IsAdmin", ex.Number);
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(exception, "IsAdmin", ex.Number), login, "error");
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(nullException, "IsAdmin");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(nullException, "IsAdmin"), login, "error");
                problem = "программная ошибка";
            }
            return false;
        }
    }
}

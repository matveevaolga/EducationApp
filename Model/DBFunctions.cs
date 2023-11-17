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

        public DBFunctions()
        {
            connectorToDb = new DBConnection();
            cursor = new MySqlDataAdapter();
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
                return storedPass.ToString() == password;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции IsPassCorrect, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                problem = "программная ошибка";
                return false;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции IsPassCorrect при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                problem = "ошибка регистрации";
                connectorToDb.CloseConnection();
                return false;
            }
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
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции IsRegistered, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                problem = "программная ошибка";
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции IsRegistered при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                problem = "ошибка регистрации";
                connectorToDb.CloseConnection();
                return true;
            }
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
                Console.WriteLine($"ошибка в функции Register при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return false;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции Register, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                connectorToDb.CloseConnection();
                return false;
            }
        }

        private void DeleteStats(int statsID)
        {
            try
            {
                MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @sID", connectorToDb.GetConnection());
                delete.Parameters.AddWithValue("@sID", statsID);
                delete.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции DeleteStats при обращении к бд" +
                    $", номер ошибки {ex.Number}");
            }
        }

        private void DeleteProfile(int profileID)
        {
            try
            {
                MySqlCommand delete = new MySqlCommand("delete from stats where idStats = @pID", connectorToDb.GetConnection());
                delete.Parameters.AddWithValue("@pID", profileID);
                delete.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции DeleteProfile при обращении к бд" +
                    $", номер ошибки {ex.Number}");
            }
        }

        private bool AddToUsersTable(string login, string password)
        {
            int statsID = AddToStatsTable();
            int profileID = AddToProfilesTable();
            if (statsID == 0 | profileID == 0) 
            { 
                if (statsID != 0) { DeleteStats(statsID); }
                if (profileID != 0) { DeleteProfile(profileID); }
                return false; 
            }
            try
            {
                string insertLogPass = "insert into users (login, pass, idStats, idProfile) " +
                    "values (@uLogin, @uPass, @uStatsId, @uProfileId);";
                MySqlCommand register = new MySqlCommand(insertLogPass, connectorToDb.GetConnection());
                register.Parameters.AddWithValue("@uLogin", login);
                register.Parameters.AddWithValue("@uPass", password);
                register.Parameters.AddWithValue("@uStatsId", statsID);
                register.Parameters.AddWithValue("@uProfileId", profileID);
                register.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции AddToUsersTable при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return false;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции AddToUsersTable, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return false;
            }
        }

        private int AddToProfilesTable()
        {
            try
            {
                MySqlCommand idProfileInsert = new MySqlCommand("insert into profiles (about) values ('unfilled');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from profiles;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                return Convert.ToInt32(currentID.ToString());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции AddToProfilesTable при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции AddToProfilesTable, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return 0;
            }
        }

        private int AddToStatsTable()
        {
            try
            {
                MySqlCommand idProfileInsert = new MySqlCommand("insert into stats (solved, unsolved, сoveredTopics) values ('', '', '');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from stats;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                return Convert.ToInt32(currentID.ToString());
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"ошибка в функции AddToStatsTable при обращении к бд" +
                    $", номер ошибки {ex.Message}");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции AddToStatsTable, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return 0;
            }
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
                Console.WriteLine($"ошибка в функции ProfileIDByLog при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции ProfileIDByLog, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return 0;
            }
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
                Console.WriteLine($"ошибка в функции StatsIDByLog при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции StatsIDByLog, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return 0;
            }
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
                Console.WriteLine($"ошибка в функции GetProfileField при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return "ошибка, не удалось получить поле";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции GetProfileField, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return "программная ошибка";
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
                Console.WriteLine($"ошибка в функции GetStatsField при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return "ошибка, не удалось получить поле";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции GetStatsField, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return "программная ошибка";
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
                Console.WriteLine($"ошибка в функции ChangeField при обращении к бд" +
                    $", номер ошибки {ex.Number}");
                return false;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ошибка в функции ChangeField, " +
                    "обращение к полю null, имя ошибки NullReferenceException");
                return false;
            }
        }

        public bool isAdmin(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select isAdmin from users where login = @uLogin;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.Parameters.AddWithValue("@uLogin", login);
                bool field = Convert.ToBoolean(command.ExecuteScalar());
                connectorToDb.CloseConnection();
                return field;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ошибка при обращении к бд");
                return false;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("обращение к null");
                return false;
            }
        }
    }
}

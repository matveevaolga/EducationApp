using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FormProject
{
    internal class DBFunctions
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
                problem = "Не удалось подключиться к базе данных.";
                Console.WriteLine("IsPassCorrect");
                return false;
            }
            catch (MySqlException)
            {
                problem = "Не удалось совершить запрос к базе данных.";
                Console.WriteLine("IsPassCorrect");
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
                problem = "Не удалось подключиться к базе данных.";
                return true;
            }
            catch (MySqlException)
            {
                Console.WriteLine("IsRegisered");
                problem = "Возникла ошибка во время исполнения запроса к базе данных";
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
            catch (NullReferenceException)
            {
                Console.WriteLine("Register");
                return false;
            }
            catch (MySqlException)
            {
                Console.WriteLine("Register");
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
            catch (MySqlException)
            {
                return;
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
            catch (MySqlException)
            {
                return;
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
            catch (MySqlException)
            {
                Console.WriteLine("AddToUsersTable");
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
            catch (MySqlException)
            {
                Console.WriteLine("AddToProfilesTable");
                return 0;
            }
        }

        private int AddToStatsTable()
        {
            try
            {
                MySqlCommand idProfileInsert = new MySqlCommand("insert into stats (solved, unsolved, coveredTopics) values ('', '', '');", connectorToDb.GetConnection());
                idProfileInsert.ExecuteNonQuery();
                MySqlCommand getCurrentID = new MySqlCommand("select last_insert_id() from stats;", connectorToDb.GetConnection());
                object currentID = getCurrentID.ExecuteScalar();
                if (currentID == null) { return 0; }
                return Convert.ToInt32(currentID.ToString());
            }
            catch (MySqlException)
            {
                Console.WriteLine("AddToStatsTable");
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
            catch (MySqlException)
            {
                Console.WriteLine("ProfileIDByLog");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ProfileIDByLog");
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
            catch (MySqlException)
            {
                Console.WriteLine("StatsIDByLog");
                return 0;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("StatsIDByLog");
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
            catch (MySqlException)
            {
                Console.WriteLine("StatsIDByLog");
                return "ошибка, не удалось изменить поле";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("StatsIDByLog");
                return "ошибка, не удалось изменить поле";
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
            catch (MySqlException)
            {
                Console.WriteLine("StatsIDByLog");
                return "ошибка, не удалось изменить поле";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("StatsIDByLog");
                return "ошибка, не удалось изменить поле";
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
            catch (MySqlException)
            {
                Console.WriteLine("ChangeField");
                return false;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("ChangeField");
                return false;
            }
        }
    }
}

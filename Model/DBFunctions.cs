using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FormProject.Model;
using System.Resources;
using Microsoft.Win32;
using static IronPython.Modules._ast;
using IronPython.Runtime.Operations;
using static IronPython.Modules.PythonCsvModule;

namespace FormProject
{
    public class DBFunctions
    {
        DBConnection connectorToDb;
        DataTable table;
        MySqlDataAdapter cursor;
        const string path = "logsFile.txt";
        LogsFileFuntcions writer;
        ResourceManager rm;

        public DBFunctions()
        {
            connectorToDb = new DBConnection();
            cursor = new MySqlDataAdapter();
            writer = new LogsFileFuntcions(path);
            rm = new ResourceManager("FormProject.Properties.Resources",
                typeof(DBFunctions).Assembly); 
        }

        string GetMysqlException(MySqlException ex)
        {
            string exception;
            switch (ex.Number)
            {
                case 0:
                    exception = rm.GetString("cantConnectException");
                    break;
                case 1045:
                    exception = rm.GetString("accessException");
                    break;
                case 1050:
                    exception = rm.GetString("dbDoesntExistException");
                    break;
                case 1051:
                    exception = rm.GetString("tableDoesntExistException");
                    break;
                case 1055:
                    exception = rm.GetString("columnDoesntExistException");
                    break;
                default:
                    exception = rm.GetString("dbException");
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
                writer.WriteToLogsFile(string.Format(exception, "IsPassCorrect", ex.Number), login);
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsPassCorrect");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "IsPassCorrect"), login);
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
                Console.WriteLine(rm.GetString("dbException"), "IsRegistered", ex.Number);
                writer.WriteToLogsFile(string.Format(rm.GetString("dbException"), "IsRegistered", ex.Number), login);
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsRegistered");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "IsRegistered"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "Regiser", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "Regiser");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "Register"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "DeleteStats", ex.Number), login);
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
                writer.WriteToLogsFile(string.Format(exception, "DeleteProfile", ex.Number), login);
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
                string insertLogPass = "insert into users (login, pass, idStats, idProfile, isAdmin) " +
                    $"values (@uLogin, @uPass, @uStatsId, @uProfileId, {1});";
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
                writer.WriteToLogsFile(string.Format(exception, "AddToUsersTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "AddToUsersTable");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "AddToUsersTable"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "AddToProfilesTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "AddToProfilesTable");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "AddToProfilesTable"), login);
            }
            return 0;
        }

        private int AddToStatsTable(string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand idProfileInsert = new MySqlCommand("insert into stats (solved, unsolved, сoveredTopics, favourite) values ('', '', '', '');", connectorToDb.GetConnection());
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
                writer.WriteToLogsFile(string.Format(exception, "AddToStatsTable", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "AddToStatsTable");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "AddToStatsTable"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "ProfileIDByLog", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "ProfileIDByLog");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "ProfileIDByLog"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "StatsIDByLog", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "StatsIDByLog");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "StatsIDByLog"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "GetStatsField", ex.Number), login);
                return "программная ошибка";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetStatsField");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetStatsField"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "GetStatsField", ex.Number), login);
                return "программная ошибка";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetStatsField");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetStatsField"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "ChangeField", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "ChangeField");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "ChangeField"), login);
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
                writer.WriteToLogsFile(string.Format(exception, "IsAdmin", ex.Number), login, "error");
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsAdmin");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "IsAdmin"), login, "error");
                problem = "программная ошибка";
            }
            return false;
        }

        public List<Dictionary<string, string>> GetExercises(out string problem, string login)
        {
            List<Dictionary<string, string>> exerciseData = new List<Dictionary<string, string>>();
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select idExercise, theme, complexity, description, exp, answer, additionalContent from exercises;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();
                Dictionary<string, string> newDict;
                while (reader.Read())
                {
                    newDict = new Dictionary<string, string>();
                    newDict["id"] = reader.GetValue(0).ToString();
                    newDict["theme"] = reader.GetValue(1).ToString();
                    newDict["complexity"] = reader.GetValue(2).ToString();
                    newDict["description"] = reader.GetValue(3).ToString();
                    newDict["exp"] = reader.GetValue(4).ToString();
                    newDict["answer"] = reader.GetValue(5).ToString();
                    newDict["additionalContent"] = reader.GetValue(6).ToString();
                    exerciseData.Add(newDict);
                }
                reader.Close();
                problem = "";
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetExercises", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetExercises", ex.Number), login, "error");
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsAdmin");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetExercises"), login, "error");
                problem = "программная ошибка";
            }
            return exerciseData;
        }

        public void IncreaseEXP(string login, int exp)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"update stats set exp = exp + {exp} where idStats = {idStats};" +
                    $"update stats set solvedAmount = solvedAmount + 1 where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "IncreaseEXP", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "IncreaseEXP", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IncreaseEXP");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "IncreaseEXP"), login, "error");
            }
        }

        public bool IsSolved(string login, int id)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select solved from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string[] solved = command.ExecuteScalar().ToString().Split();
                connectorToDb.CloseConnection();
                return solved.Contains(id.ToString());
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "IsSolved", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "IsSolved", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsSolved");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "IsSolved"), login, "error");
            }
            return false;
        }

        public void AddToSolved(string login, int id)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select solved from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string solved = command.ExecuteScalar().ToString();
                solved += $" {id}";
                connectorToDb.OpenConnection();
                commandText = $"update stats set solved = '{solved}' where idStats = {idStats};";
                command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "AddToSolved", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "AddToSolved", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "AddToSolved");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "AddToSolved"), login, "error");
            }
        }

        public void CreateExercise(string login, Dictionary<string, object> exerciseData, ref bool problem)
        {
            try
            {
                int userStatsId = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = "insert into exercises (theme, complexity, description, exp, answer, additionalContent, creatorStatsId)" +
                    " values (@Theme, @Complexity, @Description, @Exp, @Answer, @AdditionalContent, @CreatorStatsId);";
                MySqlCommand exerciseInsertCommand = new MySqlCommand(commandText, connectorToDb.GetConnection());
                exerciseInsertCommand.Parameters.AddWithValue("@Theme", exerciseData["theme"]);
                exerciseInsertCommand.Parameters.AddWithValue("@Complexity", exerciseData["complexity"]);
                exerciseInsertCommand.Parameters.AddWithValue("@Description", exerciseData["description"]);
                exerciseInsertCommand.Parameters.AddWithValue("@Exp", exerciseData["exp"]);
                exerciseInsertCommand.Parameters.AddWithValue("Answer", exerciseData["answer"]);
                exerciseInsertCommand.Parameters.AddWithValue("@AdditionalContent", exerciseData["additionalContent"]);
                exerciseInsertCommand.Parameters.AddWithValue("@CreatorStatsId", userStatsId);
                exerciseInsertCommand.ExecuteNonQuery();
                connectorToDb.CloseConnection();
                problem = false;
                return;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "CreateExercise", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "CreateExercise", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "CreateExercise");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "CreateExercise"), login);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(rm.GetString("keyNotFoundException"), "CreateExercise");
                writer.WriteToLogsFile(string.Format(rm.GetString("keyNotFoundException"), "CreateExercise"), login);
            }
            catch (Exception ex)
            {
                Console.WriteLine("unknownException", "CreateExercise", ex.GetType().Name);
                writer.WriteToLogsFile(string.Format("unknownException", "CreateExercise", ex.GetType().Name), login);
            }
            problem = true;
        }

        public int GetLastExerciseId(string login)
        {
            try
            {
                int userStatsId = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select idExercise from exercises where creatorStatsId={userStatsId}" +
                    $" and theme=\"Написать код\" order by idExercise desc limit 1;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                int exerciseId = Convert.ToInt32(command.ExecuteScalar());
                connectorToDb.CloseConnection();
                return exerciseId;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetExerciseId", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetExerciseId", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetLastExerciseId");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetLastExerciseId"), login);
            }
            catch (Exception ex)
            {
                Console.WriteLine("unknownException", "GetLastExerciseId", ex.GetType().Name);
                writer.WriteToLogsFile(string.Format("unknownException", "GetLastExerciseId", ex.GetType().Name), login);
            }
            return 0;
        }

        public void DeleteExercise(int exerciseId, string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                MySqlCommand delete = new MySqlCommand($"delete from exercises where idExercise" +
                    $" = {exerciseId}", connectorToDb.GetConnection());
                delete.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "DeleteProfile", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "DeleteProfile", ex.Number), login);
            }
        }

        public string GetCreatorLoginByIdExercise(int idExercise, string login)
        {
            try
            {
                connectorToDb.OpenConnection();
                string getId = $"select creatorStatsId from exercises where idExercise={idExercise} limit 1";
                MySqlCommand command = new MySqlCommand(getId, connectorToDb.GetConnection());
                int statsId = Convert.ToInt32(command.ExecuteScalar());

                string getLogin = $"select login from users where idStats={statsId} limit 1";
                command = new MySqlCommand(getLogin, connectorToDb.GetConnection());

                string creatorLogin = command.ExecuteScalar().ToString();
                connectorToDb.CloseConnection();
                return creatorLogin;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetStatsByExercise", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetStatsByExercise", ex.Number), login);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetStatsByExercise");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetStatsByExercise"), login);
            }
            catch (Exception ex)
            {
                Console.WriteLine("unknownException", "GetStatsByExercise", ex.GetType().Name);
                writer.WriteToLogsFile(string.Format("unknownException", "GetStatsByExercise", ex.GetType().Name), login);
            }
            return "";
        }

        public string GetFavourite(string login)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select favourite from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string favourite = command.ExecuteScalar().ToString();
                connectorToDb.CloseConnection();
                return favourite;
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetFavourite", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetFavourite", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetFavourite");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetFavourite"), login, "error");
            }
            return "";
        }

        public void AddToFavourite(string login, int id)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select favourite from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string favourite = command.ExecuteScalar().ToString();
                favourite += $"#{id}#";
                connectorToDb.OpenConnection();
                commandText = $"update stats set favourite = '{favourite}' where idStats = {idStats};";
                command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "AddToFavourite", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "AddToFavourite", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "AddToFavourite");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "AddToFavourite"), login, "error");
            }
        }

        public void DeleteFromFavourite(string login, int id)
        {
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select favourite from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string favourite = command.ExecuteScalar().ToString();
                favourite = favourite.Replace($"#{id}#", "");
                connectorToDb.OpenConnection();
                commandText = $"update stats set favourite = '{favourite}' where idStats = {idStats};";
                command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                command.ExecuteNonQuery();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "DeleteFromFavourite", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "DeleteFromFavourite", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "DeleteFromFavourite");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "DeleteFromFavourite"), login, "error");
            }
        }

        public Dictionary<string, string>
            GetFavouriteDict(out string problem, string login, int id)
        {
            Dictionary<string, string> exerciseData = new Dictionary<string, string>();
            try
            {
                connectorToDb.OpenConnection();
                string commandText = $"select idExercise, theme, complexity, description," +
                    $" exp, answer, additionalContent from exercises where idExercise = {id} limit 1;";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    exerciseData["id"] = reader.GetValue(0).ToString();
                    exerciseData["theme"] = reader.GetValue(1).ToString();
                    exerciseData["complexity"] = reader.GetValue(2).ToString();
                    exerciseData["description"] = reader.GetValue(3).ToString();
                    exerciseData["exp"] = reader.GetValue(4).ToString();
                    exerciseData["answer"] = reader.GetValue(5).ToString();
                    exerciseData["additionalContent"] = reader.GetValue(6).ToString();
                }
                reader.Close();
                problem = "";
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetFavouriteDict", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetFavouriteDict", ex.Number), login, "error");
                problem = "ошибка авторизации";
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "IsAdmin");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetFavouriteDict"), login, "error");
                problem = "программная ошибка";
            }
            return exerciseData;
        }

        public List<Dictionary<string, string>> GetSolved(string login)
        {
            List<Dictionary<string, string>> exerciseData = new List<Dictionary<string, string>>();
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select solved from stats where idStats = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                string solved = command.ExecuteScalar().ToString();
                Dictionary<string, string> newDict;
                foreach (string ex in solved.Split())
                {
                    if (!int.TryParse(ex.Trim(), out int exId)) continue;
                    commandText = $"select idExercise, theme, complexity" +
                        $" from exercises where IdExercise={exId};";
                    command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                    MySqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    newDict = new Dictionary<string, string>();
                    newDict["Id"] = reader.GetValue(0).ToString();
                    newDict["Type"] = reader.GetValue(1).ToString();
                    newDict["Complexity"] = reader.GetValue(2).ToString();
                    exerciseData.Add(newDict);
                    reader.Close();
                }
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetSolved", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetSolved", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetSolved");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"), "GetSolved"), login, "error");
            }
            return exerciseData;
        }

        public List<Dictionary<string, string>> GetCreated(string login)
        {
            List<Dictionary<string, string>> exerciseData = new List<Dictionary<string, string>>();
            try
            {
                int idStats = StatsIDByLog(login);
                connectorToDb.OpenConnection();
                string commandText = $"select idExercise, theme, complexity" +
                    $" from exercises where creatorStatsId = {idStats};";
                MySqlCommand command = new MySqlCommand(commandText, connectorToDb.GetConnection());
                MySqlDataReader reader = command.ExecuteReader();
                Dictionary<string, string> newDict;
                while (reader.Read())
                {
                    newDict = new Dictionary<string, string>();
                    newDict["Id"] = reader.GetValue(0).ToString();
                    newDict["Type"] = reader.GetValue(1).ToString();
                    newDict["Complexity"] = reader.GetValue(2).ToString();
                    exerciseData.Add(newDict);
                }
                reader.Close();
                connectorToDb.CloseConnection();
            }
            catch (MySqlException ex)
            {
                string exception = GetMysqlException(ex);
                Console.WriteLine(exception, "GetCreated", ex.Number);
                writer.WriteToLogsFile(string.Format(exception, "GetCreated", ex.Number), login, "error");
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(rm.GetString("nullException"), "GetCreated");
                writer.WriteToLogsFile(string.Format(rm.GetString("nullException"),
                    "GetCreated"), login, "error");
            }
            return exerciseData;
        }
    }
}

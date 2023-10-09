using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject
{
    internal class DBFunctions
    {
        DBConnection connection = new DBConnection();
        DataTable table = new DataTable();
        MySqlDataAdapter cursor = new MySqlDataAdapter();

        public bool IsRegistered(string login)
        {
            MySqlCommand registered = new MySqlCommand("SELECT * FROM `log_and_pass`" +
                "where `login` = @uLogin", connection.GetConnection());
            registered.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            connection.OpenConnection();
            cursor.SelectCommand = registered;
            cursor.Fill(table);
            connection.CloseConnection();
            return table.Rows.Count > 0; 
        }

        public bool Register(string login, string password) 
        {
            Console.WriteLine(login);
            MySqlCommand register = new MySqlCommand("INSERT INTO `log_and_pass` (login, password) " +
                "values (@uLogin, @uPassword)", connection.GetConnection());
            register.Parameters.Add("@uLogin", MySqlDbType.VarChar).Value = login;
            register.Parameters.Add("@uPassword", MySqlDbType.VarChar).Value = password;
            try
            {
                connection.OpenConnection();
                register.ExecuteNonQuery();
                connection.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}

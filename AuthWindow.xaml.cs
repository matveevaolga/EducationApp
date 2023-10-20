using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FormProject
{
    /// <summary>
    /// Логика взаимодействия для AuthorizatoinWindow.xaml
    /// </summary>
    public partial class AuthorizatoinWindow : Window
    {
        public AuthorizatoinWindow()
        {
            InitializeComponent();
        }

        public AuthorizatoinWindow(string message)
        {
            InitializeComponent();
            success.Content = message;

        }

        private bool LengthCheck()
        {
            if (login.Text.Length == 0)
            {
                message.Text = "Вы не ввели логин.";
                return false;
            }
            if (password.Password.Length == 0)
            {
                message.Text = "Вы не ввели пароль.";
                return false;
            }
            return true;
        }
        private bool CheckLog()
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (!dBFunctions.IsRegistered(login.Text))
            {
                message.Text = "Под таким логином нет пользователя.";
                return false;
            }
            return true;
        }

        private bool CheckPass()
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (!dBFunctions.IsPassCorrect(login.Text, password.Password))
            {
                message.Text = "Вы ввели не верный пароль.";
                return false;
            }
            return true;
        }

        private void SignUp(object sender, EventArgs e)
        {
            RegisterWindow regWindow = new RegisterWindow();
            regWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            regWindow.Show();
            this.Close();
        }

        private void LogIn(object sender, EventArgs e)
        {
            login.Text = login.Text.Trim();
            password.Password = password.Password.Trim();
            if (LengthCheck() && CheckLog() && CheckPass() )
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mainWindow.Show();
                this.Close();
            }
        }
    }
}

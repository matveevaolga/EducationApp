using FormProject.View;
using MySql.Data.MySqlClient;
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
using FormProject.Controller;

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
                message.Content = "Вы не ввели логин.";
                return false;
            }
            if (password.Password.Length == 0)
            {
                message.Content = "Вы не ввели пароль.";
                return false;
            }
            return true;
        }

        private void EndAuth(string problem)
        {
            if (problem != "") message.Content = problem;
            login.Text = "";
            password.Password = "";
        }

        private bool CheckLog()
        {
            bool isReg = DBHelpFunctional.HelpIsRegistered(login.Text, out string problem);
            if (problem != "")
            {
                message.Content = problem;
                return false;
            }
            else if (!isReg)
            {
                message.Content = "Под таким логином нет пользователя.";
                return false;
            }
            return true;
        }

        private bool CheckPass()
        {
            bool isPassOk = DBHelpFunctional.HelpIsPassCorrect(login.Text, password.Password, out string problem);
            if (problem != "")
            {
                EndAuth(problem);
                return false;
            }
            else if (!isPassOk)
            {
                EndAuth("Вы ввели неверный пароль");
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
                MainWindow mainWindow = new MainWindow(login.Text);
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mainWindow.Show();
                this.Close();
            }
        }
    }
}

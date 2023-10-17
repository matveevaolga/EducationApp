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

        private bool CheckLog()
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (!dBFunctions.IsRegistered(login.Text))
            {
                login.Text = "Вы еще не зарегистрировались. Заполните форму для входа.";
                RegisterWindow regWindow = new RegisterWindow();
                regWindow.Show();
                this.Close();
                return false;
            }
            return true;
        }

        private bool CheckPass()
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (!dBFunctions.IsPassCorrect(login.Text, password.Password))
            {
                passwordMessage.Text = "Вы ввели не верный пароль.";
                return false;
            }
            return true;
        }

        private void LogIn(object sender, EventArgs e)
        {
            login.Text = login.Text.Trim();
            password.Password = password.Password.Trim();
            if ( CheckLog() & CheckPass())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                message.Text = "Повторите вход.";
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void LogAndPassMessages(bool isRegistered)
        {
            if (isRegistered) { loginMessage.Text = "Пользователь с таким логином уже зарегестрирован"; }
            if (password.Password.Length < 5) { passwordMessage.Text = "Введенный пароль слишком ненадежен"; }
        }
        private void SendForm(object sender, EventArgs e)
        {
            login.Text = login.Text.Trim();
            password.Password = password.Password.Trim();
            DBFunctions dBFunctions = new DBFunctions();
            bool isRegistered = dBFunctions.IsRegistered(login.Text);
            LogAndPassMessages(isRegistered);
            if (!(isRegistered | password.Password.Length < 5) & 
                dBFunctions.Register(login.Text, password.Password)) 
            {
                message.Text = "Вы успешно зарегестрировались";
                AuthorizatoinWindow authWindow = new AuthorizatoinWindow();
                authWindow.Show();
                this.Close();
            }
            message.Text = "Произошла ошибка во время регистрации";
            login.Text = "";
            password.Password = "";
        }

    }
}

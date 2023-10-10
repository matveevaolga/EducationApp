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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private bool IsFormValid(string userLogin, string userPassword)
        {
            DBFunctions dBFunctions = new DBFunctions();
            bool isRegistered = dBFunctions.IsRegistered(userLogin);
            string loginmessage = "Введите логин";
            string passwordmessage = "Введите пароль";
            if (isRegistered) { loginmessage = "Пользователь с таким логином уже зарегестрирован"; }
            if (userPassword.Length < 5) { passwordmessage = "Введенный пароль слишком ненадежен"; }
            loginError.Text = loginmessage, passwordError.Text = passwordmessage;
            if (isRegistered | userPassword.Length < 5) { return false; }
            if (dBFunctions.Register(userLogin, userPassword)) { return true; }
            else { return false; }
        }

        private void SendForm(object sender, EventArgs e)
        {
            message.Text = "Заполните форму для регистрации";
            string userLogin = login.Text.Trim();
            string userPassword = password.Password.Trim();
            if (IsFormValid(userLogin, userPassword)) { message.Text = "Вы успешно зарегестрировались"; }
            else { message.Text = "Произошла ошибка, повторите регистрацию"; }
            login.Text = "";
            password.Password = "";
        }
    }
}

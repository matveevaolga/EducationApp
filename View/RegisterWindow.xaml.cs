using System;
using System.Windows;
using FormProject.Controller;

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

        private bool LogAndPassMessages(bool isRegistered)
        {
            if (isRegistered) { 
                loginMessage.Content = "Пользователь с таким логином уже зарегестрирован";
                return false;
            }
            if (password.Password.Length < 5) { 
                passwordMessage.Content = "Введенный пароль слишком ненадежен";
                return false;
            }
            return true;
        }

        private void EndRegistration(string problem)
        {
            if (problem != "") message.Content = problem;  
            login.Text = "";
            password.Password = "";
        }

        private void SendForm(object sender, EventArgs e)
        {
            login.Text = login.Text.Trim();
            password.Password = password.Password.Trim();
            string problem;
            bool isRegistered = DBHelpFunctional.HelpIsRegistered(login.Text, out problem);
            if (problem == "" && LogAndPassMessages(isRegistered))
            {
                if (DBHelpFunctional.HelpRegister(login.Text, password.Password, out problem))
                {
                    AuthorizatoinWindow authWindow = new AuthorizatoinWindow("Вы успешно зарегестрировались");
                    authWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    authWindow.Show();
                    this.Close();
                }
            }
            EndRegistration(problem);
        }

        private void GoBack(object sender, EventArgs e)
        {
            AuthorizatoinWindow authWindow = new AuthorizatoinWindow();
            authWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            authWindow.Show();
            this.Close();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using FormProject.Controller;
using FormProject.View;

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
            if (password.Text.Length < 5) { 
                passwordMessage.Content = "Введенный пароль слишком ненадежен";
                return false;
            }
            return true;
        }

        private void EndRegistration(string problem)
        {
            if (problem != "") message.Content = problem;  
            login.Text = "";
            password.Text = "";
        }
        string currentButton;
        public void ChangeWindow(object sender, EventArgs e)
        {
            if (currentButton == "success")
            {
                AuthorizatoinWindow authWindow = 
                    new AuthorizatoinWindow("Вы успешно зарегистрировались");
                authWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                authWindow.Show();
                this.Close();
            }
            else if (currentButton == "backToAuth")
            {
                AuthorizatoinWindow authWindow = new AuthorizatoinWindow();
                authWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                authWindow.Show();
                this.Close();
            }
        }
        private void SendForm(object sender, EventArgs e)
        {
            currentButton = "pupu...";
            VisualStateManager.GoToState((Button)sender, "NormalState", true);
            VisualStateManager.GoToState((Button)sender, "ClickedState", true);
            login.Text = login.Text.Trim();
            password.Text = password.Text.Trim();
            string problem;
            bool isRegistered = DBHelpFunctional.HelpIsRegistered(login.Text, out problem);
            if (problem == "" && LogAndPassMessages(isRegistered))
            {
                if (DBHelpFunctional.HelpRegister(login.Text, password.Text, out problem))
                {
                    currentButton = "success";
                    VisualStateManager.GoToState((Button)sender, "NormalState", true);
                    VisualStateManager.GoToState((Button)sender, "ClickedState", true);
                }
            }
            EndRegistration(problem);
        }

        private void ToAuthorization(object sender, EventArgs e)
        {
            currentButton = "backToAuth";
            VisualStateManager.GoToState((Button)sender, "NormalState", true);
            VisualStateManager.GoToState((Button)sender, "ClickedState", true);
        }
    }
}

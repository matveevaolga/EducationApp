using FormProject.View;
using System;
using System.Windows;
using FormProject.Controller;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Media.Animation;

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
            this.Topmost = true;
        }

        private bool LengthCheck()
        {
            if (login.Text.Length == 0)
            {
                message.Content = "Вы не ввели логин.";
                return false;
            }
            if (password.Text.Length == 0)
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
            password.Text = "";
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
            bool isPassOk = DBHelpFunctional.HelpIsPassCorrect(login.Text,
                password.Text, out string problem);
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
        string currentButton;
        private void SignUp(object sender, EventArgs e)
        {
            currentButton = "signUpButton";
            VisualStateManager.GoToState(signUpButton, "NormalState", true);
            VisualStateManager.GoToState(signUpButton, "ClickedState", true);
        }
        public void ChangeWindow(object sender, EventArgs e)
        {
            if (currentButton == "signUpButton")
            {
                RegisterWindow regWindow = new RegisterWindow();
                regWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                regWindow.Show();
                regWindow.Topmost = false;
                this.Close();
            }
            else if (currentButton == "logInButton")
            {
                MainWindow mainWindow = new MainWindow(login.Text);
                mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                mainWindow.Topmost = false;
                mainWindow.Show();
                this.Close();
            }
        }
        private void LogIn(object sender, EventArgs e)
        {
            currentButton = "pupu...";
            VisualStateManager.GoToState((Button)sender, "NormalState", true);
            VisualStateManager.GoToState((Button)sender, "ClickedState", true);
            login.Text = login.Text.Trim();
            password.Text = password.Text.Trim();
            if (LengthCheck() && CheckLog() && CheckPass())
            {
                currentButton = "logInButton";
                VisualStateManager.GoToState((Button)sender, "NormalState", true);
                VisualStateManager.GoToState((Button)sender, "ClickedState", true);
            }
        }
    }
}

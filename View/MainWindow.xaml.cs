using FormProject.View.UserControls;
using System;
using System.Windows;

namespace FormProject.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string login;

        public MainWindow(string login)
        {
            InitializeComponent();
            MainWindow.login = login;
            currentContent.Content = new UserProfile(login);
        }

        private void ToExercises(object sender, EventArgs e)
        {
            currentContent.Content = new Exercises(login);
        }

        private void ToFavourite(object sender, EventArgs e)
        {
            currentContent.Content = new Favourite(login);
        }

        private void ToUserProfile(object sender, EventArgs e)
        {
            currentContent.Content = new UserProfile(login);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < this.MinWidth) { this.Width = this.MinWidth; }
            if (e.NewSize.Height < this.MinHeight) { this.Height = this.MinHeight; }
        }
    }
}

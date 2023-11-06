using FormProject.View.UserControls;
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
    }
}

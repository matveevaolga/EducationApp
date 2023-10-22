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
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Window
    {   

        public UserProfile()
        {
            InitializeComponent();
            statsBorder.Visibility = Visibility.Visible;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowStats(object sender, EventArgs e)
        {
            statsBorder.Visibility = Visibility.Visible;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowEdit(object sender, EventArgs e)
        {
            statsBorder.Visibility = Visibility.Collapsed;
            editBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            statsBorder.Visibility = Visibility.Collapsed;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Visible;
        }
    }
}

using FormProject.Controller;
using System;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    
    class GeneralInfo
    {
        public string login { get; set; }
        public string uName { get; set; }
        public string uLevel { get; set; }
        public string uExp { get; set; }
        public string uInfo { get; set; }
        public ImageBrush uImage { get; set; }
        public GeneralInfo(string uLogin)
        {
            login = uLogin;
            uName = DBHelpFunctional.HelpGetProfileField(login, "name");
            uLevel = DBHelpFunctional.HelpGetStatsField(login, "level");
            uExp = DBHelpFunctional.HelpGetStatsField(login, "exp");
            uInfo = DBHelpFunctional.HelpGetProfileField(login, "about");
            uImage = new ImageBrush();
            uImage.ImageSource = new BitmapImage(new Uri("D:\\C#projects\\WpfProjects\\FormProject\\Datas\\Images\\default.png",
                UriKind.Relative));
        }
    }

    public partial class UserProfile : UserControl
    {
        public string login;

        public UserProfile(string login)
        {
            this.login = login;
            InitializeComponent();
            switchUC.Content = new StatsUC(login);
            DataContext = new GeneralInfo(login);
        }

        private void ShowStats(object sender, EventArgs e)
        {
            switchUC.Content = new StatsUC(login);
            DataContext = new GeneralInfo(login);
        }

        private void ShowEdit(object sender, EventArgs e)
        {
            switchUC.Content = new EditUC(login);
            DataContext = new GeneralInfo(login);
        }
    }
}

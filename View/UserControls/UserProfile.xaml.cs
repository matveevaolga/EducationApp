using FormProject.Controller;
using System;
using System.Windows.Controls;

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
        public GeneralInfo(string uLogin)
        {
            login = uLogin;
            uName = "Имя:\n" + DBHelpFunctional.HelpGetProfileField(login, "name");
            uLevel = "Уровень:\n" + DBHelpFunctional.HelpGetStatsField(login, "level");
            uExp = "Exp:\n" + DBHelpFunctional.HelpGetStatsField(login, "exp");
            uInfo = "О себе:\n" + DBHelpFunctional.HelpGetProfileField(login, "about");
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

        private void ShowSettings(object sender, EventArgs e)
        {
            switchUC.Content = new SettingsUC(login);
            DataContext = new GeneralInfo(login);
        }
    }
}

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
    /// 

    class GeneralInfo
    {
        public string login {  get; set; }
        public string uName { get; set; }
        public string uLevel { get; set; }
        public string uExp { get; set; }
        public string uInfo {  get; set; }
        public GeneralInfo(string login)
        {
            this.login = login;
            DBFunctions dBFunctions = new DBFunctions();
            uName = "Имя:\n" + dBFunctions.GetProfileField(login, "name");
            uLevel = "Уровень:\n" + dBFunctions.GetStatsField(login, "level");
            uExp = "Exp:\n" + dBFunctions.GetStatsField(login, "exp");
            uInfo = "О себе:\n" + dBFunctions.GetProfileField(login, "about");
        }
    }

    class StatsInfo:GeneralInfo
    {
        public string uActive { get; set; }
        public string uMaxSession {  get; set; }
        public string uSolvedAmount { get; set; }
        public string uTopicsAmount {  get; set; }
        public StatsInfo(string login):base(login)
        {
            DBFunctions dBFunctions=new DBFunctions();
            uActive = dBFunctions.GetStatsField(login, "active");
            uMaxSession = dBFunctions.GetStatsField(login, "maxSession");
            uSolvedAmount = dBFunctions.GetStatsField(login, "solvedAmount");
            uTopicsAmount = dBFunctions.GetStatsField(login, "coveredTopicsAmount");
        }
    }

    class EditInfo:GeneralInfo
    {

        public EditInfo(string login):base(login)
        {

        }
    }

    class SettingsInfo:GeneralInfo
    {
        public SettingsInfo(string login):base(login)
        {

        }
    }

    public partial class UserProfile : Window
    {   
        string login;

        public UserProfile(string login)
        {
            this.login = login;
            InitializeComponent();
            this.DataContext = new StatsInfo(login);
            statsBorder.Visibility = Visibility.Visible;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowStats(object sender, EventArgs e)
        {
            this.DataContext = new StatsInfo(login);
            statsBorder.Visibility = Visibility.Visible;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowEdit(object sender, EventArgs e)
        {
            this.DataContext = new EditInfo(login);
            statsBorder.Visibility = Visibility.Collapsed;
            editBorder.Visibility = Visibility.Visible;
            settingsBorder.Visibility = Visibility.Collapsed;
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            this.DataContext = new SettingsInfo(login);
            statsBorder.Visibility = Visibility.Collapsed;
            editBorder.Visibility = Visibility.Collapsed;
            settingsBorder.Visibility = Visibility.Visible;
        }

        private void EditProfile(object sender, EventArgs e)
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (nameChange.Text != "") 
            { 
                bool result = dBFunctions.ChangeField(login, "profiles", "name", nameChange.Text);
                nameChange.Text = result == true ? "" : "ошибка, не удалось изменить поле";
            }
            if (infoChange.Text != "") 
            { 
                bool result = dBFunctions.ChangeField(login, "profiles", "about", infoChange.Text);
                infoChange.Text = result == true ? "" : "ошибка, не удалось изменить поле";
            }
        }
    }
}

﻿using System;
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
            DBFunctions dBFunctions = new DBFunctions();
            uName = "Имя:\n" + dBFunctions.GetProfileField(login, "name");
            uLevel = "Уровень:\n" + dBFunctions.GetStatsField(login, "level");
            uExp = "Exp:\n" + dBFunctions.GetStatsField(login, "exp");
            uInfo = "О себе:\n" + dBFunctions.GetProfileField(login, "about");
        }
    }

    public partial class UserProfile : UserControl
    {
        public static string login;

        public UserProfile(string login)
        {
            UserProfile.login = login;
            InitializeComponent();
            switchUC.Content = new StatsUC();
            DataContext = new GeneralInfo(login);
        }

        private void ShowStats(object sender, EventArgs e)
        {
            switchUC.Content = new StatsUC();
            DataContext = new GeneralInfo(login);
        }

        private void ShowEdit(object sender, EventArgs e)
        {
            switchUC.Content = new EditUC();
            DataContext = new GeneralInfo(login);
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            switchUC.Content = new SettingsUC();
            DataContext = new GeneralInfo(login);
        }
    }
}
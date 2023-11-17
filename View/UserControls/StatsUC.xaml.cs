using FormProject.Controller;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StatsUC.xaml
    /// </summary>
    public partial class StatsUC : UserControl
    {
        public StatsUC(string login)
        {
            InitializeComponent();
            DataContext = new StatsInfo(login);
        }
    }

    class StatsInfo 
    {   
        public string uActive { get; set; }
        public string uMaxSession { get; set; }
        public string uSolvedAmount { get; set; }
        public string uTopicsAmount { get; set; }
        public StatsInfo(string login)
        {
            uActive = DBHelpFunctional.HelpGetStatsField(login, "active");
            uMaxSession = DBHelpFunctional.HelpGetStatsField(login, "maxSession");
            uSolvedAmount = DBHelpFunctional.HelpGetStatsField(login, "solvedAmount");
            uTopicsAmount = DBHelpFunctional.HelpGetStatsField(login, "coveredTopicsAmount");
        }
    }
}

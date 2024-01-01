using FormProject.Controller;
using System.Windows.Controls;

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

using FormProject.Controller;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для StatsUC.xaml
    /// </summary>
    public partial class StatsUC : UserControl
    {
        public class ExerciseDescription
        {
            public int Id { get; set; }
            public string Type { get; set; }
            public int Exp { get; set; }
            public string Date { get; set; }
            public ExerciseDescription(int id, string type, int exp, string date)
            {
                Id = id;
                Type = type;
                Exp = exp;
                Date = date;
            }
        }
        public string uActive { get; set; }
        public string uMaxSession { get; set; }
        public string uSolvedAmount { get; set; }
        public string uTopicsAmount { get; set; }
        List<ExerciseDescription> solvedExercises;
        public List<ExerciseDescription> SolvedExercises
        {
            get { return solvedExercises; }
            set { solvedExercises = value; }
        }
        List<ExerciseDescription> createdExercises;
        public List<ExerciseDescription> CreatedExercises
        {
            get { return createdExercises; }
            set { createdExercises = value; }
        }
        public StatsUC(string login)
        {
            InitializeComponent();
            uActive = DBHelpFunctional.HelpGetStatsField(login, "active");
            uMaxSession = DBHelpFunctional.HelpGetStatsField(login, "maxSession");
            uSolvedAmount = DBHelpFunctional.HelpGetStatsField(login, "solvedAmount");
            uTopicsAmount = DBHelpFunctional.HelpGetStatsField(login, "coveredTopicsAmount");
            //FillSolved();
            //FillCreated();
            DataContext = this;
        }
    }
}

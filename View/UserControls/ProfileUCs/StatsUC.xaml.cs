using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
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
            public string Id { get; set; }
            public string Type { get; set; }
            public string Complexity { get; set; }
            public ExerciseDescription(string id, string type, string complexity)
            {
                Id = id;
                Type = type;
                Complexity = complexity;
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
        string login;
        public StatsUC(string login)
        {
            InitializeComponent();
            this.login = login;
            uActive = DBHelpFunctional.HelpGetStatsField(login, "active");
            uMaxSession = DBHelpFunctional.HelpGetStatsField(login, "maxSession");
            uSolvedAmount = DBHelpFunctional.HelpGetStatsField(login, "solvedAmount");
            uTopicsAmount = DBHelpFunctional.HelpGetStatsField(login, "coveredTopicsAmount");
            solvedExercises = new List<ExerciseDescription>();
            createdExercises = new List<ExerciseDescription>();
            FillSolved();
            FillCreated();
            DataContext = this;
        }
        void FillSolved()
        {
            List<Dictionary<string,string>> solved =
                DBHelpFunctional.HelpGetSolved(login);
            ExerciseDescription newExercise;
            foreach (Dictionary<string,string> exData in solved)
            {
                newExercise = new ExerciseDescription(exData["Id"],
                    exData["Type"], exData["Complexity"]);
                SolvedExercises.Add(newExercise);
            }
        }
        void FillCreated()
        {
            List<Dictionary<string, string>> created =
                DBHelpFunctional.HelpGetCreated(login);
            ExerciseDescription newExercise;
            foreach (Dictionary<string, string> exData in created)
            {
                newExercise = new ExerciseDescription(exData["Id"],
                    exData["Type"], exData["Complexity"]);
                CreatedExercises.Add(newExercise);
            }
        }
    }
}

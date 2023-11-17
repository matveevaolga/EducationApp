using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using FormProject.View.UserControls.ExercisesUCs;
using FormProject.Controller;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Exercises.xaml
    /// </summary>
    public partial class Exercises : UserControl
    {
        public DBFunctions dBFunctions;

        public Exercises(string login)
        {
            InitializeComponent();
            if (DBHelpFunctional.HelpIsAdmin(login, out string problem)) { createExerciseButton.Visibility = Visibility.Visible; }
            exercisesFunctional.Content = new ShowExercisesUC(problem);
        }

        private void switchExercisesFunctional(object sender, EventArgs e)
        {
            CreateExerciseUC createExerciseUC = new CreateExerciseUC();
            ShowExercisesUC showExercisesUC = new ShowExercisesUC();
            if (exercisesFunctional.Content.GetType() != createExerciseUC.GetType()) 
            { exercisesFunctional.Content = createExerciseUC; }
            else { exercisesFunctional.Content = showExercisesUC; }
        }
    }
}

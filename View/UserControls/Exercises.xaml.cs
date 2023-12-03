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
        string login;

        public Exercises(string login)
        {
            InitializeComponent();
            this.login = login;
            if (DBHelpFunctional.HelpIsAdmin(login, out string problem)) 
            { createExerciseButton.Visibility = Visibility.Visible; }
            exercisesFunctional.Content = new ShowExercisesUC(problem);
        }

        private void showExercises(object sender, EventArgs e)
        {
            ShowExercisesUC showExercisesUC = new ShowExercisesUC(login);
            exercisesFunctional.Content = showExercisesUC;
        }

        private void createExercise(object sender, EventArgs e)
        {
            CreateExerciseUC createExerciseUC = new CreateExerciseUC();
            exercisesFunctional.Content = createExerciseUC;
        }
    }
}

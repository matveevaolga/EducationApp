using System;
using System.Windows;
using System.Windows.Controls;
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
            exercisesFunctional.Content = new ShowExercisesUC(login);
        }

        private void showExercises(object sender, EventArgs e)
        {
            ShowExercisesUC showExercisesUC = new ShowExercisesUC(login);
            exercisesFunctional.Content = showExercisesUC;
        }

        private void createExercise(object sender, EventArgs e)
        {
            CreateExerciseUC createExerciseUC = new CreateExerciseUC(login);
            exercisesFunctional.Content = createExerciseUC;
        }
    }
}

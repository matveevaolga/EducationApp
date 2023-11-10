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
using FormProject.View.UserControls.ExercisesUCs;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Exercises.xaml
    /// </summary>
    public partial class Exercises : UserControl
    {
        public Exercises(string login)
        {
            InitializeComponent();
        }

        private void WriteCodeChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseUC.Content = new WriteCode();
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseUC.Content = new YourAnswer();
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseUC.Content = new ChooseCorrect();
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseUC.Content = new InsertTheMissing();
        }
    }
}

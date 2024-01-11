using FormProject.View.UserControls.CreateExercisesUCs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateExerciseUC.xaml
    /// </summary>
    public partial class CreateExerciseUC : UserControl
    {
        string login;
        public CreateExerciseUC(string login)
        {
            InitializeComponent();
            this.login = login;
            switchExerciseCreatingUC.Content = new CreateChooseCorrect(login);
        }

        private void WriteCodeChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseCreatingUC.Content = new CreateWriteCode(login);
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseCreatingUC.Content = new CreateYourAnswer(login);
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseCreatingUC.Content = new CreateChooseCorrect(login);
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            switchExerciseCreatingUC.Content = new CreateInsertTheMissing(login);
        }

        private void GetChosenTheme(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string theme = comboBox.SelectedItem.ToString().Split(new string[] { ": " },
                StringSplitOptions.None).Last();
            Console.WriteLine(theme);
        }
    }
}

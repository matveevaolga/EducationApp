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
        public CreateExerciseUC()
        {
            InitializeComponent();
        }

        private void WriteCodeChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
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

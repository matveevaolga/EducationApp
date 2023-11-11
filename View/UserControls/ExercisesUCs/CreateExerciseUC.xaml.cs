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
            switchExerciseUC.Content = new WriteCode();
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
            switchExerciseUC.Content = new YourAnswer();
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
            switchExerciseUC.Content = new ChooseCorrect();
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)sender;
            Console.WriteLine(comboBoxItem.Content);
            switchExerciseUC.Content = new InsertTheMissing();
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

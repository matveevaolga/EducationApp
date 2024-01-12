using FormProject.View.UserControls.CreateExercisesUCs;
using System;
using System.Collections.Generic;
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
        string Theme { get; set; }
        int Complexity { get; set; }
        string Description { get; set; }
        int Exp { get; set; }
        string Answer { get; set; }
        string AdditionalContent { get; set; }

        public CreateExerciseUC(string login)
        {
            InitializeComponent();
            DataContext = this;
            this.login = login;
            chooseExerciseType.Content = new CreateChooseCorrect(login);
        }

        private void WriteCodeChosen(object sender, RoutedEventArgs e)
        {
            chooseExerciseType.Content = new CreateWriteCode(login);
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            chooseExerciseType.Content = new CreateYourAnswer(login);
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            chooseExerciseType.Content = new CreateChooseCorrect(login);
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            chooseExerciseType.Content = new CreateInsertTheMissing(login);
        }

        private void GetChosenTheme(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string theme = comboBox.SelectedItem.ToString().Split(new string[] { ": " },
                StringSplitOptions.None).Last();
            Console.WriteLine(theme);
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            //string Theme = exerciseType.SelectedValue.ToString();
            //int Complexity = exerciseComplexity.Text;
            //string Description = exerciseCondition.Text;
            //int Exp = exerciseExp.Text;
            //string Answer = chooseExerciseType.correctOption.Text;
            //string AdditionalContent = chooseExerciseType.
            Dictionary<string, object> exerciseData = FormExerciseDict();
        }

        Dictionary<string, object> FormExerciseDict()
        {
            Dictionary<string, object> exerciseDict = new Dictionary<string, object>();
            exerciseDict["theme"] = Theme;
            exerciseDict["complexity"] = Complexity;
            exerciseDict["description"] = Description;
            exerciseDict["exp"] = Exp;
            exerciseDict["answer"] = Answer;
            exerciseDict["additionalContent"] = AdditionalContent;
            return exerciseDict;
        }
    }
}

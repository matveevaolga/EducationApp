using FormProject.Controller;
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
    /// Логика взаимодействия для ChooseCorrect.xaml
    /// </summary>
    public partial class ChooseCorrect : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;
        bool isSolved;

        public ChooseCorrect(Dictionary<string, string> exerciseData, string login)
        {
            InitializeComponent();
            this.exerciseData = exerciseData;
            this.login = login;
            idDesc.Content += exerciseData["id"];
            expDesc.Content += exerciseData["exp"];
            if (DBHelpFunctional.HelpIsSolved(login, int.Parse(exerciseData["id"])))
            {
                expDesc.Content = "Задача уже была решена";
                isSolved = true;
            }
            else isSolved = false;
            ShowExerciseDesc();
        }

        public void ShowExerciseDesc()
        {
            WrapPanel exDescStack = new WrapPanel();
            exDescStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            exDescStack.Orientation = Orientation.Vertical;
            TextBlock description = new TextBlock();
            description.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            description.Text = exerciseData["Описание"];
            description.TextWrapping = TextWrapping.Wrap;
            description.HorizontalAlignment = HorizontalAlignment.Left;
            exDescStack.Children.Add(description);

            string[] answers = exerciseData["Дополнительный контент"].Split('\n');
            foreach (string answer in answers)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Content = answer;
                checkBox.Style = Application.Current.FindResource("CheckBoxStyle") as Style;
                exDescStack.Children.Add(checkBox);
            }

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exDescStack;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }


        private void CheckIfCorrect(object sender, EventArgs e)
        {
            ScrollViewer scrollViewer = exerciseDesc.Content as ScrollViewer;
            WrapPanel exDescStack = scrollViewer.Content as WrapPanel;
            List<string> chosen = new List<string>();
            foreach (var element in exDescStack.Children)
            {
                if (element is CheckBox)
                {
                    CheckBox checkBoxElement = (CheckBox) element;
                    if ((bool)checkBoxElement.IsChecked) chosen.Add((string)checkBoxElement.Content);
                }
            }
            string input = string.Join("\n", chosen);
            if (input == exerciseData["Ответ"])
            {
                if (!isSolved)
                {
                    DBHelpFunctional.HelpIncreaseEXP(login, int.Parse(exerciseData["exp"]));
                    DBHelpFunctional.HelpAddToSolved(login, int.Parse(exerciseData["id"]));
                    result.Content = $"\tВерно!\nВам начислено {exerciseData["exp"]} exp";
                    result.Visibility = Visibility.Visible;
                    endButton.IsEnabled = false;
                }
                else
                {
                    result.Content = "Верно!";
                    result.Visibility = Visibility.Visible;
                    endButton.IsEnabled = false;
                }
            }
            else
            {
                result.Content = "Неверно...";
                result.Visibility = Visibility.Visible;
            }

        }
    }
}

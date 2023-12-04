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
    /// Логика взаимодействия для YourAnswer.xaml
    /// </summary>
    public partial class YourAnswer : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;
        bool isSolved;

        public YourAnswer(Dictionary<string, string> exerciseData, string login)
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
            ShowYourAnswerDesc();
        }

        public void ShowYourAnswerDesc()
        {
            WrapPanel exDescStack = new WrapPanel();
            exDescStack.Orientation = Orientation.Vertical;
            exDescStack.HorizontalAlignment = HorizontalAlignment.Left;
            string descText = exerciseData["Описание"];

            TextBlock description = new TextBlock();
            description.Text = descText;
            description.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            description.HorizontalAlignment = HorizontalAlignment.Left;

            TextBox usersAnswer = new TextBox();
            usersAnswer.Style = Application.Current.FindResource("TextBoxStyle") as Style;
            usersAnswer.HorizontalAlignment = HorizontalAlignment.Left;
            usersAnswer.TextWrapping = TextWrapping.Wrap;
            usersAnswer.MinWidth = 50;
            exDescStack.Children.Add(description);
            exDescStack.Children.Add(usersAnswer);
            usersAnswer.Name = "usersAnswer";

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exDescStack;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Left;
            exerciseDesc.Content = scrollViewer;
        }

        private void CheckIfCorrect(object sender, EventArgs e)
        {
            TextBox input = FindAnswer();
            if (input.Text == exerciseData["Ответ"])
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

        private TextBox FindAnswer()
        {
            ScrollViewer scrollViewer = exerciseDesc.Content as ScrollViewer;
            WrapPanel exDescStack = scrollViewer.Content as WrapPanel;
            foreach (object element in exDescStack.Children)
            {
                if (element is TextBox)
                {
                    TextBox usersAnswer = (TextBox)element;
                    return usersAnswer;
                }
            }
            return null;
        }
    }
}

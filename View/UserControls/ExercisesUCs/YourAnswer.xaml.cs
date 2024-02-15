using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        bool isInFavourite;
        public Brush StarButtonForeground { get; set; }

        public YourAnswer(Dictionary<string, string> exerciseData, string login)
        {
            InitializeComponent();
            BrushConverter converter = new BrushConverter();
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
            if (DBHelpFunctional.IsExerciseInFavourite(out string problem,
                login, exerciseData["id"]))
            {
                StarButtonForeground = (Brush)converter.ConvertFromString("#F2CD5C");
                isInFavourite = true;
            }
            else
            {
                StarButtonForeground = (Brush)converter.ConvertFromString("#282b4f");
                isInFavourite = false;
            }
            DataContext = this;
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

        private void FavoriteProcessing(object sender, RoutedEventArgs e)
        {
            if (isInFavourite)
            {
                DBHelpFunctional.HelpDeleteFromFavourite(int.Parse(exerciseData["id"]), login);
                VisualStateManager.GoToState(favouriteButton, "DeleteFromFavourite", false);
            }
            else
            {
                DBHelpFunctional.HelpAddToFavourite(int.Parse(exerciseData["id"]), login);
                VisualStateManager.GoToState(favouriteButton, "AddToFavourite", false);
            }
            isInFavourite = !isInFavourite;
        }
    }
}

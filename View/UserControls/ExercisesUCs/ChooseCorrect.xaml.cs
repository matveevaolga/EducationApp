using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        bool isInFavourite;
        public Brush StarButtonForeground { get; set; }

        public ChooseCorrect(Dictionary<string, string> exerciseData, string login)
        {
            BrushConverter converter = new BrushConverter();
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

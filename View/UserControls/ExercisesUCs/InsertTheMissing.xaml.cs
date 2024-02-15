using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для InsertTheMissing.xaml
    /// </summary>
    public partial class InsertTheMissing : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;
        bool isSolved;
        List<string> blanks;
        List<string> fullText;
        bool isInFavourite;
        public Brush StarButtonForeground { get; set; }

        public InsertTheMissing(Dictionary<string, string> exerciseData, string login)
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
            ShowExerciseDesc();
        }
        
        public void ShowExerciseDesc()
        {
            StackPanel mainPanel = new StackPanel();
            TextBox textBox;
            TextBlock textBlock = new TextBlock();
            textBlock.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = exerciseData["Описание"];
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            mainPanel.Children.Add(textBlock);

            WrapPanel exDescStack = new WrapPanel();
            exDescStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            exDescStack.Orientation = Orientation.Horizontal;
            string[] data = exerciseData["Ответ"].Split('$');
            string[] tags = exerciseData["Дополнительный контент"].Split();
            if (data.GetLength(0) != tags.GetLength(0)) { throw new ArgumentException(); }
            blanks = new List<string>();
            fullText = new List<string>();
            for (int i = 0; i < data.GetLength(0); i++)
            {
                fullText.Add(data[i].Trim());
                if (tags[i] == "Blank")
                {
                    blanks.Add(data[i].Trim());
                    textBox = new TextBox();
                    textBox.Style = Application.Current.FindResource("TextBoxStyle") as Style;
                    textBox.Width = data[i].Length * 100;
                    exDescStack.Children.Add(textBox);
                }
                else
                {
                    textBlock = new TextBlock();
                    textBlock.Text = data[i].Trim();
                    textBlock.Style = Application.Current.FindResource("TextBlockStyle") as Style;
                    exDescStack.Children.Add(textBlock);
                }
            }
            mainPanel.Children.Add(exDescStack);
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = mainPanel;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }

        private void CheckIfCorrect(object sender, EventArgs e)
        {
            ScrollViewer scrollViewer = exerciseDesc.Content as ScrollViewer;
            StackPanel exDescStack = scrollViewer.Content as StackPanel;
            WrapPanel contentPanel = exDescStack.Children[1] as WrapPanel;
            string input = "";
            foreach (var element in contentPanel.Children)
            {
                if (element is TextBox textBox) input += textBox.Text.Trim();
                if (element is TextBlock textBlock) input += textBlock.Text.Trim();
            }
            if (input == string.Join("", fullText)) 
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

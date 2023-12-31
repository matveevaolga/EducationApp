using FormProject.Controller;
using FormProject.Model;
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
using static FormProject.Controller.DBHelpFunctional;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для ShowExercisesUC.xaml
    /// </summary>
    public partial class ShowExercisesUC : UserControl
    {
        string login;

        public ShowExercisesUC(string login)
        {
            InitializeComponent();
            this.login = login;
            StackPanel exercisesStack = FillStack();
            exercisesStack.Name = "mainStack";
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Name = "scrollPanel";
            scrollViewer.Content = exercisesStack;
            showState.Content = scrollViewer;
        }

        private StackPanel FillStack()
        {
            List<Dictionary<string, string>> exersicesData = 
                DBHelpFunctional.HelpGetExersices(out string problem, login);
            if (exersicesData == null) { return null; }
            StackPanel exercisesStack = new StackPanel();
            for (int i = 0; i < exersicesData.Count; i++){
                Dictionary<string, string> data = exersicesData[i];
                StackPanel exercise = Description(data, i + 1);
                exercisesStack.Children.Add(exercise);
            }
            return exercisesStack;
        }

        private void ToExercise(object sender, EventArgs e)
        {
            Button toTheExercise = (Button)sender;
            StackPanel exercisesStack = (StackPanel)toTheExercise.Parent;
            Dictionary<string, string> exerciseData = new Dictionary<string, string>();
            foreach (var child in exercisesStack.Children)
            {
                if (child is TextBlock)
                {
                    TextBlock field = (TextBlock)child;
                    string[] data = field.Text.ToString().Split(new string[] {": "},
                        StringSplitOptions.None);
                    if (data.GetLength(0) == 1) exerciseData.Add(data[0], " ");
                    else
                    {
                        string[] fieldInfo = data.Skip(1).Take(data.GetLength(0)).ToArray();
                        exerciseData.Add(data[0], String.Join(": ", fieldInfo));
                    }

                }
            }
            switch (exerciseData["Тема"])
            {
                case "Вставить пропущенное":
                    showState.Content = new InsertTheMissing(exerciseData, login);
                    break;
                case "Вписать свой ответ":
                    showState.Content = new YourAnswer(exerciseData, login);
                    break;
                case "Выбрать правильные варианты":
                    showState.Content = new ChooseCorrect(exerciseData, login);
                    break;
                case "Написать код":
                    showState.Content = new WriteCode(exerciseData, login);
                    break;
                default:
                    Console.WriteLine("Ошибка в названии типа задачи");
                    break;
            }
        }

        public StackPanel Description(Dictionary<string, string> exerciseData, int exerciseNum)
        {
            var bc = new BrushConverter();
            
            TextBlock id = new TextBlock();
            id.Name = $"idy{exerciseNum}";
            id.Text = "id: " + exerciseData["id"];
            id.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            id.HorizontalAlignment = HorizontalAlignment.Stretch;
            id.Background = (Brush)bc.ConvertFrom("#FF535572");
            id.TextWrapping = TextWrapping.Wrap;

            TextBlock theme = new TextBlock();
            theme.Name = $"themey{exerciseNum}";
            theme.Text = "Тема: " + exerciseData["theme"];
            theme.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            theme.HorizontalAlignment = HorizontalAlignment.Stretch;
            theme.Background = (Brush)bc.ConvertFrom("#FF535572");
            theme.TextWrapping = TextWrapping.Wrap;

            TextBlock complexity = new TextBlock();
            complexity.Name = $"complexityy{exerciseNum}";
            complexity.Text = "Сложность: " + exerciseData["complexity"];
            complexity.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            complexity.HorizontalAlignment = HorizontalAlignment.Stretch;
            complexity.Background = (Brush)bc.ConvertFrom("#FF535572");
            complexity.TextWrapping = TextWrapping.Wrap;

            TextBlock description = new TextBlock();
            description.Name = $"descriptiony{exerciseNum}";
            description.Text = "Описание: " + exerciseData["description"];
            description.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            description.HorizontalAlignment = HorizontalAlignment.Stretch;
            description.Background = (Brush)bc.ConvertFrom("#FF535572");
            description.TextWrapping = TextWrapping.Wrap;   

            TextBlock exp = new TextBlock();
            exp.Name = $"expy{exerciseNum}";
            exp.Text = "exp: " + exerciseData["exp"];
            exp.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            exp.HorizontalAlignment = HorizontalAlignment.Stretch;
            exp.Background = (Brush)bc.ConvertFrom("#FF535572");
            exp.TextWrapping = TextWrapping.Wrap;

            Button toTheExercise = new Button();
            toTheExercise.Name = $"toTheExercisey{exerciseNum}";
            toTheExercise.Click += ToExercise;
            toTheExercise.Style = Application.Current.FindResource("ButtonStyle") as Style;
            toTheExercise.Content = $"К упражнению {exerciseData["id"]}";
            toTheExercise.HorizontalAlignment = HorizontalAlignment.Stretch;
            toTheExercise.HorizontalContentAlignment = HorizontalAlignment.Left;

            TextBlock answer = new TextBlock();
            answer.Name = $"answery{exerciseNum}";
            answer.Text = "Ответ: " + exerciseData["answer"];
            answer.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            answer.HorizontalAlignment = HorizontalAlignment.Stretch;
            answer.Background = (Brush)bc.ConvertFrom("#FF535572");
            answer.Visibility = Visibility.Collapsed;
            answer.TextWrapping = TextWrapping.Wrap;

            TextBlock additionaContent = new TextBlock();
            additionaContent.Name = $"additionalContenty{exerciseNum}";
            additionaContent.Text = "Дополнительный контент: " + exerciseData["additionalContent"];
            additionaContent.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            additionaContent.HorizontalAlignment = HorizontalAlignment.Stretch;
            additionaContent.Background = (Brush)bc.ConvertFrom("#FF535572");
            additionaContent.Visibility = Visibility.Collapsed;
            additionaContent.TextWrapping = TextWrapping.Wrap;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Name = $"exercisesStacky{exerciseNum}";
            stackPanel.Children.Add(id);
            stackPanel.Children.Add(theme);
            stackPanel.Children.Add(complexity);
            stackPanel.Children.Add(description);
            stackPanel.Children.Add(exp);
            stackPanel.Children.Add(toTheExercise);
            stackPanel.Children.Add(answer);
            stackPanel.Children.Add(additionaContent);
            return stackPanel;
        }
    }
}

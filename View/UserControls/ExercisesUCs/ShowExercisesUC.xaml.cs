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
using static FormProject.Controller.DBHelpFunctional;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для ShowExercisesUC.xaml
    /// </summary>
    public partial class ShowExercisesUC : UserControl
    {
        string login;

        public ShowExercisesUC(string problem, string login)
        {
            InitializeComponent();
            this.login = login;
            StackPanel exercisesStack = FillStack();
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Content = exercisesStack;
            showState.Content = scrollViewer;
        }

        public ShowExercisesUC(string login)
        {
            InitializeComponent();
            this.login = login;
            StackPanel exercisesStack = FillStack();
            ScrollViewer scrollViewer = new ScrollViewer();
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
            string exerciseNumber = exercisesStack.Name.Split('y')[1];
            Console.WriteLine(exercisesStack.Name == $"exercisesStacky{ exerciseNumber}");
            Console.WriteLine(FindName($"exercisesStacky{exerciseNumber}") as StackPanel == null);
            //Console.WriteLine(id.GetType());
            //string exersiceId = id.Content.ToString().Split('y')[1];
            TestExercise exercise = new TestExercise(exercisesStack.Name);
            showState.Content = exercise;
        }

        public StackPanel Description(Dictionary<string, string> exerciseData, int exerciseNum)
        {
            var bc = new BrushConverter();
            
            Label id = new Label();
            id.Name = $"idy{exerciseNum}";
            id.Content = "id: " + exerciseData["id"];
            id.Style = Application.Current.FindResource("LabelStyle") as Style;
            id.HorizontalAlignment = HorizontalAlignment.Stretch;
            id.Background = (Brush)bc.ConvertFrom("#252634");

            Label theme = new Label();
            theme.Name = $"themey{exerciseNum}";
            theme.Content = "Тема: " + exerciseData["theme"];
            theme.Style = Application.Current.FindResource("LabelStyle") as Style;
            theme.HorizontalAlignment = HorizontalAlignment.Stretch;
            theme.Background = (Brush)bc.ConvertFrom("#252634");

            Label complexity = new Label();
            complexity.Name = $"complexityy{exerciseNum}";
            complexity.Content = "Сложность: " + exerciseData["complexity"];
            complexity.Style = Application.Current.FindResource("LabelStyle") as Style;
            complexity.HorizontalAlignment = HorizontalAlignment.Stretch;
            complexity.Background = (Brush)bc.ConvertFrom("#252634");

            Label description = new Label();
            description.Name = $"descriptiony{exerciseNum}";
            description.Content = "Описание:\n" + exerciseData["description"];
            description.Style = Application.Current.FindResource("LabelStyle") as Style;
            description.HorizontalAlignment = HorizontalAlignment.Stretch;
            description.Background = (Brush)bc.ConvertFrom("#252634");

            Label exp = new Label();
            exp.Name = $"expy{exerciseNum}";
            exp.Content = "exp: " + exerciseData["exp"];
            exp.Style = Application.Current.FindResource("LabelStyle") as Style;
            exp.HorizontalAlignment = HorizontalAlignment.Stretch;
            exp.Background = (Brush)bc.ConvertFrom("#252634");

            Button toTheExercise = new Button();
            toTheExercise.Name = $"toTheExercisey{exerciseNum}";
            toTheExercise.Click += ToExercise;
            toTheExercise.Style = Application.Current.FindResource("ButtonStyle") as Style;
            toTheExercise.Content = $"К упражнению {exerciseData["id"]}";
            toTheExercise.HorizontalAlignment = HorizontalAlignment.Stretch;
            toTheExercise.HorizontalContentAlignment = HorizontalAlignment.Left;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Name = $"exercisesStacky{exerciseNum}";
            stackPanel.Children.Add(id);
            stackPanel.Children.Add(theme);
            stackPanel.Children.Add(complexity);
            stackPanel.Children.Add(description);
            stackPanel.Children.Add(exp);
            stackPanel.Children.Add(toTheExercise);
            return stackPanel;
        }
    }
}

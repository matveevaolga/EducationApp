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
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using System.Reflection.Emit;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для WriteCode.xaml
    /// </summary>
    public partial class WriteCode : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;
        bool isSolved;

        public WriteCode(Dictionary<string, string> exerciseData, string login)
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
            DockPanel exercisePanel = new DockPanel();
            exercisePanel.LastChildFill = true;

            TextBlock description = new TextBlock();
            description.Text = exerciseData["Описание"];
            description.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            description.TextWrapping = TextWrapping.Wrap;
            DockPanel.SetDock(description, Dock.Top);
            description.HorizontalAlignment = HorizontalAlignment.Left;
            exercisePanel.Children.Add(description);

            TextBox scriptForm = new TextBox();
            scriptForm.TextWrapping = TextWrapping.Wrap;
            DockPanel.SetDock(description, Dock.Top);
            scriptForm.Style = Application.Current.FindResource("TextBoxStyle") as Style;
            scriptForm.AcceptsReturn = true;
            scriptForm.AcceptsTab = true;
            exercisePanel.Children.Add(scriptForm);

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exercisePanel;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }

        private void CheckIfCorrect(object sender, EventArgs e)
        {
            ScrollViewer scrollViewer = exerciseDesc.Content as ScrollViewer;
            DockPanel exercisePanel = scrollViewer.Content as DockPanel;
            string input = "";
            foreach (var element in exercisePanel.Children)
            {
                if (element is TextBox textBox) input += textBox.Text;
            }
            bool result = ProcessInputScript(input);
        }

        private bool ProcessInputScript(string input)
        {
            string[] answers = exerciseData["Ответ"].Split(new string[] {"#\n"}, StringSplitOptions.None);
            string[] tests = exerciseData["Дополнительный контент"].Split();
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.Execute(input, scope);
            dynamic fib = scope.GetVariable("fib");
            for (int i = 0; i < answers.GetLength(0); i++)
            {
                dynamic output = fib(int.Parse(tests[i]));
                if (output != answers[i]) { Console.WriteLine(tests[i]); return false; }
            }
            return true;
        }
    }
}

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
            string res = ProcessInputScript(input);
            switch (res)
            {
                case "ok":
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
                    break;
                case "wrong":
                    result.Content = "Неверно...";
                    result.Visibility = Visibility.Visible;
                    break;
                default:
                    result.Content = $"Произошла ошибка {res}";
                    result.Visibility = Visibility.Visible;
                    break;
            }
        }

        private string ProcessInputScript(string input)
        {
            try
            {
                string[] answers = exerciseData["Ответ"].Split(new string[] { "#\n" }, StringSplitOptions.None);
                string[] tests = exerciseData["Дополнительный контент"].Split(new string[] { "#\n" }, StringSplitOptions.None);
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                engine.Execute(input, scope);
                dynamic function = scope.GetVariable("function");
                object[][] args_ = new object[tests.GetLength(0)][];
                for (int j = 0; j < tests.GetLength(0); j++)
                {
                    string[] currentTest = tests[j].Split('#');
                    args_[j] = new object[currentTest.GetLength(0)];
                    for (int i = 0; i < currentTest.GetLength(0); i++)
                    {
                        if (int.TryParse(currentTest[i], out int x)) args_[j][i] = x;
                        else if (double.TryParse(currentTest[i], out double y)) args_[j][i] = y;
                        else args_[j][i] = currentTest[i];
                    }
                }
                for (int i = 0; i < answers.GetLength(0); i++)
                {
                    dynamic output = function(args_[i]);
                    if (output != answers[i]) { Console.WriteLine(output + "|" + answers[i]); return "wrong"; }
                }
                return "ok";
            }
            catch (Exception ex) { return ex.GetType().Name; }
        }
    }
}

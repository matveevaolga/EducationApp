using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using FormProject.Model;
using System.Windows.Input;

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
        string functionName;

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
            functionName = GetFunctionName();
            ShowExerciseDesc();
        }

        public string GetFunctionName()
        {
            string description = exerciseData["Описание"];
            description = description.Split(new string[] {"функцию"}, StringSplitOptions.None)[1];
            description = description.Split('(')[0];
            description = description.Trim(new char[] { ' ', ',' });
            return description;
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
            //scriptForm.KeyUp += new KeyEventHandler(EnterPressed);
            exercisePanel.Children.Add(scriptForm);

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exercisePanel;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
            }
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
            string res = ProcessInputWithJsonTests(input);
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

        private string ProcessInputWithJsonTests(string input)
        {
            try
            {
                ScriptEngine engine = Python.CreateEngine();
                ScriptScope scope = engine.CreateScope();
                input += '\n' + System.IO.File.ReadAllText("..\\..\\Datas\\Tests\\TestScript.txt");
                engine.Execute(input, scope);
                dynamic helper = scope.GetVariable("helper");

                if (!int.TryParse(exerciseData["id"], out int idExercise)) throw new ArgumentException();
                JsonParsing.TestData[] tests = JsonParsing.ParseExercise("Exercise" + $"{idExercise}");
                foreach (JsonParsing.TestData test in tests)
                {
                    string[] Test = test.Test;
                    string Answer = test.Answer;
                    dynamic output = helper(Test, functionName);
                    if (output != Answer) { Console.WriteLine(output + "|" + Answer); return "wrong"; }
                }
                return "ok";
            }
            catch (Exception ex) { return ex.Message; }
        }
    }
}

using FormProject.Controller;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;
using FormProject.Model;
using System.Windows.Input;
using System.Resources;
using System.Linq;
using System.Text;

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
        ResourceManager rm;

        public WriteCode(Dictionary<string, string> exerciseData, string login)
        {
            rm = new ResourceManager("FormProject.Properties.Resources",
                typeof(DBFunctions).Assembly);
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
            try
            {
                string description = exerciseData["Описание"];
                description = description.Split(new string[] { "функцию" }, StringSplitOptions.None)[1];
                description = description.Split('(')[0];
                description = description.Trim(new char[] { ' ', ',' });
                return description;
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(rm.GetString("keyNotFoundException"), "GetFunctionName");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("keyNotFoundException"), "GetFunctionName"), login);
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine(rm.GetString("indexOutOfRangeException"), "GetFunctionName");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("indexOutOfRangeException"), "GetFunctionName"), login);
            }
            return "ошибка";
        }

        public void ShowExerciseDesc()
        {
            try
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
                scriptForm.KeyUp += new KeyEventHandler(EnterPressed);
                exercisePanel.Children.Add(scriptForm);

                ScrollViewer scrollViewer = new ScrollViewer();
                scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollViewer.Content = exercisePanel;
                scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                exerciseDesc.Content = scrollViewer;
            }
            catch (ResourceReferenceKeyNotFoundException)
            {
                Console.WriteLine(rm.GetString("resourceReferenceKeyNotFoundException"), "ShowExerciseDesc");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("resourceReferenceKeyNotFoundException"), "ShowExerciseDesc"), login);
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(rm.GetString("keyNotFoundException"), "ShowExerciseDesc");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("keyNotFoundException"), "ShowExerciseDesc"), login);
            }
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox scriptForm = sender as TextBox;
                string[] lines = scriptForm.Text.Split('\n');
                if (lines.GetLength(0) > 1 && lines[lines.GetLength(0) - 2].Length > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    string lastLine = lines[lines.GetLength(0) - 2];
                    foreach (char symb in lastLine)
                    {
                        if (symb == ' ' || symb == '\t') sb.Append(symb);
                        else break;
                    }
                    if (lastLine.EndsWith(":\r"))
                    {
                        sb.Append('\t');
                    }
                    scriptForm.AppendText(sb.ToString());
                }
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
            string res = ProcessInputWithJsonTests(input, out string comparison);
            switch (res)
            {
                case "ok":
                    if (!isSolved)
                    {
                        try
                        {
                            DBHelpFunctional.HelpIncreaseEXP(login, int.Parse(exerciseData["exp"]));
                            DBHelpFunctional.HelpAddToSolved(login, int.Parse(exerciseData["id"]));
                            result.Content = $"\tВерно!\nВам начислено {exerciseData["exp"]} exp";
                        }
                        catch (KeyNotFoundException)
                        {
                            Console.WriteLine(rm.GetString("keyNotFoundException"), "CheckIfCorrect");
                            LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("keyNotFoundException"), "CheckIfCorrect"), login);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine(rm.GetString("formatException"), "CheckIfCorrect");
                            LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("formatException"), "CheckIfCorrect"), login);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(rm.GetString("unknownException"), ex.GetType().Name, "CheckIfCorrect");
                            LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("unknownException"), ex.GetType().Name, "CheckIfCorrect"), login);
                        }
                        result.Content = "\tВерно!\tПри начислении exp произошла ошибка";
                    }
                    else
                    {
                        result.Content = "Верно!";
                        result.Visibility = Visibility.Visible;
                        endButton.IsEnabled = false;
                    }
                    break;
                case "wrong":
                    result.Content = $"Неверно: {comparison}";
                    result.Visibility = Visibility.Visible;
                    break;
                default:
                    result.Content = $"Произошла ошибка {res}";
                    result.Visibility = Visibility.Visible;
                    break;
            }
        }

        private string ProcessInputWithJsonTests(string input, out string comparison)
        {
            comparison = string.Empty;
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
                    if (output != Answer) { comparison = $"ваш ответ = {output}, а верный = {Answer}"; return "wrong"; }
                }
                return "ok";
            }
            catch (Exception ex) 
            {
                Console.WriteLine(rm.GetString("unknownException"), ex.GetType().Name, "ProcessInputWithJsonTests");
                LogsFileHelpFunctions.HelpWriteToLogsFile(string.Format(rm.GetString("unknownException"), ex.GetType().Name, "ProcessInputWithJsonTests"), login);
                return ex.Message; 
            }
        }
    }
}

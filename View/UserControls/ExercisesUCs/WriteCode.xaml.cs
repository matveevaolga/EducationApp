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
using System.Runtime.InteropServices.WindowsRuntime;

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
            if (DBHelpFunctional.IsExerciseInFavourite(out string problem,
                login, exerciseData["id"]))
                favouriteButton.Style = Resources["FavouriteButton"] as Style;
            else favouriteButton.Style = Resources["UnFavouriteButton"] as Style;
            ShowExerciseDesc();
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
                        sb.Append("    ");
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
            string message = "ok";
            ProcessInputWithJsonTests(input, ref message);
            switch (message)
            {
                case "ok":
                    if (!isSolved)
                    {
                        try
                        {
                            DBHelpFunctional.HelpIncreaseEXP(login, int.Parse(exerciseData["exp"]));
                            DBHelpFunctional.HelpAddToSolved(login, int.Parse(exerciseData["id"]));
                            result.Content = $"\tВерно!\nВам начислено {exerciseData["exp"]} exp";
                            result.Visibility = Visibility.Visible;
                            break;
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
                        result.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        result.Content = "Верно!";
                        endButton.IsEnabled = false;
                        result.Visibility = Visibility.Visible;
                    }
                    break;
                default:
                    result.Content = message;
                    result.Visibility = Visibility.Visible;
                    break;

            }
        }

        private void ProcessInputWithJsonTests(string input, ref string message)
        {
            try
            {
                string creatorLogin = DBHelpFunctional.HelpGetCreatorLoginByIdExercise
                    (int.Parse(exerciseData["id"]), login);
                JsonParsing.RunTestsFromJson(input, creatorLogin, int.Parse(exerciseData["id"]),
                    exerciseData["Дополнительный контент"], ref message);
            }
            catch (Exception ex) { message = ex.Message; }
        }

        private void FavoriteProcessing(object sender, RoutedEventArgs e)
        {
            if (favouriteButton.Style == Resources["FavouriteButton"] as Style)
            {
                favouriteButton.Style = Resources["UnFavouriteButton"] as Style;
                DBHelpFunctional.HelpDeleteFromFavourite(int.Parse(exerciseData["id"]), login);
            }
            else
            {
                favouriteButton.Style = Resources["FavouriteButton"] as Style;
                DBHelpFunctional.HelpAddToFavourite(int.Parse(exerciseData["id"]), login);
            }
        }
    }
}

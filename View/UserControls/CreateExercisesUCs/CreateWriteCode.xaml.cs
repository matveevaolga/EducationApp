using IronPython.Hosting;
using IronPython.Runtime.Operations;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Xml;
using System.Text.Json;
using System.Reflection;


namespace FormProject.View.UserControls.CreateExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateWriteCode.xaml
    /// </summary>
    public partial class CreateWriteCode : UserControl, INotifyPropertyChanged
    {
        string[] types = { "int", "float", "list", "bool", "str", "dict" };
        string funcName;
        public string tests;
        Color colorLight = (Color)ColorConverter.ConvertFromString("#dedee8");
        Color colorDark = (Color)ColorConverter.ConvertFromString("#282b4f");
        Color colorRed = (Color)ColorConverter.ConvertFromString("#FF0000");
        bool incorrectSignature;
        public bool IncorrectSignature 
        {   
            get { return incorrectSignature; }
            set { incorrectSignature = value; OnPropertyChanged("IncorrectSignature"); } 
        }
        bool noTests;
        public bool NoTests
        {
            get { return noTests; }
            set { noTests = value; OnPropertyChanged("NoTests"); }
        }
        bool allTestsPassed;
        public bool AllTestsPassed
        {
            get { return allTestsPassed; }
            set { allTestsPassed = value; OnPropertyChanged("AllTestsPassed"); }
        }
        bool incorrectTestResult;
        public bool IncorrectTestResult
        {
            get { return incorrectTestResult; }
            set { incorrectTestResult = value; OnPropertyChanged("IncorrectTestResult"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public CreateWriteCode()
        {
            InitializeComponent();
            IncorrectSignature = false;
            NoTests = false;
            AllTestsPassed = false;
            IncorrectTestResult = false;
            DataContext = this;
        }

        public void GetWriteCodeExerciseData(out string answer, out string additionalContent)
        {
            answer = additionalContent = string.Empty;
            NoTests = createdTests.Items.Count == 0;
            if (signature.Text == string.Empty) IncorrectSignature = true;
            if (noTests || IncorrectSignature) return;
            TryRunTests();
            if (IncorrectTestResult) return;
            AllTestsPassed = true;
            additionalContent = signature.Text;
            answer = "-";
        }

        private void TryRunTests()
        {
            List<string> testsList = new List<string>();
            string fullScript;
            string function = solution.Text;
            string helperScript = System.IO.File.
                ReadAllText("..\\..\\Datas\\Tests\\TestScript.txt",
                Encoding.GetEncoding("windows-1251"));
            ScriptEngine engine = Python.CreateEngine();
            foreach (string test in createdTests.Items)
            {
                try
                {
                    testsList.Add(test);
                    fullScript = $"{function}\ntest_data={test}\nfunction_name=\"{funcName}\"\n{helperScript}";
                    engine.Execute(fullScript);
                }
                catch (Exception ex)
                {
                    failedTest.Text = $"Ошибка: {ex.Message} на тесте {test}";
                    IncorrectTestResult = true;
                    break;
                }
            }
            tests = $"[{string.Join(", ", testsList)}]";
        }

        private void DeleteTest(object sender, RoutedEventArgs e)
        {
            if (createdTests.SelectedValue != null)
            {
                createdTests.Items.Remove(createdTests.SelectedValue.ToString());
                createdTests.Text = "Созданные тесты";
            }
        }

        private void CreateTest(object sender, RoutedEventArgs e)
        {
            Label label = null;
            TextBox textBox = null;
            List<string> parameters = new List<string>();
            foreach (WrapPanel parameter in testData.Children)
            {
                if (parameter.Tag == null || parameter.Tag.ToString() == "IncorrectInput") return;
                foreach (object data in parameter.Children)
                {
                    if (data is Label) label = data as Label;
                    if (data is TextBox) textBox = data as TextBox;
                }
                parameters.Add($"\"{label.Content.ToString().Split(':')[0].Trim()}" +
                    $"\": {textBox.Text}");
            }
            string test = $"{{{string.Join(", ", parameters.ToArray())}}}";
            if (test != string.Empty && (createdTests.Items.Count == 0 || !createdTests.Items.Contains(test)))
                { createdTests.Items.Add(test); NoTests = false; }
        }

        private void CheckSignature(object sender, TextChangedEventArgs e)
        {
            createdTests.Items.Clear();
            testData.Children.Clear();
            TextBox textBox = sender as TextBox;
            string signature = textBox.Text;
            string[] parameters;
            string answer;
            try
            {
                TryRunSignature(signature);
                funcName = signature.Substring(0, signature.IndexOf("("));
                parameters = signature.Substring(signature.IndexOf("(") + 1,
                    signature.IndexOf(")") - signature.IndexOf("(") - 1).Split(',');
                foreach (string param in parameters)
                {
                    string parameterName = param.Split(':')[0].Trim();
                    string parameterType = param.Split(':')[1].Trim();
                    if (!types.Contains(parameterType)) throw new FormatException();
                    AddParameter(parameterName, parameterType);
                }
                answer = signature.Substring(signature.IndexOf(")") + 1).Replace("->", "").Trim();
                if (!types.Contains(answer)) throw new FormatException();
                AddParameter("Ответ", answer);
                IncorrectSignature = false;
            }
            catch { IncorrectSignature = true; testData.Children.Clear(); }
        }

        private void TryRunSignature(string signature)
        {
            if (signature.Contains("=")) throw new FormatException();
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            engine.Execute($"def {signature}:\n\tpass", scope);
        }

        private void AddParameter(string parameterName, string parameterType)
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Orientation = Orientation.Horizontal;
            Thickness margin = wrapPanel.Margin;
            if (testData.Children.Count != 0) margin.Left = 10;
            wrapPanel.Margin = margin;

            Label label = new Label();
            label.Content = parameterName + ": " + parameterType;
            label.Style = Application.Current.FindResource("LabelStyle") as Style;
            label.Foreground = new SolidColorBrush(colorLight);
            label.Background = new SolidColorBrush(colorDark);
            wrapPanel.Children.Add(label);

            TextBox textBox = new TextBox();
            textBox.Style = Application.Current.FindResource("TextBoxStyle") as Style;
            textBox.Width = parameterType.Length * 50;
            textBox.Tag = parameterType;
            textBox.TextChanged += CheckParameterValue;
            wrapPanel.Children.Add(textBox);

            testData.Children.Add(wrapPanel);
        }

        private void CheckParameterValue(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string value = textBox.Text;
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();

            WrapPanel wrapPanel = textBox.Parent as WrapPanel;
            Label label = null;
            foreach (object child in wrapPanel.Children) 
            { if (child is Label) label = (Label)child; }
            try
            {
                engine.Execute($"value_type = type({value}).__name__", scope);
                if (scope.GetVariable("value_type") != textBox.Tag.ToString()) throw new FormatException();
                if (textBox.Tag.ToString() == "list" || textBox.Tag.ToString() == "dict")
                    CheckListParameter(value);
                wrapPanel.Tag = "CorrectInput";
                label.Foreground = new SolidColorBrush(colorLight);
            }
            catch
            {
                wrapPanel.Tag = "IncorrectInput";
                if (label != null) label.Foreground = new SolidColorBrush(colorRed);
            }
        }

        private void CheckListParameter(string structData)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string structTraversal = System.IO.File.
                ReadAllText("..\\..\\Datas\\Tests\\StructTraversalScript.txt");
            structTraversal = $"values = {structData}\n{structTraversal}";
            scope.SetVariable("types", types);
            engine.Execute(structTraversal, scope);
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
    }
}

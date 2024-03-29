﻿using FormProject.Controller;
using FormProject.Model;
using FormProject.View.UserControls.CreateExercisesUCs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateExerciseUC.xaml
    /// </summary>
    public partial class CreateExerciseUC : UserControl, INotifyPropertyChanged
    {
        string login;
        string theme;
        int complexity;
        string description;
        int exp;
        string answer;
        string additionalContent;
        List<string> incorrectlyFilled;
        bool problem;
        public bool Problem { get { return problem; } set
            { problem = value; OnPropertyChanged("Problem"); } }

        public CreateExerciseUC(string login)
        {
            InitializeComponent();
            this.login = login;
            Problem = false;
            DataContext = this;
            chooseExerciseType.Content = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void WriteCodeChosen(object sender, RoutedEventArgs e)
        {
            exerciseCondition.Style = Resources["SignatureTip"] as Style;
            chooseExerciseType.Content = new CreateWriteCode();
        }

        private void YourAnswerChosen(object sender, RoutedEventArgs e)
        {
            exerciseCondition.Style = Application.Current.Resources["TextBoxStyle"] as Style;
            chooseExerciseType.Content = new CreateYourAnswer();
        }

        private void ChooseCorrectChosen(object sender, RoutedEventArgs e)
        {
            exerciseCondition.Style = Application.Current.Resources["TextBoxStyle"] as Style;
            chooseExerciseType.Content = new CreateChooseCorrect();
        }

        private void InsertTheMissingChosen(object sender, RoutedEventArgs e)
        {
            exerciseCondition.Style = Application.Current.Resources["TextBoxStyle"] as Style;
            chooseExerciseType.Content = new CreateInsertTheMissing();
        }

        private void GetChosenTheme(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            string theme = comboBox.SelectedItem.ToString().Split(new string[] { ": " },
                StringSplitOptions.None).Last();
        }

        private void Finish(object sender, RoutedEventArgs e)
        {
            incorrectlyFilled = new List<string>();
            GetGeneralData();
            if (incorrectlyFilled.Count > 0) { FinishDenied(); return; }
            GetDataSpecificForExerciseType();
            if (answer == string.Empty || additionalContent == string.Empty) return;
            if (theme == "Написать код" && !description.Contains(additionalContent)) 
                { 
                    exerciseCondition.Tag = "IncorrectInput"; 
                    exerciseCondition.Text = string.Empty;
                    return; 
                }
            Dictionary<string, object> exerciseData = FormExerciseDict();
            DBHelpFunctional.HelpCreateExercise(login, exerciseData, ref problem);
            if (problem) return;
            if (theme == "Написать код") try { AddTestsToJson(); } catch { Problem = true; DeleteExercise(); return; }
            success.Visibility = Visibility.Visible; finishButton.IsEnabled = false; 
        }

        void GetGeneralData()
        {
            if (exerciseType.SelectedItem != null)
            {
                ComboBoxItem comboBoxItem = exerciseType.SelectedItem as ComboBoxItem;
                theme = comboBoxItem.Content.ToString();
            }
            else incorrectlyFilled.Add("exerciseType");
            description = exerciseCondition.Text;
            if (description == string.Empty) incorrectlyFilled.Add("exerciseCondition");
            if (!int.TryParse(exerciseExp.Text, out exp) || exp < 1 || exp > 10)
                incorrectlyFilled.Add("exerciseExp");
            if (!int.TryParse(exerciseComplexity.Text, out complexity) || complexity < 1 || complexity > 10)
                incorrectlyFilled.Add("exerciseComplexity");
        }

        void GetDataSpecificForExerciseType()
        {
            switch (theme)
            {
                case "Написать код":
                    CreateWriteCode writeCode = chooseExerciseType.Content as CreateWriteCode;
                    writeCode.GetWriteCodeExerciseData(out answer, out additionalContent);
                    break;
                case "Вписать свой ответ":
                    CreateYourAnswer yourAnswer = chooseExerciseType.Content as CreateYourAnswer;
                    yourAnswer.GetYourAnswerExerciseData(out answer, out additionalContent);
                    break;
                case "Выбрать верное":
                    CreateChooseCorrect chooseCorrect = chooseExerciseType.Content as CreateChooseCorrect;
                    chooseCorrect.GetChooseCorrectExerciseData(out answer, out additionalContent);
                    break;
                case "Вставить пропущенное":
                    CreateInsertTheMissing insertTheMissing = chooseExerciseType.Content as CreateInsertTheMissing;
                    insertTheMissing.GetInsertTheMissingExerciseData(out answer, out additionalContent);
                    break;
                default:
                    incorrectlyFilled.Add("exerciseType");
                    break;
            }
        }

        void FinishDenied()
        {
            TextBox textBox;
            ComboBox comboBox;
            foreach (string field in incorrectlyFilled)
            {
                if (FindName(field) is TextBox)
                {
                    textBox = FindName(field) as TextBox;
                    textBox.Text = string.Empty;
                    textBox.Tag = "IncorrectInput";
                }
                else if (FindName(field) is ComboBox)
                {
                    comboBox = FindName(field) as ComboBox;
                    comboBox.Foreground = new SolidColorBrush(Colors.Red);
                    comboBox.Text = "Выбор не сделан";
                }
            }
        }

        Dictionary<string, object> FormExerciseDict()
        {
            Dictionary<string, object> exerciseDict = new Dictionary<string, object>();
            exerciseDict["theme"] = theme;
            exerciseDict["complexity"] = complexity;
            exerciseDict["description"] = description;
            exerciseDict["exp"] = exp;
            exerciseDict["answer"] = answer;
            exerciseDict["additionalContent"] = additionalContent;
            return exerciseDict;
        }

        private void ResetTextBoxBackground(object sender, TextChangedEventArgs args)
        {
            TextBox textBox = sender as TextBox;
            textBox.Tag = "Reset";
        }

        private void ResetComboBox(object sender, MouseEventArgs args)
        {
            ComboBox comboBox = sender as ComboBox;
            Color color = (Color)ColorConverter.ConvertFromString("#dedee8");
            comboBox.Foreground = new SolidColorBrush(color);
            comboBox.Text = "Тип задачи";
        }

        void AddTestsToJson()
        {
            CreateWriteCode writeCode = chooseExerciseType.Content as CreateWriteCode;
            string tests = writeCode.tests;
            int exerciseId = DBHelpFunctional.HelpGetLastExerciseId(login);
            if (exerciseId == 0) { throw new ArgumentException(); }
            JsonParsing.WriteExerciseToJson(tests, login, exerciseId);
        }

        void DeleteExercise()
        {
            int exerciseId = DBHelpFunctional.HelpGetLastExerciseId(login);
            DBHelpFunctional.HelpDeleteExercise(exerciseId, login);
        }
    }
}

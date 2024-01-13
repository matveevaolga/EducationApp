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

namespace FormProject.View.UserControls.CreateExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateChooseCorrect.xaml
    /// </summary>
    public partial class CreateChooseCorrect : UserControl
    {
        string login;

        public CreateChooseCorrect(string login)
        {
            InitializeComponent();
            this.login = login;
        }

        void AddNewOption(object sender, RoutedEventArgs e)
        {
            if (optionToAdd.Text != string.Empty)
                allOptions.Items.Add(optionToAdd.Text);
        }

        void DeleteOption(object sender, RoutedEventArgs e)
        {
            if (allOptions.SelectedValue != null)
                allOptions.Items.Remove(allOptions.SelectedValue.ToString());
        }

        public void GetChooseCorrectExerciseData(out string answer, out string additionalContent)
        {
            answer = correctOption.Text;
            List<string> options = new List<string>();
            foreach (string item in allOptions.Items) options.Add(item);
            additionalContent = string.Join("\n", options);
            if (answer == string.Empty) correctOption.Tag = "IncorrectInput";
            if (additionalContent == string.Empty || !options.Contains(answer))
            {
                allOptions.Foreground = new SolidColorBrush(Colors.Red);
                allOptions.Text = "Вариант с правильным ответом не создан";
            }
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
            comboBox.Text = "Варианты ответа";
        }
    }
}

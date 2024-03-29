﻿using System;
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


namespace FormProject.View.UserControls.CreateExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateInsertTheMissing.xaml
    /// </summary>
    public partial class CreateInsertTheMissing : UserControl, INotifyPropertyChanged
    {
        Color colorLight = (Color)ColorConverter.ConvertFromString("#dedee8");
        Color colorDark = (Color)ColorConverter.ConvertFromString("#282b4f");
        Color colorRed = (Color)ColorConverter.ConvertFromString("#FF0000");
        bool problemText;
        bool problemBlank;
        public bool ProblemText { get { return problemText; } set 
            { problemText = value; OnPropertyChanged("ProblemText"); } }
        public bool ProblemBlank { get { return problemBlank; } set 
            { problemBlank = value; OnPropertyChanged("ProblemBlank"); } }
        public CreateInsertTheMissing()
        {
            InitializeComponent();
            ProblemText = false;
            ProblemBlank = false;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void GetInsertTheMissingExerciseData(out string answer, out string additionalContent)
        {
            answer = string.Empty;
            additionalContent = string.Empty;
            List<string> answerText = new List<string>();
            List<string> tags = new List<string>();
            foreach (WrapPanel wrap in allText.Children)
            {
                foreach (object child in wrap.Children)
                {
                    if (child is Label)
                    {
                        Label label = child as Label;
                        answerText.Add(label.Content.ToString());
                        tags.Add(label.Tag.ToString());
                    }
                }
            }
            if (!tags.Contains("TextBlock")) ProblemText = true; 
            else ProblemText = false;
            if (!tags.Contains("Blank")) ProblemBlank = true;
            else ProblemBlank = false;
            if (ProblemBlank || ProblemText) return;
            answer = string.Join("$", answerText);
            additionalContent = string.Join(" ", tags);
        }

        private void AddTextBlock(object sender, RoutedEventArgs e) 
        {
            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Orientation = Orientation.Horizontal;
            Thickness margin = wrapPanel.Margin;
            if (allText.Children.Count != 0) margin.Left = 10;
            wrapPanel.Margin = margin;

            Label label = new Label();
            label.Content = addingTextTextBox.Text;
            label.Style = Application.Current.FindResource("LabelStyle") as Style;
            label.Foreground = new SolidColorBrush(colorLight);
            label.Background = new SolidColorBrush(colorDark);
            label.Tag = "TextBlock";
            wrapPanel.Children.Add(label);

            Button button = new Button();
            button.Style = Application.Current.FindResource("ButtonStyle") as Style;
            button.Click += DeleteBlock;
            button.Content = "X";
            button.Background = new SolidColorBrush(colorRed);
            wrapPanel.Children.Add(button);

            allText.Children.Add(wrapPanel);
        }

        private void AddBlank(object sender, RoutedEventArgs e)
        {

            WrapPanel wrapPanel = new WrapPanel();
            wrapPanel.Orientation = Orientation.Horizontal;
            Thickness margin = wrapPanel.Margin;
            margin.Left = 10;
            wrapPanel.Margin = margin;

            Label label = new Label();
            label.Content = addingBlankTextBox.Text;
            label.Style = Application.Current.FindResource("LabelStyle") as Style;
            label.Background = new SolidColorBrush(colorLight);
            label.Foreground = new SolidColorBrush(colorDark);
            label.Tag = "Blank";
            wrapPanel.Children.Add(label);

            Button button = new Button();
            button.Style = Application.Current.FindResource("ButtonStyle") as Style;
            button.Click += DeleteBlock;
            button.Content = "X";
            button.Background = new SolidColorBrush(colorRed);
            wrapPanel.Children.Add(button);

            allText.Children.Add(wrapPanel);
        }

        void DeleteBlock(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            WrapPanel wrapPanel = button.Parent as WrapPanel;
            wrapPanel.Children.Clear();
            wrapPanel.Visibility = Visibility.Collapsed;
        }
    }
}

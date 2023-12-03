﻿using System;
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

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для InsertTheMissing.xaml
    /// </summary>
    public partial class InsertTheMissing : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;

        public InsertTheMissing(Dictionary<string, string> exerciseData, string login)
        {
            InitializeComponent();
            this.exerciseData = exerciseData;
            this.login = login;
            idDesc.Content += exerciseData["id"];
            expDesc.Content += exerciseData["exp"];
            ShowExerciseDesc();
        }
        
        public void ShowExerciseDesc()
        {
            WrapPanel exDescStack = new WrapPanel();
            exDescStack.HorizontalAlignment = HorizontalAlignment.Stretch;
            exDescStack.Orientation = Orientation.Horizontal;
            String[] description = exerciseData["Описание"].Split('#');
            foreach (string descPiece in description)
            {
                TextBlock l = new TextBlock();
                l.Text = descPiece;
                l.Style = Application.Current.FindResource("TextBlockStyle") as Style;
                exDescStack.Children.Add(l);
                if (descPiece != description[description.GetLength(0) - 1])
                {
                    Grid grid = new Grid();
                    grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextBox textBox = new TextBox();
                    textBox.Style = Application.Current.FindResource("TextBoxStyle") as Style;
                    textBox.TextWrapping = TextWrapping.Wrap;
                    grid.Children.Add(textBox);
                    exDescStack.Children.Add(grid);
                }
            }
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Content = exDescStack;
            exerciseDesc.Content = scrollViewer;
        }
    }
}

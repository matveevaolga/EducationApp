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
    }
}

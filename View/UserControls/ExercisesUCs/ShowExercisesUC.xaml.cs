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
using static FormProject.Controller.DBHelpFunctional;

namespace FormProject.View.UserControls.ExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для ShowExercisesUC.xaml
    /// </summary>
    public partial class ShowExercisesUC : UserControl
    {
        public ShowExercisesUC(string problem)
        {
            InitializeComponent();
            FillStack();
        }

        public ShowExercisesUC()
        {
            InitializeComponent();
            FillStack();
        }

        private void FillStack()
        {
            Dictionary<string, string>[] exersicesData = DBHelpFunctional.HelpGetExersices(); 
        }
    }
}

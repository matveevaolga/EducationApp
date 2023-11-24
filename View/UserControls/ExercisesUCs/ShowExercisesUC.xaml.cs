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
        string login;

        public ShowExercisesUC(string problem, string login)
        {
            InitializeComponent();
            this.login = login;
            FillStack();
        }

        public ShowExercisesUC(string login)
        {
            InitializeComponent();
            this.login = login;
            FillStack();
        }

        private void FillStack()
        {
            List<Dictionary<string, string>> exersicesData = DBHelpFunctional.HelpGetExersices(out string problem, login);
            if (exersicesData == null) { return; }
            foreach (Dictionary<string, string> data in exersicesData)
            {
                ExerciseDescription exersice = new ExerciseDescription(data);
                exercisesStack.Children.Add(exersice);
            }
        }
    }
}

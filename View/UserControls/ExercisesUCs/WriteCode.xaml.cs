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

        public WriteCode(Dictionary<string, string> exerciseData, string login)
        {
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
            ShowExerciseDesc();
        }

        public void ShowExerciseDesc() { }

        private void CheckIfCorrect(object sender, EventArgs e) { }
    }
}

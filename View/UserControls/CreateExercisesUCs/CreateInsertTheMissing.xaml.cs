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
    /// Логика взаимодействия для CreateInsertTheMissing.xaml
    /// </summary>
    public partial class CreateInsertTheMissing : UserControl
    {
        string login;
        public CreateInsertTheMissing(string login)
        {
            InitializeComponent();
            this.login = login;
        }

        public void GetInsertTheMissingExerciseData(out string answer, out string additionalContent)
        {
            answer = additionalContent = string.Empty;
        }
    }
}

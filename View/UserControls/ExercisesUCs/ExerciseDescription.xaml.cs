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
    /// Логика взаимодействия для ExerciseDescription.xaml
    /// </summary>
    public partial class ExerciseDescription : UserControl
    {
        public ExerciseDescription(Dictionary<string, string> exerciseData)
        {
            InitializeComponent();
            DataContext = new DescriptionInfo(exerciseData);
        }

        class DescriptionInfo
        {
            public string id { get; set; }
            public string theme { get; set; }
            public string complexity { get; set; }
            public string description { get; set; }
            public string exp { get; set; }

            public DescriptionInfo(Dictionary<string, string> exerciseData)
            {
                id = "id: " + exerciseData["id"];
                theme = "Тема: " + exerciseData["theme"];
                complexity = "Сложность: " + exerciseData["complexity"];
                description = "Описание:\n" + exerciseData["description"];
                exp = "exp: " + exerciseData["exp"];
            }
        }
    }
}

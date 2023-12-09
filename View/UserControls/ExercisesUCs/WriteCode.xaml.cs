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
using Microsoft.Scripting.Hosting;
using IronPython.Hosting;

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

        public void ShowExerciseDesc()
        {
            DockPanel exercisePanel = new DockPanel();
            exercisePanel.LastChildFill = true;

            TextBlock description = new TextBlock();
            description.Text = exerciseData["Описание"];
            description.Style = Application.Current.FindResource("TextBlockStyle") as Style;
            description.TextWrapping = TextWrapping.Wrap;
            DockPanel.SetDock(description, Dock.Top);
            description.HorizontalAlignment = HorizontalAlignment.Left;
            exercisePanel.Children.Add(description);

            TextBox scriptForm = new TextBox();
            scriptForm.TextWrapping = TextWrapping.Wrap;
            DockPanel.SetDock(description, Dock.Top);
            scriptForm.Style = Application.Current.FindResource("TextBoxStyle") as Style;
            scriptForm.AcceptsReturn = true;
            scriptForm.AcceptsTab = true;
            exercisePanel.Children.Add(scriptForm);

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exercisePanel;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }

        private void CheckIfCorrect(object sender, EventArgs e) { }
    }
}

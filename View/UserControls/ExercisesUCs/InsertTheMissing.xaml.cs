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
    /// Логика взаимодействия для InsertTheMissing.xaml
    /// </summary>
    public partial class InsertTheMissing : UserControl
    {
        Dictionary<string, string> exerciseData;
        string login;
        bool isSolved;

        public InsertTheMissing(Dictionary<string, string> exerciseData, string login)
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
                    TextBox textBox = new TextBox();
                    textBox.Style = Application.Current.FindResource("TextBoxStyle") as Style;
                    textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    textBox.TextWrapping = TextWrapping.Wrap;
                    textBox.MinWidth = 50;
                    exDescStack.Children.Add(textBox);
                }
            }
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = exDescStack;
            scrollViewer.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            exerciseDesc.Content = scrollViewer;
        }

        private void CheckIfCorrect(object sender, EventArgs e)
        {
            ScrollViewer scrollViewer = exerciseDesc.Content as ScrollViewer;
            WrapPanel exDescStack = scrollViewer.Content as WrapPanel;
            string input = "";
            foreach (var element in exDescStack.Children)
            {
                if (element is TextBox textBox) input += textBox.Text;
                if (element is TextBlock textBlock) input += textBlock.Text;
            }
            if (input == exerciseData["Ответ"])
            {
                if (!isSolved)
                {
                    DBHelpFunctional.HelpIncreaseEXP(login, int.Parse(exerciseData["exp"]));
                    DBHelpFunctional.HelpAddToSolved(login, int.Parse(exerciseData["id"]));
                    result.Content = $"\tВерно!\nВам начислено {exerciseData["exp"]} exp";
                    result.Visibility = Visibility.Visible;
                    endButton.IsEnabled = false;
                }
                else
                {
                    result.Content = "Верно!";
                    result.Visibility = Visibility.Visible;
                    endButton.IsEnabled = false;
                }
            }
            else
            {
                result.Content = "Неверно...";
                result.Visibility = Visibility.Visible;
            }
        }
    }
}

using System.Windows.Controls;

namespace FormProject.View.UserControls.CreateExercisesUCs
{
    /// <summary>
    /// Логика взаимодействия для CreateYourAnswer.xaml
    /// </summary>
    public partial class CreateYourAnswer : UserControl
    {
        public CreateYourAnswer()
        {
            InitializeComponent();
        }

        public void GetYourAnswerExerciseData(out string answer, out string additionalContent)
        {
            additionalContent = "-";
            answer = correctAnswer.Text;
            if (answer == string.Empty) correctAnswer.Tag = "IncorrectInput";
        }

        private void ResetTextBoxBackground(object sender, TextChangedEventArgs args)
        {
            TextBox textBox = sender as TextBox;
            textBox.Tag = "Reset";
        }
    }
}

using System.Windows.Controls;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SettingsUC.xaml
    /// </summary>
    public partial class SettingsUC : UserControl
    {
        public SettingsUC(string login)
        {
            InitializeComponent();
            DataContext = new SettingsInfo(login);
        }
    }

    class SettingsInfo
    {
        public SettingsInfo(string login)
        {

        }
    }
}

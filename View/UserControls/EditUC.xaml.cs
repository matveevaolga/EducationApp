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

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EditUC.xaml
    /// </summary>
    public partial class EditUC : UserControl
    {
        public EditUC()
        {
            InitializeComponent();
            DataContext = new EditInfo();
        }

        class EditInfo
        {
            public EditInfo()
            {

            }
        }

        private void EditProfile(object sender, EventArgs e)
        {
            DBFunctions dBFunctions = new DBFunctions();
            if (nameChange.Text != "")
            {
                bool result = dBFunctions.ChangeField(UserProfile.login, "profiles", "name", nameChange.Text);
                nameChange.Text = result == true ? "" : "ошибка, не удалось изменить поле";
            }
            if (infoChange.Text != "")
            {
                bool result = dBFunctions.ChangeField(UserProfile.login, "profiles", "about", infoChange.Text);
                infoChange.Text = result == true ? "" : "ошибка, не удалось изменить поле";
            }
        }
    }
}

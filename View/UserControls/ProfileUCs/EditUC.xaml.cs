using FormProject.Controller;
using System;

namespace FormProject.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для EditUC.xaml
    /// </summary>
    public partial class EditUC: System.Windows.Controls.UserControl
    {
        string login;

        public EditUC(string login)
        {
            this.login = login;
            InitializeComponent();
            DataContext = new EditInfo(login);
        }

        class EditInfo
        {
            public EditInfo(string login)
            {

            }
        }

        private void EditProfile(object sender, EventArgs e)
        {
            if (nameChange.Text != "")
            {
                bool result = DBHelpFunctional.HelpChangeField(this.login, "profiles", "name", nameChange.Text, out string problem);
                nameChange.Text = result == true ? "" : problem;
            }
            if (infoChange.Text != "")
            {
                bool result = DBHelpFunctional.HelpChangeField(this.login, "profiles", "about", infoChange.Text, out string problem);
                infoChange.Text = result == true ? "" : problem;
            }
        }
    }
}

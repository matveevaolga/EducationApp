using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormProject.Controller
{
    public class DBHelpFunctional
    {
        public static string HelpGetProfileField(string login, string field)
        {
            DBFunctions dBFunctions = new DBFunctions();
            return dBFunctions.GetProfileField(login, field);
        }

        public static string HelpGetStatsField(string login, string field)
        {
            DBFunctions dBFunctions = new DBFunctions();
            return dBFunctions.GetStatsField(login, field);
        }

        public static bool HelpChangeField(string login, string table, string column, string value)
        {
            DBFunctions dBFunctions = new DBFunctions();
            return dBFunctions.ChangeField(login, table, column, value);
        }
    }
}

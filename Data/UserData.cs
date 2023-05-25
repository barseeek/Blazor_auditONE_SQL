using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor_auditONE_SQL.Data
{
    public static class AuthorizedUsers
    {
        public static List<UserData> Users = new List<UserData>();

        /*public static List<int> getSigurIds(string samaccountname)
        {
            try
            {
                return Users.Where(u => u.samaccountname == samaccountname).First().DepartmentSigurIDs;
            }
            catch (Exception)
            {
                return null;
            }
        }*/
        public static string getUserRole(string samaccountname)
        {
            try
            {
                return Users.Where(u => u.samaccountname == samaccountname).First().role;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /*public static string getFIO(string samaccountname)
        {
            try
            {
                return Users.Where(u => u.samaccountname == samaccountname).First().FIO;
            }
            catch (Exception)
            {
                return null;
            }
        }*/

        public static void LogOutUser(string samaccountname)
        {
            Users.RemoveAll(u => u.samaccountname == samaccountname);
        }

        public static void LogInUser(UserData usr)
        {
            LogOutUser(usr.samaccountname);
            Users.Add(usr);
        }

    }
    public class UserData
    {
        public UserData()
        {
            DepartmentSigurIDs = new List<int>();
            role = null;
            samaccountname = null;
            FIO = null;
        }
        public string samaccountname { get; set; }
        public string role { get; set; }
        public string department { get; set; }
        public string FIO { get; set; }


        public List<int> DepartmentSigurIDs { get; set; }


    }
}

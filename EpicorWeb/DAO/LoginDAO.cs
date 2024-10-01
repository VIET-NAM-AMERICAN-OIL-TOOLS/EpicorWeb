using System.Data;

namespace EpicorWeb.DAO
{
    public class LoginDAO
    {
        public int checkUsername(string username)
        {
            int rowcount = 0;
            string queryStr = "select UserID from Ice.SysUserFile where UserID='" + username + "' and UserDisabled = 'false' ";
            DataProvider data = new DataProvider();
            DataTable dataTable = data.ExecuteQuery(queryStr);
            rowcount = dataTable.Rows.Count;
            return rowcount;
        }

        public string getPassFromUserUsername(string username)
        {
            string pass = "";
            string queryStr = "select Password from Ice.SysUserFile where UserID='" + username + "'";
            DataProvider data = new DataProvider();
            DataTable dataTable = data.ExecuteQuery(queryStr);
            pass = dataTable.Rows[0]["Password"].ToString();
            return pass;
        }
    }
}

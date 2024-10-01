using System.Data;
using EpicorWeb.DAO;
using EpicorWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;

namespace EpicorWeb.AllFunction
{
    public class CategoryAdmin
    {
        public bool HasChildren(int Id)
        {
            // Tìm tất cả các hàng trong DataTable có ParentID bằng ID của parentRow
            string query = "exec SP_HasChildren @id";
            return new DataProviderLocal().ExecuteQuery(query, new object[] {Id}).Rows.Count > 0;
        }

        public DataTable GetChildren(int Id)
        {
            string query = "exec SP_HasChildren @id";
            return new DataProviderLocal().ExecuteQuery(query, new object[] { Id });
        }

        public bool HasChildrenWithUser(int Id, string UserID)
        {
            // Tìm tất cả các hàng trong DataTable có ParentID bằng ID của parentRow
            string query = "Exec [dbo].[SP_HasChildrenWithUser] @id , @UserId";
            return new DataProviderLocal().ExecuteQuery(query, new object[] { Id , UserID }).Rows.Count > 0;
        }

        public DataTable GetChildrenWithUser(int Id, string UserID)
        {
            string query = "Exec [dbo].[SP_HasChildrenWithUser] @id , @UserId";
            return new DataProviderLocal().ExecuteQuery(query, new object[] { Id , UserID });
        }

    }
}

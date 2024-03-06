using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AccuFintech.DataAccess
{
    public class MenuDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public List<MenuModel> get_Menu()
        {
            List<MenuModel> MenuL = new List<MenuModel>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"Select M.MenuID,MenuName,uppercode,ControllerName,ActionName,Icon from TBL_Menu M
                                                            LEFT JOIN WEB_MenuPermission W ON M.MenuID = W.MenuID", con);
                DataTable tbl = new DataTable();
                da.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        MenuModel M = new MenuModel();
                        M.MenuID = Convert.ToInt64(dr["MenuID"].ToString());
                        M.MenuName = dr["MenuName"].ToString();
                        M.MenuParentID = Convert.ToInt64(dr["uppercode"].ToString());
                        M.ControllerName = dr["ControllerName"].ToString();
                        M.ActionName = dr["ActionName"].ToString();
                        M.Icon = dr["Icon"].ToString();
                        MenuL.Add(M);
                    }
                }
                return MenuL;
            }
        }

        public List<MenuModel> get_Menu(string UserType)
        {
            List<MenuModel> MenuL = new List<MenuModel>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(@"Select M.MenuID,M.MenuName,M.uppercode,M.ControllerName,M.Icon,M.ActionName
                                                            from TBL_Menu M, WEB_MenuPermission P where M.menuid = p.menuid and
                                                            p.usertype = @usertype Order by MenuID", con);
                da.SelectCommand.Parameters.AddWithValue("@usertype", UserType);
                DataTable tbl = new DataTable();
                da.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        MenuModel M = new MenuModel();
                        M.MenuID = Convert.ToInt64(dr["MenuID"].ToString());
                        M.MenuName = dr["MenuName"].ToString();
                        M.MenuParentID = Convert.ToInt64(dr["uppercode"].ToString());
                        M.ControllerName = dr["ControllerName"].ToString();
                        M.ActionName = dr["ActionName"].ToString();
                        M.Icon = dr["Icon"].ToString();
                        MenuL.Add(M);
                    }
                }
                return MenuL;
            }
        }
    }
}
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
    public class MenuPermissionDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public List<DropdownModel> LoadUserType()
        {
            List<DropdownModel> dm = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select UserType from USER_TypeMaster", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            DropdownModel dl = new DropdownModel();
                            dl.Key = dr["UserType"].ToString();
                            dl.Value = dr["UserType"].ToString();
                            dm.Add(dl);
                        }
                        return dm;
                    }
                    else
                    {
                        return new List<DropdownModel>();
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<DropdownModel>();
            }
        }

        public List<MenuPermission> LoadAllMenu(string userType)
        {
            List<MenuPermission> mp = new List<MenuPermission>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("USP_LoadAllMenuForUserType", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@UserType", userType);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            MenuPermission me = new MenuPermission();
                            me.UserType = userType;
                            me.MenuID = Convert.ToInt32(dr["MenuID"]);
                            me.MenuName = dr["MenuName"].ToString();
                            me.MenuParentID = Convert.ToInt32(dr["UpperCode"]);
                            me.IsAsign = dr["IsAsign"].ToString() == "0" ? false : true;
                            mp.Add(me);
                        }
                        return mp;
                    }
                    else
                    {
                        return new List<MenuPermission>();
                    }

                }
            }
            catch (Exception ex)
            {

                return new List<MenuPermission>();
            }
        }

        public Boolean InsertMenus(UserMenu mnu, string UserType)
        {
            string menulist = "";
            foreach (var item in mnu.menuPermission)
            {
                if (item.IsAsign)
                {
                    menulist = menulist + item.MenuID.ToString() + ";";
                }

            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertmenuPermission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    cmd.Parameters.AddWithValue("@UTypeAccessList", menulist);
                    cmd.Parameters.AddWithValue("@DELM", ";");
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
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
    public class UserTypeDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public UserTypeDal()
        {
            global = new GlobalFunction();
        }

        public List<UserTypeMaster> GetAllUserType()
        {
            List<UserTypeMaster> utm = new List<UserTypeMaster>();
            try
            {
                using (System.Data.SqlClient.SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select UserType,Designation,IsBackDate,Extention,IsUpdate,IsDelete,AllFranchaise,RankUpTo,AllPlans,IsReprint,AllReportFranchaise from USER_TypeMaster", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            UserTypeMaster um = new UserTypeMaster();
                            um.UserType = dr["UserType"].ToString();
                            um.Designation = dr["Designation"].ToString();
                            um.IsBackDate = Convert.ToBoolean(dr["IsBackDate"].ToString() == "1" ? true : false);
                            um.IsUpdate = Convert.ToBoolean(dr["IsUpdate"].ToString() == "1" ? true : false);
                            um.IsDelete = Convert.ToBoolean(dr["IsDelete"].ToString() == "1" ? true : false);
                            utm.Add(um);
                        }
                        return utm;

                    }
                    else
                    {
                        return new List<UserTypeMaster>();
                    }
                }
            }
            catch (Exception ex)
            {
                return new List<UserTypeMaster>();
            }
        }

        public Boolean InsertUserType(UserTypeMaster Usr, string EntryType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertUserType", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InsertOrUpdate", EntryType);
                    cmd.Parameters.AddWithValue("@UserType", Usr.UserType);
                    cmd.Parameters.AddWithValue("@Designation", Usr.Designation);
                    cmd.Parameters.AddWithValue("@IsBackDate", Usr.IsBackDate ? "1" : "0");
                    cmd.Parameters.AddWithValue("@Extention", "");
                    cmd.Parameters.AddWithValue("@IsUpdate", Usr.IsUpdate ? "1" : "0");
                    cmd.Parameters.AddWithValue("@IsDelete", Usr.IsDelete ? "1" : "0");
                    cmd.Parameters.AddWithValue("@AllFranchaise", "0");
                    cmd.Parameters.AddWithValue("@AllReportFranchaise", "0");
                    cmd.Parameters.AddWithValue("@RankUpTo", 99);
                    cmd.Parameters.AddWithValue("@AllPlans", "0");
                    cmd.Parameters.AddWithValue("@IsReprint", "0");
                    cmd.Parameters.AddWithValue("@PlanTableList", "");
                    cmd.Parameters.AddWithValue("@DELM", ";");
                    cmd.Parameters.AddWithValue("@USERNAME", HttpContext.Current.Session["_UserId"].ToString());
                    cmd.Parameters.AddWithValue("@UDATE", global.ConvertStringToInt(global.CurrentDate()));
                    cmd.Parameters.AddWithValue("@UTIME", global.CurrentTime());
                    cmd.Parameters.AddWithValue("@USERBCODE", "");
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

        public UserTypeMaster SelectUserType(string userType)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("Select UserType,Designation,IsBackDate,Extention,IsUpdate,IsDelete,AllFranchaise,RankUpTo,AllPlans,IsReprint,AllReportFranchaise from USER_TypeMaster where UserType=@UserType", con);
                    da.SelectCommand.Parameters.AddWithValue("@UserType", userType);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        UserTypeMaster um = new UserTypeMaster();
                        um.UserType = dt.Rows[0]["UserType"].ToString();
                        um.Designation = dt.Rows[0]["Designation"].ToString();
                        um.IsBackDate = Convert.ToBoolean(dt.Rows[0]["IsBackDate"].ToString() == "1" ? true : false);
                        um.IsUpdate = Convert.ToBoolean(dt.Rows[0]["IsUpdate"].ToString() == "1" ? true : false);
                        um.IsDelete = Convert.ToBoolean(dt.Rows[0]["IsDelete"].ToString() == "1" ? true : false);
                        return um;
                    }
                    else
                    {
                        return new UserTypeMaster();
                    }
                }
            }
            catch (Exception ex)
            {
                return new UserTypeMaster();
            }
        }
    }
}
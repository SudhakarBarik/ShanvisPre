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
    public class UserMasterDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public bool UserEntry(UserMasterModel UM)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand UserInsert = new SqlCommand(@"INSERT INTO [USER_Master]([UserID],[Password],[UserType],[FullName],[Mobile],[LockMode])
                                                    VALUES(@UserID, @Password, @Usertype, @Fullname, @Mobile, @LockMode)", con);
                    UserInsert.Parameters.AddWithValue("@UserID", UM.UserID);
                    UserInsert.Parameters.AddWithValue("@Password", UM.Password);
                    UserInsert.Parameters.AddWithValue("@Usertype", UM.UserType);
                    UserInsert.Parameters.AddWithValue("@Fullname", UM.Fullname);
                    UserInsert.Parameters.AddWithValue("@Mobile", UM.Phone);
                    UserInsert.Parameters.AddWithValue("@LockMode", UM.LockMode == true ? 1 : 0);

                    int status = UserInsert.ExecuteNonQuery();

                    if (status > 0)
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

        public bool UpdateUserEntry(UserMasterModel UM)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand UserUpdate = new SqlCommand(@"UPDATE USER_Master SET UserType = @UserType,FullName = @Fullname,
                                                            Password = @Password,Mobile = @Mobile,LockMode = @LockMode WHERE UserID = @UserID", con);
                    UserUpdate.Parameters.AddWithValue("@UserID", UM.UserID);
                    UserUpdate.Parameters.AddWithValue("@Password", UM.Password);
                    UserUpdate.Parameters.AddWithValue("@Usertype", UM.UserType);
                    UserUpdate.Parameters.AddWithValue("@Fullname", UM.Fullname);
                    UserUpdate.Parameters.AddWithValue("@Mobile", UM.Phone);
                    UserUpdate.Parameters.AddWithValue("@LockMode", UM.LockMode == true ? 1 : 0);

                    int status = UserUpdate.ExecuteNonQuery();

                    if (status > 0)
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

        public List<UserMasterModel> GetAllUser()
        {
            List<UserMasterModel> UserList = new List<UserMasterModel>();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT UserID, UserType, FullName, Mobile, LockMode FROM USER_Master WHERE UserID NOT IN ('Codeenv')", con);
                DataTable tbl = new DataTable();
                da.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        UserMasterModel UM = new UserMasterModel();
                        UM.UserID = dr["UserID"].ToString();
                        UM.UserType = dr["UserType"].ToString();
                        UM.Fullname = dr["FullName"].ToString();
                        UM.Phone = dr["Mobile"].ToString();
                        UM.LockModeStatus = dr["LockMode"].ToString() == "True" ? "Active" : "Inactive";
                        UserList.Add(UM);
                    }
                    return UserList;
                }
                else
                {
                    return UserList;
                }
            }
        }

        public UserMasterModel GetAllUserByID(string UserID)
        {
            UserMasterModel UM = new UserMasterModel();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT UserID, Password, UserType, FullName, Mobile, LockMode FROM USER_Master WHERE UserID = @UserID", con);
                da.SelectCommand.Parameters.AddWithValue("@UserID", UserID);
                DataTable tbl = new DataTable();
                da.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    UM.UserID = tbl.Rows[0]["UserID"].ToString();
                    UM.Password = tbl.Rows[0]["Password"].ToString();
                    UM.UserType = tbl.Rows[0]["UserType"].ToString();
                    UM.Fullname = tbl.Rows[0]["FullName"].ToString();
                    UM.Phone = tbl.Rows[0]["Mobile"].ToString();
                    UM.LockMode = tbl.Rows[0]["LockMode"].ToString() == "True" ? true : false;
                    return UM;
                }
                else
                {
                    return UM;
                }
            }
        }

        public bool DeleteUser(string UserID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand UserDelete = new SqlCommand("DELETE FROM USER_Master WHERE UserID = @UserID", con);
                    UserDelete.Parameters.AddWithValue("@UserID", UserID);

                    int status = UserDelete.ExecuteNonQuery();

                    if (status > 0)
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
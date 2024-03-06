using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using AccuFintech.Models;

namespace AccuFintech.DataAccess
{
    public class AccountDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public AccountModel Login(string UserID, string password)
        {
            AccountModel usr = new AccountModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("WEB_LoginUserAdmin", con);
                    da.SelectCommand.Parameters.AddWithValue("@Userid", UserID);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        if (String.Equals(tbl.Rows[0]["Password"].ToString(), password))
                        {
                            usr.UserID = tbl.Rows[0]["UserID"].ToString();
                            usr.UserType = tbl.Rows[0]["UserType"].ToString();
                        }
                    };
                    return usr;
                }
            }
            catch (Exception ex)
            {
                return usr;
            }
        }

        public StudentLoginModel StudentLogin(string StudentID, string Password)
        {
            StudentLoginModel usr = new StudentLoginModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("WEB_LoginStudent", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        if (String.Equals(tbl.Rows[0]["Password"].ToString(), Password))
                        {
                            usr.StudentID = tbl.Rows[0]["StudentID"].ToString();
                            usr.UserType = tbl.Rows[0]["UserType"].ToString();
                        }
                    };
                    return usr;
                }
            }
            catch (Exception ex)
            {
                return usr;
            }
        }
    }
}
using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Windows;

namespace AccuFintech.DataAccess
{
    public class GlobalFunction
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

        public List<DropdownModel> LoadUserType()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                List<DropdownModel> dl = new List<DropdownModel>();
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT UserType FROM USER_TypeMaster WHERE UserType != 'Employee'", con);
                DataTable tbl = new DataTable();
                da.Fill(tbl);
                if (tbl.Rows.Count > 0)
                {
                    foreach (DataRow dr in tbl.Rows)
                    {
                        DropdownModel s = new DropdownModel()
                        {
                            Key = dr["UserType"].ToString(),
                            Value = dr["UserType"].ToString(),
                        };
                        dl.Add(s);
                    }
                    return dl;
                }
                else
                {
                    return new List<DropdownModel>();
                }
            }
        }


        public T NullFilter<T>(T obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {

                if (property.PropertyType == typeof(string))
                {
                    string pName = property.Name;
                    var pvalue = property.GetValue(obj, null);
                    if (pvalue == null)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(string.Empty, propertyInfo.PropertyType), null);
                    }
                }
                if (property.PropertyType == typeof(Int16) || property.PropertyType == typeof(Int32) || property.PropertyType == typeof(Int64) || property.PropertyType == typeof(int) || property.PropertyType == typeof(Double) || property.PropertyType == typeof(Decimal) || property.PropertyType == typeof(Byte))
                {
                    string pName = property.Name;
                    var pvalue = property.GetValue(obj, null);
                    if (pvalue == null)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(0, propertyInfo.PropertyType), null);
                    }
                }
                if (property.PropertyType == typeof(Boolean))
                {
                    string pName = property.Name;
                    var pvalue = property.GetValue(obj, null);
                    if (pvalue == null)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(false, propertyInfo.PropertyType), null);
                    }
                }
                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(Int32) || property.PropertyType == typeof(Int64) || property.PropertyType == typeof(int) || property.PropertyType == typeof(Double) || property.PropertyType == typeof(Decimal) || property.PropertyType == typeof(Byte))
                {
                    string pName = property.Name;
                    var pvalue = property.GetValue(obj, null);
                    if (pvalue == null)
                    {
                        PropertyInfo propertyInfo = type.GetProperty(property.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(DateTime.Now, propertyInfo.PropertyType), null);
                    }
                }
            }
            return obj;
        }

        public int ConvertStringToInt(string dts)
        {
            int dtint = 0;
            if (dts.Contains("-"))
            {
                DateTime dt = DateTime.ParseExact(dts, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                dtint = Convert.ToInt32(dt.Date.ToString("yyyyMMdd"));
            }
            else if (dts.Contains("/"))
            {
                string[] formats = { "yyyy/MM/dd", "dd/MM/yyyy" };
                DateTime parsedDate;
                var isValidFormat = DateTime.TryParseExact(dts, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                if (isValidFormat)
                {
                    dtint = Convert.ToInt32(parsedDate.Date.ToString("yyyyMMdd"));
                }
                //DateTime dt = DateTime.ParseExact(dts, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //dtint = Convert.ToInt32(dt.Date.ToString("yyyyMMdd"));
            }
            return dtint;
        }

        public string ConvertTimetoInt(string T)
        {
            string[] TimeList = T.Split(':');
            string timeint = TimeList[0].ToString().PadLeft(2, '0') + "" + TimeList[1].ToString().PadLeft(2, '0');
            return timeint;
        }

        public int ConvertDateToInt(DateTime dts)
        {
            int dtint = 0;
            dtint = Convert.ToInt32(dts.Date.ToString("yyyyMMdd"));
            return dtint;
        }
        public string ConvertIntToString(int dts)
        {
            string dtint;
            DateTime dt = DateTime.ParseExact(Convert.ToString(dts), "yyyyMMdd", CultureInfo.InvariantCulture);
            dtint = dt.Date.ToString("dd/MM/yyyy");
            return dtint;
        }

        public string ConvertDateFormat(int dts)
        {
            string dtint;
            DateTime dt = DateTime.ParseExact(Convert.ToString(dts), "yyyyMMdd", CultureInfo.InvariantCulture);
            dtint = dt.Date.ToString("dd MMMM, yyyy");
            return dtint;
        }

        public string ConvertIntToTime(string T)
        {
            string Hour = T.Substring(0, 2);
            string Minute = T.Substring(2, 2);
            string Time = Hour + ":" + Minute;
            return Time;
        }

        public string CurrentDate()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select convert(varchar,getdate(),103)", con);
                string serverDate = cmd.ExecuteScalar().ToString();
                return serverDate;
            }
        }

        public string CurrentTime()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select convert(nvarchar(50),getdate(),108)", con);
                string serverDate = cmd.ExecuteScalar().ToString();
                return serverDate;
            }
        }

        public List<DropdownModel> GetCourses()
        {
            List<DropdownModel> CourseList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT ProgramCode, Coursename FROM TBL_CourseMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["ProgramCode"].ToString();
                            DM.Value = dr["Coursename"].ToString();
                            CourseList.Add(DM);
                        }
                    }
                }
                return CourseList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CourseList;
            }
        }
        public Boolean FinyearValidation(int EDate)
        {
            int FStartDate = Convert.ToInt32(HttpContext.Current.Session["_FinStartDate"].ToString());
            int FEndDate = Convert.ToInt32(HttpContext.Current.Session["_FinEndDate"].ToString());
            if (EDate >= FStartDate && EDate <= FEndDate) { return true; } else { return false; }
        }
        public List<DropdownModel> GetAllFranchaiseList()
        {
            List<DropdownModel> FranchaiseList = new List<DropdownModel>();
            string UserType = HttpContext.Current.Session["_UserType"].ToString();
            string UserID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT FranchaiseCode, Name FROM tbl_franchaisemaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        DropdownModel dm = new DropdownModel() { Key = "All", Value = "All" };
                        FranchaiseList.Insert(0, dm);
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["FranchaiseCode"].ToString();
                            DM.Value = dr["Name"].ToString();
                            FranchaiseList.Add(DM);
                        }
                    }
                }
                return FranchaiseList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return FranchaiseList;
            }
        }

        public List<DropdownModel> GetFranchaiseList()
        {
            List<DropdownModel> FranchaiseList = new List<DropdownModel>();
            string UserType = HttpContext.Current.Session["_UserType"].ToString();
            string UserID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da = new SqlDataAdapter("SELECT FranchaiseCode, Name FROM tbl_franchaisemaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["FranchaiseCode"].ToString();
                            DM.Value = dr["Name"].ToString();
                            FranchaiseList.Add(DM);
                        }
                    }
                }
                return FranchaiseList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return FranchaiseList;
            }
        }

        public List<DropdownModel> GetStudentList()
        {
            List<DropdownModel> StudentList = new List<DropdownModel>();
            string UserType = HttpContext.Current.Session["_UserType"].ToString();
            string UserID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    if (UserType.ToUpper() == "ADMINISTRATOR")
                    {
                        da = new SqlDataAdapter("SELECT StudentID, Studentname FROM TBL_Student", con);
                    }
                    else
                    {
                        da = new SqlDataAdapter("SELECT StudentID, Studentname FROM TBL_Student WHERE Franchaise = @Franchaise", con);
                        da.SelectCommand.Parameters.AddWithValue("@Franchaise", UserID);
                    }
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["StudentID"].ToString();
                            DM.Value = dr["Studentname"].ToString() + "-" + dr["StudentID"].ToString();
                            StudentList.Add(DM);
                        }
                    }
                }
                return StudentList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return StudentList;
            }
        }

        public List<DropdownModel> GetAllState()
        {
            List<DropdownModel> StateList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT state_id, state_name FROM TBL_State WHERE status = 'Active'", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["state_id"].ToString();
                            DM.Value = dr["state_name"].ToString();
                            StateList.Add(DM);
                        }
                    }
                }
                return StateList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return StateList;
            }
        }

        public List<DropdownModel> GetAllDistrict(string StateID)
        {
            List<DropdownModel> DistrictList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT DistrictID, district_name FROM TBL_District WHERE State_id = @StateID AND district_status = 'Active'", con);
                    da.SelectCommand.Parameters.AddWithValue("@StateID", StateID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["DistrictID"].ToString();
                            DM.Value = dr["district_name"].ToString();
                            DistrictList.Add(DM);
                        }
                    }
                }
                return DistrictList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return DistrictList;
            }
        }

        public List<DropdownModel> GetAllCourses()
        {
            List<DropdownModel> CourseList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT ProgramCode, Coursename FROM TBL_CourseMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["ProgramCode"].ToString();
                            DM.Value = dr["Coursename"].ToString();
                            CourseList.Add(DM);
                        }
                    }
                    return CourseList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CourseList;
            }
        }

        public List<DropdownModel> GetAllSessions()
        {
            List<DropdownModel> SessionsList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT SessionName , StartDate , EndDate FROM TBL_SessionMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        DropdownModel dm = new DropdownModel() { Key = "All", Value = "All" };
                        SessionsList.Insert(0, dm);
                        foreach (DataRow dr in tbl.Rows)
                        {
                            string SDate = ConvertIntToString(Convert.ToInt32(dr["StartDate"].ToString()));
                            string EDate = ConvertIntToString(Convert.ToInt32(dr["EndDate"].ToString()));
                            string val = dr["SessionName"].ToString() + "(" + SDate + " - " + EDate + ")";
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["SessionName"].ToString();
                            DM.Value = val;
                            SessionsList.Add(DM);
                        }
                    }
                    return SessionsList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return SessionsList;
            }
        }

        public List<DropdownModel> GetSessions()
        {
            List<DropdownModel> SessionsList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT SessionName , StartDate , EndDate FROM TBL_SessionMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                     
                        foreach (DataRow dr in tbl.Rows)
                        {
                            string SDate = ConvertIntToString(Convert.ToInt32(dr["StartDate"].ToString()));
                            string EDate = ConvertIntToString(Convert.ToInt32(dr["EndDate"].ToString()));
                            string val = dr["SessionName"].ToString() + "(" + SDate + " - " + EDate + ")";
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["SessionName"].ToString();
                            DM.Value = val;
                            SessionsList.Add(DM);
                        }
                    }
                    return SessionsList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return SessionsList;
            }
        }


        public List<DropdownModel> GetAllBatches()
        {
            List<DropdownModel> CourseList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("select BatchID , BatchName from TBL_BatchMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["BatchID"].ToString();
                            DM.Value = dr["BatchName"].ToString();
                            CourseList.Add(DM);
                        }
                    }
                    return CourseList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CourseList;
            }
        }

        public List<string> IsDateOverLap(string FromDate, string Todate)
        {
            //Return List
            List<string> Season = new List<string>();
            string fromDate;
            string todate;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("USP_CheckIsDateOverLap", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@FromDate", ConvertStringToInt(FromDate));
                    da.SelectCommand.Parameters.AddWithValue("@ToDate", ConvertStringToInt(Todate));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count < 0)
                    {
                        return new List<string>();
                    }
                    else if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in tbl.Rows)
                        {
                            fromDate = Dr["StartDate"].ToString();
                            todate = Dr["EndDate"].ToString();
                            Season.Add(fromDate);
                            Season.Add(todate);
                        }
                    };
                    return Season;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<string>();
            }
        }



    }
}

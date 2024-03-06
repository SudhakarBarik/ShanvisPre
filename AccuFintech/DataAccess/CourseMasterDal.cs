using AccuFintech.Models;
using AccuFintech.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AccuFintech.DataAccess
{
    public class CourseMasterDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public CourseMasterDal()
        {
            global = new GlobalFunction();
        }
        public Dictionary<string, string> AddOrUpdateCourse(CourseMasterModel CM)
        {
            Dictionary<string, string> ResultDict = new Dictionary<string, string>();
            
            try
            {
                CM = global.NullFilter<CourseMasterModel>(CM);     
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_AddOrUpdateCourse", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Programcode", CM.Programcode);
                    cmd.Parameters.AddWithValue("@Programname", CM.Coursename);
                    cmd.Parameters.AddWithValue("@MODULE", CM.CourseModule);
                    cmd.Parameters.AddWithValue("@MonthDuration", CM.MonthDuration);
                    cmd.Parameters.AddWithValue("@HourDuration", CM.HourDuration);
                    cmd.Parameters.AddWithValue("@Eligibility", CM.Eligibility);
                    cmd.Parameters.AddWithValue("@CareerOpportunities", CM.CareerOportunities);
                    cmd.Parameters.AddWithValue("@Fees", CM.Fees);
                    cmd.Parameters.AddWithValue("@Opsection", CM.opsection);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);

                    if (ERRORCODE == 0)
                    {
                        ResultDict.Add("status", "0");
                        ResultDict.Add("msg", "Course details saved successfully");
                    }
                    else
                    {
                        ResultDict.Add("status", "1");
                        ResultDict.Add("msg", "Failed to save course details!!");
                    }                    
                }
                return ResultDict;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                ResultDict.Add("status", "1");
                ResultDict.Add("msg", "Failed to save course details!!");
                return ResultDict;
            }
        }

        public List<CourseMasterModel> GetAllCourses()
        {
            List<CourseMasterModel> Courses = new List<CourseMasterModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT Programcode, CourseName, MonthDuration, HourDuration, Eligibility, Fees FROM TBL_CourseMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            CourseMasterModel CM = new CourseMasterModel();
                            CM.Programcode = dr["Programcode"].ToString();
                            CM.Coursename = dr["CourseName"].ToString();
                            CM.MonthDuration = dr["MonthDuration"].ToString();
                            CM.HourDuration = dr["HourDuration"].ToString();
                            CM.Eligibility = dr["Eligibility"].ToString();
                            CM.Fees = dr["Fees"].ToString();
                            Courses.Add(CM);
                        }
                    }
                }
                return Courses;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return Courses;
            }
        }

        public CourseMasterModel GetCourseByID(string Programcode)
        {
            CourseMasterModel CM = new CourseMasterModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT Programcode, CourseName, CourseModule, MonthDuration, 
                                                                HourDuration, Eligibility, CareerOpportunities , Fees FROM TBL_CourseMaster
                                                                WHERE ProgramCode = @Programcode", con);
                    da.SelectCommand.Parameters.AddWithValue("@Programcode", Programcode);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        CM.Programcode = tbl.Rows[0]["Programcode"].ToString();
                        CM.Coursename = tbl.Rows[0]["CourseName"].ToString();
                        CM.CourseModule = tbl.Rows[0]["CourseModule"].ToString();
                        CM.MonthDuration = tbl.Rows[0]["MonthDuration"].ToString();
                        CM.HourDuration = tbl.Rows[0]["HourDuration"].ToString();
                        CM.Eligibility= tbl.Rows[0]["Eligibility"].ToString();
                        CM.CareerOportunities = tbl.Rows[0]["CareerOpportunities"].ToString();
                        CM.Fees = tbl.Rows[0]["Fees"].ToString();
                    }
                }
                return CM;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CM;
            }
        }

        public Dictionary<string, string> RemoveCourse(string Programcode)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM TBL_CourseMaster WHERE [ProgramCode] = @Programcode", con);
                    cmd.Parameters.AddWithValue("@Programcode", Programcode);
                    int Status = cmd.ExecuteNonQuery();
                    if (Status > 0)
                    {
                        Result.Add("status", "0");
                        Result.Add("msg", "Course details removed successfully");
                    }
                    else
                    {
                        Result.Add("status", "1");
                        Result.Add("msg", "Failed to remove Course details");
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to remove Course");
                return Result;
            }
        }
    }
}
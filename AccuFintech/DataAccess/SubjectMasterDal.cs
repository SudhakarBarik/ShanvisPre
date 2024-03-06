using AccuFintech.Models;
using Newtonsoft.Json;
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
    public class SubjectMasterDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public SubjectMasterDal()
        {
            global = new GlobalFunction();
        }

        public Dictionary<string, string> AddOrUpdateSubject(SubjectModel SM)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            List<SubjectModel> SubjectList = new List<SubjectModel>();
            bool flag = true;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand CmdSub = new SqlCommand("SELECT COUNT(*) FROM TBL_SubjectMaster WHERE CourseID = @CourseID", con);
                    CmdSub.Parameters.AddWithValue("@CourseID", SM.CourseID);
                    int SubCount = Convert.ToInt32(CmdSub.ExecuteScalar());
                    if (SubCount > 0)
                    {
                        SqlCommand cmdDelete = new SqlCommand("DELETE FROM TBL_SubjectMaster WHERE CourseID = @CourseID", con);
                        cmdDelete.Parameters.AddWithValue("@CourseID", SM.CourseID);
                        int DeleteStatus = cmdDelete.ExecuteNonQuery();
                        if (DeleteStatus > 0)
                        {
                            flag = true;
                        }
                        else
                        {
                            flag = false;
                        }
                    }
                }

                if (flag == true)
                {
                    SubjectList = JsonConvert.DeserializeObject<List<SubjectModel>>(SM.SubjectString);
                    foreach (var item in SubjectList)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            if (con.State == ConnectionState.Open) { con.Close(); }
                            con.Open();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO [TBL_SubjectMaster]([SubjectID],[CourseID],[Subject],[FullMarks],[PassMarks])
                                                            VALUES(LEFT(NEWID(),8),@CourseID, @Subject, @FullMarks, @PassMarks)", con);
                            cmd.Parameters.AddWithValue("@CourseID", item.CourseID);
                            cmd.Parameters.AddWithValue("@Subject", item.Subject);
                            cmd.Parameters.AddWithValue("@FullMarks", item.FullMarks);
                            cmd.Parameters.AddWithValue("@PassMarks", item.PassMarks);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    Result.Add("status", "0");
                    Result.Add("msg", "Subject details saved successfully");
                }
                else
                {
                    Result.Add("status", "1");
                    Result.Add("msg", "Failed to save Subject");
                }
               
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to save Subject");
                return Result;
            }
        }

        public List<SubjectModel> GetSubjectList(string CourseID)
        {
            List<SubjectModel> SubjectList = new List<SubjectModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT S.CourseID, C.Coursename, S.Subject, S.FullMarks, S.PassMarks FROM TBL_SubjectMaster S
                                                                INNER JOIN TBL_CourseMaster C ON C.ProgramCode = S.CourseID
                                                                WHERE S.CourseID = @CourseID", con);
                    da.SelectCommand.Parameters.AddWithValue("@CourseID", CourseID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            SubjectModel SM = new SubjectModel();
                            SM.CourseID = dr["CourseID"].ToString();
                            SM.Course = dr["Coursename"].ToString();
                            SM.Subject = dr["Subject"].ToString();
                            SM.FullMarks = dr["FullMarks"].ToString();
                            SM.PassMarks = dr["PassMarks"].ToString();
                            SubjectList.Add(SM);
                        }
                    }
                }
                return SubjectList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return SubjectList;
            }
        }
    }
}
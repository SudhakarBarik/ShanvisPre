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
    public class MarksEntryDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public MarksEntryDal()
        {
            global = new GlobalFunction();
        }
        public MarksEntryModel GetMarksEntryDetails(string StudentID)
        {
            MarksEntryModel MEModel = new MarksEntryModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT S.StudentID, S.Studentname, S.Gurdian, S.JoiningDate, Course,
                                                                    S.Examregdate, S.Examdate, F.FranchaiseID, F.Center, C.Coursename FROM TBL_Student S
                                                                    INNER JOIN TBL_CourseMaster C ON S.Course = C.ProgramCode
                                                                    INNER JOIN TBL_Franchaise F ON S.Franchaise = F.FranchaiseID
                                                                    WHERE StudentID = @StudentID", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        MEModel.StudentID = tbl.Rows[0]["StudentID"].ToString();
                        MEModel.StudentIDView = tbl.Rows[0]["StudentID"].ToString();
                        MEModel.Studentname = tbl.Rows[0]["Studentname"].ToString();
                        MEModel.Gurdianname = tbl.Rows[0]["Gurdian"].ToString();
                        MEModel.DOJ = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["JoiningDate"]));
                        MEModel.ExamRegdate = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["Examregdate"]));
                        MEModel.Examdate = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["Examdate"]));
                        MEModel.Franchaisecode = tbl.Rows[0]["FranchaiseID"].ToString();
                        MEModel.Franchaise = tbl.Rows[0]["Center"].ToString();
                        MEModel.Course = tbl.Rows[0]["Coursename"].ToString();
                        MEModel.CourseID = tbl.Rows[0]["Course"].ToString();
                    }
                }
                return MEModel;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return MEModel;
            }
        }

        public List<SubjectMarksModel> GetAllSubjectCourseWise(string CourseID, string StudentID)
        {
            List<SubjectMarksModel> SubjectList = new List<SubjectMarksModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT S.SubjectID, S.Subject, S.FullMarks, S.PassMarks, SM.Theory, SM.Practical, SM.MarksObtained  
                                                                FROM TBL_SubjectMaster S LEFT JOIN 
                                                                (SELECT StudentID, CourseID, SubjectID, Theory, Practical, MarksObtained 
                                                                FROM TBL_StudentMarks WHERE CourseID = @CourseID AND StudentID = @StudentID) AS SM ON SM.CourseID = S.CourseID AND S.SubjectID = SM.SubjectID
                                                                WHERE S.CourseID = @CourseID", con);
                    da.SelectCommand.Parameters.AddWithValue("@CourseID", CourseID);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            SubjectMarksModel SM = new SubjectMarksModel();
                            SM.SubjectID = dr["SubjectID"].ToString();
                            SM.Subject = dr["Subject"].ToString();
                            SM.FullMarks = dr["FullMarks"].ToString();
                            SM.PassMarks = dr["PassMarks"].ToString();
                            SM.Theory = dr["Theory"].ToString();
                            SM.Practical = dr["Practical"].ToString();
                            SM.MarksObtained = dr["MarksObtained"].ToString();
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

        public Dictionary<string, string> SaveStudentMarks(MarksEntryModel MEModel)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            List<SubjectMarksModel> SubjectWiseMarksList = new List<SubjectMarksModel>();
            var flag = true;
            try
            {
                 using (SqlConnection con = new SqlConnection(ConnectionString))
                 {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmdMarks = new SqlCommand("SELECT COUNT(*) FROM TBL_StudentMarks WHERE StudentID = @StudentID And CourseID = @CourseID", con);
                    cmdMarks.Parameters.AddWithValue("@StudentID", MEModel.StudentID);
                    cmdMarks.Parameters.AddWithValue("@CourseID", MEModel.CourseID);
                    int IsexistCount = Convert.ToInt32(cmdMarks.ExecuteScalar());
                    if (IsexistCount > 0)
                    {
                        SqlCommand DeleteCmd = new SqlCommand("DELETE FROM TBL_StudentMarks WHERE StudentID = @StudentID And CourseID = @CourseID", con);
                        DeleteCmd.Parameters.AddWithValue("@StudentID", MEModel.StudentID);
                        DeleteCmd.Parameters.AddWithValue("@CourseID", MEModel.CourseID);
                        int Status = DeleteCmd.ExecuteNonQuery();
                        if (Status > 0)
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
                    SubjectWiseMarksList = JsonConvert.DeserializeObject<List<SubjectMarksModel>>(MEModel.StudentMarksJson);
                    foreach (var item in SubjectWiseMarksList)
                    {
                        using (SqlConnection con = new SqlConnection(ConnectionString))
                        {
                            if (con.State == ConnectionState.Open) { con.Close(); }
                            con.Open();

                            SqlCommand cmd = new SqlCommand(@"INSERT INTO [TBL_StudentMarks]([StudentID],[CourseID],[SubjectID],[FullMarks],[PassMarks],[Theory],[Practical],[MarksObtained])
                                                        VALUES(@StudentID, @CourseID, @SubjectID, @FullMarks, @PassMarks, @Theory, @Practical, @MarksObtained)", con);
                            cmd.Parameters.AddWithValue("@StudentID", item.StudentID);
                            cmd.Parameters.AddWithValue("@CourseID", item.CourseID);
                            cmd.Parameters.AddWithValue("@SubjectID", item.SubjectID);
                            cmd.Parameters.AddWithValue("@FullMarks", item.FullMarks);
                            cmd.Parameters.AddWithValue("@PassMarks", item.PassMarks);
                            cmd.Parameters.AddWithValue("@Theory", item.Theory);
                            cmd.Parameters.AddWithValue("@Practical", item.Practical);
                            cmd.Parameters.AddWithValue("@MarksObtained", item.MarksObtained);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                
                Result.Add("status", "0");
                Result.Add("msg", "Student Marks saved successfully");
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to save Student Marks");
                return Result;
            }
        }
    }
}
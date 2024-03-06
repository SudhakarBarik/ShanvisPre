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
    public class OnlineExamDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public OnlineExamDal()
        {
            global = new GlobalFunction();
        }
        public StudentDetails GetStudentDetails()
        {
            StudentDetails SD = new StudentDetails();
            string StudentID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"select StudentID, Studentname, 
                                                                (select ConfigValue from TBL_Configuration WHERE ConfigField = 'Question Set') AS QuestionSet ,
                                                                (select ConfigValue from TBL_Configuration WHERE ConfigField = 'Exam date') AS ExamDate 
                                                                from TBL_Student WHERE StudentID = @StudentID", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        SD.StudentID = tbl.Rows[0]["StudentID"].ToString();
                        SD.Studentname = tbl.Rows[0]["Studentname"].ToString();
                        SD.QSet = tbl.Rows[0]["QuestionSet"].ToString();
                        SD.StuExamdate = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["ExamDate"]));
                    }
                }
                return SD;
            }
            catch (Exception ex)
            {
                return SD;
            }
        }

        public int GetExamTime()
        {
            int NowTime = 0;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Open) { con.Close(); }
                con.Open();

                SqlCommand cmdTime = new SqlCommand("SELECT cast(format(GETDATE(),'HHmm') as int) AS Nowtime", con);
                NowTime = Convert.ToInt32(cmdTime.ExecuteScalar());
            }
            return NowTime;
        }

        public List<QuestionLists> GetAllQuestionByQset(string Qset)
        {
            List<QuestionLists> QLists = new List<QuestionLists>();
            string StudentID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_StartExam", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    da.SelectCommand.Parameters.AddWithValue("@QSet", Qset);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            QuestionLists QL = new QuestionLists();
                            QL.QuestionID = dr["QuestionID"].ToString();
                            QL.Question = dr["Question"].ToString();
                            QL.Ans1 = dr["Answer1"].ToString();
                            QL.Ans2 = dr["Answer2"].ToString();
                            QL.Ans3 = dr["Answer3"].ToString();
                            QL.Ans4 = dr["Answer4"].ToString();
                            QL.AnsOption = dr["AnsOption"].ToString();
                            QL.RightAns = dr["RightAnswer"].ToString();
                            QL.IsCompleted = dr["IsCompleted"].ToString();
                            QLists.Add(QL);
                        }
                    }
                }
                return QLists;
            }
            catch (Exception ex)
            {
                return QLists;
            }
        }

        public QuestionLists GetQuestionByQid(string QSet, string QID)
        {
            QuestionLists QL = new QuestionLists();
            string StudentID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT Q.QuestionID, Q.QuestionSet, Q.Question, Q.Answer1, Q.Answer2, Q.Answer3, Q.Answer4, 
                                                                S.IsCompleted, S.AnsOption FROM TBL_StudentAnswer S 
                                                                INNER JOIN TBL_QuestionMaster Q ON S.QuestionID = Q.QuestionID
                                                                WHERE Q.QuestionSet = @QSet AND S.StudentID = @StudentID AND S.QuestionID = @QID", con);
                    da.SelectCommand.Parameters.AddWithValue("@QSet", QSet);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    da.SelectCommand.Parameters.AddWithValue("@QID", QID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        QL.QuestionID = tbl.Rows[0]["QuestionID"].ToString();
                        QL.Question = tbl.Rows[0]["Question"].ToString();
                        QL.Ans1 = tbl.Rows[0]["Answer1"].ToString();
                        QL.Ans2 = tbl.Rows[0]["Answer2"].ToString();
                        QL.Ans3 = tbl.Rows[0]["Answer3"].ToString();
                        QL.Ans4 = tbl.Rows[0]["Answer4"].ToString();
                        QL.AnsOption = tbl.Rows[0]["AnsOption"].ToString();
                        QL.IsCompleted = tbl.Rows[0]["IsCompleted"].ToString();
                    }
                }
                return QL;
            }
            catch (Exception ex)
            {
                return QL;
            }
        }

        public bool AnsweredQuestion(QAnsModel QAM)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_StudentAnswered", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentID", QAM.StudentID);
                    cmd.Parameters.AddWithValue("@QuestionSet", QAM.QuestionSet);
                    cmd.Parameters.AddWithValue("@QuestionID", QAM.QuestionID);
                    cmd.Parameters.AddWithValue("@AnsOption", QAM.AnsOption);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);

                    if (ERRORCODE == 0)
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

        public Dictionary<string, string> checkExamdate()
        {
            Dictionary<string, string> ResultDict = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("SELECT ConfigValue FROM TBL_Configuration WHERE ConfigField = 'Exam date'", con);
                    int ExamDate = Convert.ToInt32(cmd.ExecuteScalar());

                    SqlCommand cmdToday = new SqlCommand("SELECT CONVERT(VARCHAR(100),GETDATE(),112) AS TodayDate", con);
                    int Today = Convert.ToInt32(cmdToday.ExecuteScalar());

                    if (Convert.ToInt32(Today) == ExamDate)
                    {
                        ResultDict.Add("Status", "0");
                        ResultDict.Add("Message", "Exam");
                    }
                    else
                    {
                        ResultDict.Add("Status", "1");
                        ResultDict.Add("Message", "Sorry!! Today is not Your Examdate");
                    }
                }
                return ResultDict;
            }
            catch (Exception ex)
            {
                ResultDict.Add("Status", "1");
                ResultDict.Add("Message", "Failed to check Examdate !!");
                return ResultDict;
            }
        }

        public Dictionary<string, string> CheckAlreadyExamGiven(string Questionset, string Examdate)
        {
            Dictionary<string, string> Resultdict = new Dictionary<string, string>();
            string StudentID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT COUNT(*) As IsExamAlreadyAttend FROM [TBL_ExamDetails] 
                                                                WHERE StudentID = @StudentID AND QuestionSet = @Questionset AND Examdate = @Examdate", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    da.SelectCommand.Parameters.AddWithValue("@Questionset", Questionset);
                    da.SelectCommand.Parameters.AddWithValue("@Examdate", global.ConvertStringToInt(Examdate));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        int examCount = Convert.ToInt32(tbl.Rows[0]["IsExamAlreadyAttend"]);
                        if (examCount > 0)
                        {
                            Resultdict.Add("Status", "1");
                            Resultdict.Add("Message", "Sorry!! You already Attended exam. Thank you.");
                        }
                        else
                        {
                            Resultdict.Add("Status", "0");
                            Resultdict.Add("Message", "Exam");
                        }
                    }
                    else
                    {
                        Resultdict.Add("Status", "1");
                        Resultdict.Add("Message", "Failed to check Exam Given or not !!");
                    }
                }
                return Resultdict;
            }
            catch (Exception ex)
            {
                Resultdict.Add("Status", "1");
                Resultdict.Add("Message", "Failed to check Exam Given or not !!");
                return Resultdict;
            }
        }

        public List<DiscountList> GetAllDiscounts()
        {
            List<DiscountList> DiscountLists = new List<DiscountList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT Marksfrom, Marksto, Discount FROM TBL_DiscountMaster", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DiscountList DL = new DiscountList();
                            DL.MarksFrom = dr["Marksfrom"].ToString();
                            DL.MarksTo = dr["Marksto"].ToString();
                            DL.FeesDiscount = dr["Discount"].ToString();
                            DiscountLists.Add(DL);
                        }
                    }
                }
                return DiscountLists;
            }
            catch (Exception ex)
            {
                return DiscountLists;
            }
        }

        public AnsModel GetAnswerDetails(string QSet, string StudentID)
        {
            AnsModel AM = new AnsModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_FetchQuestionAnswered", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.AddWithValue("@Questionset", QSet);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        AM.TotalNoOfQuestions = Convert.ToInt32(tbl.Rows[0]["TotalNoOfQuestion"]);
                        AM.NoOfPendingQuestion = Convert.ToInt32(tbl.Rows[0]["TotalNoOfQuestion"]) - Convert.ToInt32(tbl.Rows[0]["NoOfQuestionAnswered"]);
                        AM.NoOfQuestionAnswered = Convert.ToInt32(tbl.Rows[0]["NoOfQuestionAnswered"]);
                        AM.NoOfReviewedQuestion = Convert.ToInt32(tbl.Rows[0]["NoOfReviewedQuestion"]);
                        AM.TotalMarks = Convert.ToInt32(tbl.Rows[0]["TotalMarks"]);
                    }
                }
                return AM;
            }
            catch (Exception ex)
            {
                return AM;
            }
        }

        public bool AddExamDetails(OnlineExamModel OEModel)
        {
            try
            {
                OEModel = global.NullFilter<OnlineExamModel>(OEModel);
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"USP_AddExamDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", OEModel.StudentID);
                    cmd.Parameters.AddWithValue("@QuestionSet", OEModel.QuestionSet);
                    cmd.Parameters.AddWithValue("@Examdate", global.ConvertStringToInt(OEModel.StuExamdate));
                    cmd.Parameters.AddWithValue("@Starttime", OEModel.EStartTime);
                    cmd.Parameters.AddWithValue("@EndTime", OEModel.EEndtime);
                    cmd.Parameters.AddWithValue("@TotalQuestion", OEModel.AETotalNoOfQuestions);
                    cmd.Parameters.AddWithValue("@NoOfReviewed", OEModel.AENoOfReviewedQuestions);
                    cmd.Parameters.AddWithValue("@NoOfAttempt", OEModel.AENoOfAttemptedQuestions);
                    cmd.Parameters.AddWithValue("@NoOfPending", OEModel.AENoOfPendingQuestions);
                    cmd.Parameters.AddWithValue("@Marks", OEModel.AETotalMarks);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);
                    if (ERRORCODE == 0)
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

        public bool MarkQtnAsReview(string QSet, string QID)
        {
            string StudentID = HttpContext.Current.Session["_UserId"].ToString();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand(@"UPDATE [dbo].[TBL_StudentAnswer]
			                                        SET IsCompleted = 2, AnsOption = ''
			                                        WHERE [StudentID] = @StudentID AND [QuestionSet] = @QuestionSet AND 
                                                    [QuestionID] = @QuestionID", con);
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);
                    cmd.Parameters.AddWithValue("@QuestionSet", QSet);
                    cmd.Parameters.AddWithValue("@QuestionID", QID);

                    int Status = cmd.ExecuteNonQuery();

                    if (Status > 0)
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
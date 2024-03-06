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
    public class QuestionMasterDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public QuestionMasterDal()
        {
            global = new GlobalFunction();
        }
        public Dictionary<string, string> AddQuestions(QuestionMasterModel QModel)
        {
            Dictionary<string, string> Resultdict = new Dictionary<string, string>();
            try
            {
                QModel = global.NullFilter<QuestionMasterModel>(QModel);
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_AddQuestion", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@QuestionID", QModel.Qid);
                    cmd.Parameters.AddWithValue("@QuestionSet", QModel.QSet);
                    cmd.Parameters.AddWithValue("@Question", QModel.Question);
                    cmd.Parameters.AddWithValue("@Answer1", QModel.Ans1);
                    cmd.Parameters.AddWithValue("@Answer2", QModel.Ans2);
                    cmd.Parameters.AddWithValue("@Answer3", QModel.Ans3);
                    cmd.Parameters.AddWithValue("@Answer4", QModel.Ans4);
                    cmd.Parameters.AddWithValue("@RightAnswer", QModel.RightAns);
                    cmd.Parameters.AddWithValue("@opsection", QModel.opsection);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);
                    if (ERRORCODE == 0)
                    {
                        Resultdict.Add("Status", "0");
                        Resultdict.Add("Message", "Question Added Successfully");
                    }
                    else
                    {
                        Resultdict.Add("Status", "1");
                        Resultdict.Add("Message", "Failed to add Question");
                    }
                }
                return Resultdict;
            }
            catch (Exception ex)
            {
                Resultdict.Add("Status", "1");
                Resultdict.Add("Message", "Failed to add Question");
                return Resultdict;
            }
        }

        public List<QuestionList> GetQuestionList(string QSet)
        {
            List<QuestionList> QuestionLists = new List<QuestionList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT QuestionID, Question, Answer1, Answer2, Answer3, Answer4, RightAnswer 
                                                            FROM TBL_QuestionMaster WHERE QuestionSet = @QuestionSet", con);
                    da.SelectCommand.Parameters.AddWithValue("@QuestionSet", QSet);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            QuestionList QL = new QuestionList();
                            QL.QID = dr["QuestionID"].ToString();
                            QL.Question = dr["Question"].ToString();
                            QL.Answer1 = dr["Answer1"].ToString();
                            QL.Answer2 = dr["Answer2"].ToString();
                            QL.Answer3 = dr["Answer3"].ToString();
                            QL.Answer4 = dr["Answer4"].ToString();
                            QL.RightAnswer = dr["RightAnswer"].ToString();
                            QuestionLists.Add(QL);
                        }
                    }
                }
                return QuestionLists;
            }
            catch (Exception ex)
            {
                return QuestionLists;
            }
        }

        public QuestionMasterModel GetQuestionDetail(string QID)
        {
            QuestionMasterModel QModel = new QuestionMasterModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT QuestionID, Question, Answer1, Answer2, Answer3, Answer4, RightAnswer, QuestionSet 
                                                                FROM TBL_QuestionMaster WHERE QuestionID = @QuestionID", con);
                    da.SelectCommand.Parameters.AddWithValue("@QuestionID", QID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        QModel.Qid = tbl.Rows[0]["QuestionID"].ToString();
                        QModel.Question = tbl.Rows[0]["Question"].ToString();
                        QModel.Ans1 = tbl.Rows[0]["Answer1"].ToString();
                        QModel.Ans2 = tbl.Rows[0]["Answer2"].ToString();
                        QModel.Ans3 = tbl.Rows[0]["Answer3"].ToString();
                        QModel.Ans4 = tbl.Rows[0]["Answer4"].ToString();
                        QModel.QSet = tbl.Rows[0]["QuestionSet"].ToString();
                        QModel.RightAns = tbl.Rows[0]["RightAnswer"].ToString();
                    }
                }
                return QModel;
            }
            catch (Exception ex)
            {
                return QModel;
            }
        }

        public bool RemoveQuestion(string QID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM TBL_QuestionMaster WHERE QuestionID = @QuestionID", con);
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
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
    public class ExamReportDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public ExamReportDal()
        {
            global = new GlobalFunction();
        }
        public List<ExamReportListModel> GetExamReportList(string Fromdate, string Todate)
        {
            List<ExamReportListModel> ExamReportList = new List<ExamReportListModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_SearchExamReport", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.AddWithValue("@Fromdate", global.ConvertStringToInt(Fromdate));
                    da.SelectCommand.Parameters.AddWithValue("@Todate", global.ConvertStringToInt(Todate));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            ExamReportListModel ELModel = new ExamReportListModel();
                            ELModel.StudentID = dr["StudentID"].ToString();
                            ELModel.Studentname = dr["Studentname"].ToString();
                            ELModel.Questionset = dr["QuestionSet"].ToString();
                            ELModel.Endtime = dr["EndTime"].ToString();
                            ELModel.TotalQuestion = dr["TotalQuestion"].ToString();
                            ELModel.NoOfAttempt = dr["NoOfAttempt"].ToString();
                            ELModel.NoOfPending = dr["NoOfPending"].ToString();
                            ELModel.Marks = dr["Marks"].ToString();
                            ELModel.Discount = dr["Discount"].ToString() + "%";

                            if (dr["Starttime"].ToString() != "")
                            {
                                int HH = Convert.ToInt32(dr["Starttime"].ToString().Substring(0, 2));
                                int MM = Convert.ToInt32(dr["Starttime"].ToString().Substring(2));

                                TimeSpan timespan = new TimeSpan(HH, MM, 00);
                                DateTime time = DateTime.Today.Add(timespan);
                                ELModel.Starttime = time.ToString("hh:mm tt");
                            }

                            if (dr["EndTime"].ToString() != "")
                            {
                                int HH = Convert.ToInt32(dr["EndTime"].ToString().Substring(0, 2));
                                int MM = Convert.ToInt32(dr["EndTime"].ToString().Substring(2));

                                TimeSpan timespan = new TimeSpan(HH, MM, 00);
                                DateTime time = DateTime.Today.Add(timespan);
                                ELModel.Endtime = time.ToString("hh:mm tt");
                            }

                            ExamReportList.Add(ELModel);
                        }
                    }
                }
                return ExamReportList;
            }
            catch (Exception ex)
            {
                return ExamReportList;
            }
        }
    }
}
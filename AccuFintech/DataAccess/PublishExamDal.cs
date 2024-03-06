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
    public class PublishExamDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public PublishExamDal()
        {
            global = new GlobalFunction();
        }
        public bool SetExamdate(PublishExamModel PEModel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE TBL_Configuration SET ConfigValue = @Questionset WHERE ConfigField = 'Question Set'", con);
                    cmd.Parameters.AddWithValue("@Questionset", PEModel.QuestionSet);
                    cmd.ExecuteNonQuery();

                    SqlCommand cmd2 = new SqlCommand("UPDATE TBL_Configuration SET ConfigValue = @Examdate WHERE ConfigField = 'Exam date'", con);
                    cmd2.Parameters.AddWithValue("@Examdate", global.ConvertStringToInt(PEModel.Examdate));
                    cmd2.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<ConfigList> GetConfigurationList()
        {
            List<ConfigList> ConfigLists = new List<ConfigList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT ConfigField, ConfigValue FROM TBL_Configuration WHERE ConfigField IN('Question Set','Exam date')", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            ConfigList CL = new ConfigList();
                            CL.ConfigField = dr["ConfigField"].ToString();
                            if (CL.ConfigField == "Exam date")
                            {
                                CL.ConfigValue = global.ConvertIntToString(Convert.ToInt32(dr["ConfigValue"]));
                            }
                            else
                            {
                                CL.ConfigValue = dr["ConfigValue"].ToString();
                            }
                            ConfigLists.Add(CL);
                        }
                    }
                }
                return ConfigLists;
            }
            catch (Exception ex)
            {
                return ConfigLists;
            }
        }

        public Dictionary<string, string> ResetExam(string StudentID)
        {
            Dictionary<string, string> Resultset = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_ResetExam", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", StudentID);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);

                    if (ERRORCODE == 0)
                    {
                        Resultset.Add("Status", "0");
                        Resultset.Add("Message", "Exam Reset successfully done");
                    }
                    else
                    {
                        Resultset.Add("Status", "1");
                        Resultset.Add("Message", "Failed to reset exam !!");
                    }
                }
                return Resultset;
            }
            catch (Exception ex)
            {
                Resultset.Add("Status", "1");
                Resultset.Add("Message", "Failed to reset exam !!");
                return Resultset;
            }
        }
    }
}
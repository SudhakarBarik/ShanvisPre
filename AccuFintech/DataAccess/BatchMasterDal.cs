using AccuFintech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AccuFintech.DataAccess
{
    public class BatchMasterDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public BatchMasterDal()
        {
            global = new GlobalFunction();
        }

        public bool AddNewSession(_Session session)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "insert into TBL_SessionMaster (SessionName , StartDate , EndDate) values (@SessionName , @StartDate ,@EndDate )";
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@SessionName", session.Name);
                    cmd.Parameters.AddWithValue("@StartDate", global.ConvertStringToInt(session.FDate));
                    cmd.Parameters.AddWithValue("@EndDate", global.ConvertStringToInt(session.TDate));
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
        public List<_Session> GetAllSessions()
        {
            List<_Session> sessionList = new List<_Session>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "select Id , SessionName , StartDate , EndDate , Convert(varchar,CreateDate,103)as CreateDate from TBL_SessionMaster";
                    SqlDataAdapter sda = new SqlDataAdapter(qry, con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            _Session session = new _Session();
                            session.Id = Convert.ToInt32(dr["Id"].ToString());
                            session.Name = dr["SessionName"].ToString();
                            session.FDate = global.ConvertIntToString(Convert.ToInt32(dr["StartDate"].ToString()));
                            session.TDate = global.ConvertIntToString(Convert.ToInt32(dr["EndDate"].ToString()));
                            session.CreateDate = dr["CreateDate"].ToString();
                            sessionList.Add(session);
                        }
                    }
                    return sessionList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<_Session>();
            }

        }
        public bool DeleteSession(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                string qry = "Delete from TBL_SessionMaster where Id = @id";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@id", id);
                int rows = cmd.ExecuteNonQuery();
                return rows > 0;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
        public bool AddBatchDetails(BatchMaster batch)
        {
            bool rt = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "insert into TBL_BatchMaster (BatchID ,BatchName,CourseID ,Session ,BStartDate ,BEndDate ,Frequncy) values (@BatchID ,@BatchName , @CourseID ,@Session ,@BStartDate ,@BEndDate ,@Frequncy)";
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@BatchID", batch.BatchID);
                    cmd.Parameters.AddWithValue("@BatchName", batch.BatchName);
                    cmd.Parameters.AddWithValue("@CourseID", batch.CourseID);
                    cmd.Parameters.AddWithValue("@Session", batch.Session);
                    cmd.Parameters.AddWithValue("@BStartDate", global.ConvertStringToInt(batch.BStartDate));
                    cmd.Parameters.AddWithValue("@BEndDate", global.ConvertStringToInt(batch.BEndDate));
                    cmd.Parameters.AddWithValue("@Frequncy", batch.Frequncy);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        var ch = GenerateDate(batch);
                        foreach (checkedDatesAndDays item in ch)
                        {
                            string qry2 = "insert into TBL_BatchSchedule (BatchID , BDay , BDates) values(@BatchID,@BDay ,@BDates)";
                            SqlCommand cmd2 = new SqlCommand(qry2, con);
                            cmd2.Parameters.AddWithValue("@BatchID", batch.BatchID);
                            cmd2.Parameters.AddWithValue("@BDay", item.day);
                            cmd2.Parameters.AddWithValue("@BDates", global.ConvertStringToInt(item.date));
                            int rows2 = cmd2.ExecuteNonQuery();
                            if (rows2 > 0)
                            {
                                rt = true;
                            }
                        }

                    }
                    else
                    {
                        rt = false;
                    }
                    return rt;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
        public List<checkedDatesAndDays> GenerateDate(BatchMaster batch)
        {
            List<checkedDatesAndDays> chd = new List<checkedDatesAndDays>();
            string checkstatus = JsonConvert.SerializeObject(batch.hiddenCheckedData);
            var jsonResult = JsonConvert.DeserializeObject(checkstatus).ToString();
            batch.CheckDtday = JsonConvert.DeserializeObject<List<checkedDatesAndDays>>(jsonResult);
            string Enddt = batch.BEndDate;
            foreach (checkedDatesAndDays item in batch.CheckDtday)
            {
                DateTime begindate = Convert.ToDateTime(DateTime.ParseExact(item.date, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                string DateDay = item.day;
                DateTime enddate = Convert.ToDateTime(DateTime.ParseExact(Enddt, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                while (begindate < enddate)
                {
                    checkedDatesAndDays ch = new checkedDatesAndDays();
                    string y = begindate.ToString("dd/MM/yyyy");
                    ch.date = y;
                    ch.day = DateDay;
                    chd.Add(ch);
                    begindate = begindate.AddDays(7);
                }
            }
            return chd;
        }
        public List<BatchMaster> GetAllBatches()
        {
            List<BatchMaster> Bm = new List<BatchMaster>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter DA = new SqlDataAdapter("select BatchID,BatchName,CourseID,Session,BStartDate,BEndDate, Frequncy from TBL_BatchMaster", con);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchMaster bl = new BatchMaster()
                            {
                                BatchID = dr["BatchID"].ToString(),
                                BatchName = dr["BatchName"].ToString(),
                                CourseID = dr["CourseID"].ToString(),
                                Session = dr["Session"].ToString(),
                                BStartDate = global.ConvertIntToString(Convert.ToInt32(dr["BStartDate"].ToString())),
                                BEndDate = global.ConvertIntToString(Convert.ToInt32(dr["BEndDate"].ToString())),
                                Frequncy = dr["Frequncy"].ToString(),
                            };
                            Bm.Add(bl);

                        }
                        return Bm;
                    }
                    else
                    {
                        return new List<BatchMaster>();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<BatchMaster>();
            }
        }
        public BatchMaster GetBatchDetailsByID(string Batchid)
        {
            BatchMaster BM = new BatchMaster();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter DA = new SqlDataAdapter("select BatchID,BatchName,CourseID,Session,BStartDate,BEndDate, Frequncy from TBL_BatchMaster where BatchID = @BatchID ", con);
                    DA.SelectCommand.Parameters.AddWithValue("@BatchID", Batchid);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        BM.BatchID = dt.Rows[0]["BatchID"].ToString();
                        BM.BatchName = dt.Rows[0]["BatchName"].ToString();
                        BM.CourseID = dt.Rows[0]["CourseID"].ToString();
                        BM.Session = dt.Rows[0]["Session"].ToString();
                        BM.BStartDate = global.ConvertIntToString(Convert.ToInt32(dt.Rows[0]["BStartDate"].ToString()));
                        BM.BEndDate = global.ConvertIntToString(Convert.ToInt32(dt.Rows[0]["BEndDate"].ToString()));
                        BM.Frequncy = dt.Rows[0]["Frequncy"].ToString();
                    }
                    return BM;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new BatchMaster();
            }
        }
        public List<BatchMaster> GetBatchDetailsBySessionID(string Sessionid)
        {
            List<BatchMaster> batchlist = new List<BatchMaster>();

            string qry;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    qry = "select BatchID,BatchName,CourseID,Session,BStartDate,BEndDate, Frequncy from TBL_BatchMaster where Session = @Session ";
                    if (Sessionid == "All")
                    {
                        qry = "select BatchID,BatchName,CourseID,Session,BStartDate,BEndDate, Frequncy from TBL_BatchMaster";
                    }
                    SqlDataAdapter DA = new SqlDataAdapter(qry, con);
                    DA.SelectCommand.Parameters.AddWithValue("@Session", Sessionid);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            BatchMaster bl = new BatchMaster()
                            {
                                BatchID = dr["BatchID"].ToString(),
                                BatchName = dr["BatchName"].ToString(),
                                CourseID = dr["CourseID"].ToString(),
                                Session = dr["Session"].ToString(),
                                BStartDate = global.ConvertIntToString(Convert.ToInt32(dr["BStartDate"].ToString())),
                                BEndDate = global.ConvertIntToString(Convert.ToInt32(dr["BEndDate"].ToString())),
                                Frequncy = dr["Frequncy"].ToString(),
                            };
                            batchlist.Add(bl);

                        }
                        return batchlist;
                    }
                    else
                    {
                        return new List<BatchMaster>();
                    }

                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<BatchMaster>();
            }
        }
        public Dictionary<string, string> RemoveBatchDetails(string BatchId)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM TBL_BatchMaster WHERE BatchID = @BatchID", con);
                    cmd.Parameters.AddWithValue("@BatchID", BatchId);

                    int Status = cmd.ExecuteNonQuery();

                    if (Status > 0)
                    {
                        Result.Add("status", "0");
                        Result.Add("msg", "Batch details removed successfully");
                    }
                    else
                    {
                        Result.Add("status", "1");
                        Result.Add("msg", "Failed to remove Batch details");
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to remove Batch details");
                return Result;
            }
        }

    }
}
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
    public class FeesStructureDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;

        public FeesStructureDal()
        {
            global = new GlobalFunction();
        }

        public FeesStructureModel GetFeesDetailsByStid(string StudentID)
        {
            FeesStructureModel feesmodel = new FeesStructureModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Usp_GetFeeStructure", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@Studentid", StudentID);
                    sda.SelectCommand.Parameters.AddWithValue("@FranchaiseCode","");
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        feesmodel.FranchaiseCode = dt.Rows[0]["FranchaiseCode"].ToString();
                        feesmodel.Batch = dt.Rows[0]["Batch"].ToString();
                        feesmodel.Studentid = dt.Rows[0]["Studentid"].ToString();
                        feesmodel.Course = dt.Rows[0]["Course"].ToString();
                        feesmodel.TotalFees = dt.Rows[0]["TotalFees"].ToString();
                        feesmodel.TotalPaid = dt.Rows[0]["TotalPaid"].ToString();
                        feesmodel.RemainFee = dt.Rows[0]["RemainFee"].ToString();
                    }
                    return feesmodel;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new FeesStructureModel();
            }
        }

        public List<FeesStructureModel> GetFeesDetailsByFilter(string StudentID , string FranchaiseCode)
        {
            List<FeesStructureModel> FeesList = new List<FeesStructureModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("Usp_GetFeeStructure", con);
                    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    sda.SelectCommand.Parameters.AddWithValue("@FranchaiseCode", FranchaiseCode ?? "");
                    sda.SelectCommand.Parameters.AddWithValue("@Studentid", StudentID ?? "");
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            FeesStructureModel feesmodel = new FeesStructureModel();
                            feesmodel.FranchaiseCode = item["FranchaiseCode"].ToString();
                            feesmodel.Batch = item["Batch"].ToString();
                            feesmodel.Studentid = item["Studentid"].ToString();
                            feesmodel.Course = item["Course"].ToString();
                            feesmodel.TotalFees = item["TotalFees"].ToString();
                            feesmodel.TotalPaid = item["TotalPaid"].ToString();
                            feesmodel.RemainFee = item["RemainFee"].ToString();
                            feesmodel.PayDate =  item["PayDt"].ToString();
                            FeesList.Add(feesmodel);
                        }
                       
                    }
                    return FeesList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<FeesStructureModel>();
            }
        }

        public List<FeesStructureModel> GetAllFeesDetailsByStudent(string StudentID)
        {
            List<FeesStructureModel> FeesList = new List<FeesStructureModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "select FranchaiseCode,Studentid,Course,Batch,TotalFees , PaidFees , convert(nvarchar(12),CONVERT(date,payUtimestamp),101)as PayDt from TBL_FeesStructure where Studentid = @Studentid";
                    SqlDataAdapter sda = new SqlDataAdapter(qry, con);
                    sda.SelectCommand.Parameters.AddWithValue("@Studentid", StudentID ?? "");
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            FeesStructureModel feesmodel = new FeesStructureModel();
                            feesmodel.FranchaiseCode = item["FranchaiseCode"].ToString();
                            feesmodel.Batch = item["Batch"].ToString();
                            feesmodel.Studentid = item["Studentid"].ToString();
                            feesmodel.Course = item["Course"].ToString();
                            feesmodel.TotalFees = item["TotalFees"].ToString();
                            feesmodel.PaidFees = item["PaidFees"].ToString();
                            feesmodel.PayDate = item["PayDt"].ToString();
                            FeesList.Add(feesmodel);
                        }

                    }
                    return FeesList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new List<FeesStructureModel>();
            }
        }


        public bool PayRemainFees(FeesStructureModel fmodel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "insert into TBL_FeesStructure (FranchaiseCode , Studentid , Course , Batch , TotalFees , PaidFees,PayType,PayUTimestamp) values (@FranchaiseCode, @Studentid, @Course,@Batch, @TotalFees, @PaidFees,@PayType ,GETDATE())";
                    SqlCommand cmd = new SqlCommand(qry, con);
                    cmd.Parameters.AddWithValue("@FranchaiseCode", fmodel.FranchaiseCode);
                    cmd.Parameters.AddWithValue("@Studentid", fmodel.Studentid);
                    cmd.Parameters.AddWithValue("@Course", fmodel.Course);
                    cmd.Parameters.AddWithValue("@Batch", fmodel.Batch);
                    cmd.Parameters.AddWithValue("@TotalFees", fmodel.TotalFees);
                    cmd.Parameters.AddWithValue("@PaidFees", fmodel.PaidFees);
                    cmd.Parameters.AddWithValue("@PayType", fmodel.PayType);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex )
            {
                ex.Message.ToString();
                return false;
            }
        }

    }
}
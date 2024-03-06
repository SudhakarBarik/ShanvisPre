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
    public class FranchaiseDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public FranchaiseDal()
        {
            global = new GlobalFunction();
        }
        public List<FranchaiseModel> GetAllFranchaisees()
        {
            List<FranchaiseModel> Bm = new List<FranchaiseModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter DA = new SqlDataAdapter("select FranchaiseID,FranchaiseCode,Name,dtofop,Add1,ManagerName,Prefix,FranchaisePhNo,city,state from TBL_FranchaiseMaster ", con);
                    DataTable dt = new DataTable();
                    DA.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            FranchaiseModel bl = new FranchaiseModel()
                            {
                                FranchaiseId = long.Parse(dr["FranchaiseID"].ToString()),
                                FranchaiseCode = dr["FranchaiseCode"].ToString(),
                                FranchaiseName = dr["Name"].ToString(),
                                FranchaiseOpeningDate = dr["dtofop"].ToString().Substring(6, 2) + "/" + dr["dtofop"].ToString().Substring(4, 2) + "/" + dr["dtofop"].ToString().Substring(0, 4),
                                FranchaiseAddress = dr["Add1"].ToString(),
                                FranchaiseManager = dr["ManagerName"].ToString(),
                                FranchaisePrefix = dr["Prefix"].ToString(),
                                FranchaiseMobileNo = dr["FranchaisePhNo"].ToString(),
                                CityName = dr["city"].ToString(),
                                StateName = dr["state"].ToString()
                            };
                            Bm.Add(bl);

                        }
                        return Bm;
                    }
                    else
                    {
                        return new List<FranchaiseModel>();
                    }

                }
            }
            catch (Exception ex)
            {
                return new List<FranchaiseModel>();
            }
        }
        public FranchaiseModel FranchaisebyBId(long BrId)
        {
            FranchaiseModel bm = new FranchaiseModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("select FranchaiseID,FranchaiseCode,Name,dtofop,Add1,ManagerName,Prefix,FranchaisePhNo,city,state from TBL_FranchaiseMaster where FranchaiseID=@FranchaiseID ", con);
                    da.SelectCommand.Parameters.AddWithValue("@FranchaiseID", BrId);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        bm.FranchaiseId = long.Parse(dt.Rows[0]["FranchaiseID"].ToString());
                        bm.FranchaiseCode = dt.Rows[0]["FranchaiseCode"].ToString();
                        bm.FranchaiseName = dt.Rows[0]["Name"].ToString().ToUpper();
                        bm.FranchaiseOpeningDate = dt.Rows[0]["dtofop"].ToString().Substring(6, 2) + "/" + dt.Rows[0]["dtofop"].ToString().Substring(4, 2) + "/" + dt.Rows[0]["dtofop"].ToString().Substring(0, 4);
                        bm.FranchaiseAddress = dt.Rows[0]["Add1"].ToString();
                        bm.FranchaiseManager = dt.Rows[0]["ManagerName"].ToString();
                        bm.FranchaisePrefix = dt.Rows[0]["Prefix"].ToString();
                        bm.FranchaiseMobileNo = dt.Rows[0]["FranchaisePhNo"].ToString();
                        bm.CityName = dt.Rows[0]["city"].ToString();
                        bm.StateName = dt.Rows[0]["state"].ToString();
                        return bm;
                    }
                    else
                    {
                        return new FranchaiseModel();
                    }
                }
            }
            catch (Exception ex)
            {

                return new FranchaiseModel();
            }

        }
        public bool AddNewFranchaise(FranchaiseModel details)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("insert into TBL_FranchaiseMaster(FranchaiseCode,Name,Add1,Prefix,FranchaisePhNo,ManagerName,dtofop,city,state) values(@FranchaiseCode,@FranchaiseName,@FranchaiseAddress,@FranchaisePrefix,@FranchaiseMobileNo,@FranchaiseManager,@FranchaiseOpeningDate,@CityName,@StateName)", con);
                    cmd.Parameters.AddWithValue("@FranchaiseCode", details.FranchaiseCode);
                    cmd.Parameters.AddWithValue("@FranchaiseName", details.FranchaiseName);
                    cmd.Parameters.AddWithValue("@FranchaiseAddress", details.FranchaiseAddress);
                    cmd.Parameters.AddWithValue("@FranchaisePrefix", details.FranchaisePrefix);
                    cmd.Parameters.AddWithValue("@FranchaiseMobileNo", details.FranchaiseMobileNo);
                    cmd.Parameters.AddWithValue("@FranchaiseManager", details.FranchaiseManager);
                    cmd.Parameters.AddWithValue("@FranchaiseOpeningDate", global.ConvertStringToInt(details.FranchaiseOpeningDate));
                    cmd.Parameters.AddWithValue("@CityName", details.CityName);
                    cmd.Parameters.AddWithValue("@StateName", details.StateName);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
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
        public bool UpdateFranchaise(FranchaiseModel details)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update TBL_FranchaiseMaster set FranchaiseCode=@FranchaiseCode,Name=@FranchaiseName,Add1=@FranchaiseAddress,Prefix=@FranchaisePrefix,FranchaisePhNo=@FranchaiseMobileNo,ManagerName=@FranchaiseManager,dtofop=@FranchaiseOpeningDate,city=@CityName,state=@StateName where FranchaiseID=@FranchaiseID", con);
                    cmd.Parameters.AddWithValue("@FranchaiseID", details.FranchaiseId);
                    cmd.Parameters.AddWithValue("@FranchaiseCode", details.FranchaiseCode);
                    cmd.Parameters.AddWithValue("@FranchaiseName", details.FranchaiseName);
                    cmd.Parameters.AddWithValue("@FranchaiseAddress", details.FranchaiseAddress);
                    cmd.Parameters.AddWithValue("@FranchaisePrefix", details.FranchaisePrefix);
                    cmd.Parameters.AddWithValue("@FranchaiseMobileNo", details.FranchaiseMobileNo);
                    cmd.Parameters.AddWithValue("@FranchaiseManager", details.FranchaiseManager);
                    cmd.Parameters.AddWithValue("@FranchaiseOpeningDate", global.ConvertStringToInt(details.FranchaiseOpeningDate));
                    cmd.Parameters.AddWithValue("@CityName", details.CityName);
                    cmd.Parameters.AddWithValue("@StateName", details.StateName);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
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
        public bool DeleteFranchaise(long id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("delete from  TBL_FranchaiseMaster where FranchaiseID=@FranchaiseID", con);
                    cmd.Parameters.AddWithValue("@FranchaiseID", id);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();
                    con.Close();
                    if (i > 0)
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
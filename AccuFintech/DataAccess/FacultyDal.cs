using AccuFintech.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace AccuFintech.DataAccess
{
    public class FacultyDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public FacultyDal()
        {
            global = new GlobalFunction();
        }

        public List<FacultyModel> GetFacultyFranchaiseWise(string FranchaiseCode)
        {
            string qry = "";
            List<FacultyModel> FacultyLists = new List<FacultyModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();
                    qry = "SELECT FacultyID,FranchaiseCode,FacultyName,Gender,DOB,Phone,Email,AadharNo,Address,State,District,PinCode,MaxQualification,Department,TotalExp,JoiningDate,Password,FProfilePic FROM TBL_FacultyMaster where FranchaiseCode = @FranchaiseCode";
                    if (FranchaiseCode == "All")
                    {
                        qry = "SELECT FacultyID,FranchaiseCode,FacultyName,Gender,DOB,Phone,Email,AadharNo,Address,State,District,PinCode,MaxQualification,Department,TotalExp,JoiningDate,Password,FProfilePic FROM TBL_FacultyMaster";
                    }
                    SqlDataAdapter da = new SqlDataAdapter(qry, con);
                    da.SelectCommand.Parameters.AddWithValue("@FranchaiseCode", FranchaiseCode);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            FacultyModel FL = new FacultyModel();
                            FL.FacultyID = dr["FacultyID"].ToString();
                            FL.FacultyName = dr["FacultyName"].ToString();
                            FL.FranchaiseCode = dr["FranchaiseCode"].ToString();
                            FL.Phone = dr["Phone"].ToString();
                            FL.Gender = dr["Gender"].ToString();
                            FL.Email = dr["Email"].ToString();
                            FL.MaxQualification = dr["MaxQualification"].ToString();
                            FL.Department = dr["Department"].ToString();
                            FL.TotalExp = dr["TotalExp"].ToString();
                            //FL.JoiningDate = global.ConvertIntToString(Convert.ToInt32(dr["joiningDate"]));
                            FacultyLists.Add(FL);
                        }
                    }
                }
                return FacultyLists;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return FacultyLists;
            }
        }
        public bool AddOrUpdateFaculty(FacultyModel Facmodel)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("AddORUpdateFacultyMaster", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FacultyID", Facmodel.FacultyID ?? "");
                    cmd.Parameters.AddWithValue("@FranchiseCode", Facmodel.FranchaiseCode);
                    cmd.Parameters.AddWithValue("@FacultyName", Facmodel.FacultyName);
                    cmd.Parameters.AddWithValue("@Gender", Facmodel.Gender);
                    cmd.Parameters.AddWithValue("@DOB", global.ConvertStringToInt(Facmodel.DOB));
                    cmd.Parameters.AddWithValue("@Phone", Facmodel.Phone);
                    cmd.Parameters.AddWithValue("@Email", Facmodel.Email);
                    cmd.Parameters.AddWithValue("@AadharNo", Facmodel.AadharNo);
                    cmd.Parameters.AddWithValue("@Address", Facmodel.Address);
                    cmd.Parameters.AddWithValue("@State", Facmodel.State);
                    cmd.Parameters.AddWithValue("@District", Facmodel.District);
                    cmd.Parameters.AddWithValue("@PinCode", Facmodel.PinCode);
                    cmd.Parameters.AddWithValue("@MaxQualification", Facmodel.MaxQualification);
                    cmd.Parameters.AddWithValue("@Department", Facmodel.Department);
                    cmd.Parameters.AddWithValue("@TotalExp", Facmodel.TotalExp);
                    cmd.Parameters.AddWithValue("@Password", Facmodel.Password);
                    cmd.Parameters.AddWithValue("@Operation", Facmodel.opsection);
                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@FacultyIDReturn", SqlDbType.VarChar, 100);
                    cmd.Parameters["@FacultyIDReturn"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);
                    if (ERRORCODE == 0)
                    {
                        string Facultyid = cmd.Parameters["@FacultyIDReturn"].Value.ToString();
                        var SaveFolderPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Faculty/" + Facultyid);
                        if (!Directory.Exists(SaveFolderPath))
                        {
                            Directory.CreateDirectory(SaveFolderPath);
                        }

                        if (Facmodel.FacPicture != null)
                        {
                            string Image1 = Facultyid + "_ProfileImage" + Path.GetExtension(Facmodel.FacPicture.FileName);
                            string DocImagePath = Path.Combine(SaveFolderPath, Image1);
                            Facmodel.FacPicture.SaveAs(DocImagePath);

                            string Folderpath = "/Faculty/" + Facultyid + "/" + Image1;

                            SqlCommand ImgCmd = new SqlCommand("UPDATE [TBL_FacultyMaster] SET [FProfilePic] = @FProfilePic WHERE [FacultyID] = @FacultyID", con);
                            ImgCmd.Parameters.AddWithValue("@FProfilePic", Folderpath);
                            ImgCmd.Parameters.AddWithValue("@FacultyID", Facultyid);
                            ImgCmd.ExecuteNonQuery();
                        }
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
                ex.Message.ToString();
                return false;
            }
        }

        public FacultyModel GetfacultyByID(string FacId)
        {
            FacultyModel FacModel = new FacultyModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string qry = "SELECT FacultyID,FranchaiseCode,FacultyName,Gender,DOB,Phone,Email,AadharNo,Address,State,District,PinCode,MaxQualification,Department,TotalExp,Password,FProfilePic FROM TBL_FacultyMaster where FacultyID = @FacultyID";
                    SqlDataAdapter sda = new SqlDataAdapter(qry, con);
                    sda.SelectCommand.Parameters.AddWithValue("@FacultyID", FacId);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        FacModel.FacultyID = dt.Rows[0]["FacultyID"].ToString();
                        FacModel.FranchaiseCode = dt.Rows[0]["FranchaiseCode"].ToString();
                        FacModel.FacultyName = dt.Rows[0]["FacultyName"].ToString();
                        FacModel.Gender = dt.Rows[0]["Gender"].ToString();
                        FacModel.DOB = global.ConvertIntToString(Convert.ToInt32(dt.Rows[0]["DOB"].ToString()));
                        FacModel.Phone = dt.Rows[0]["Phone"].ToString();
                        FacModel.Email = dt.Rows[0]["Email"].ToString();
                        FacModel.AadharNo = dt.Rows[0]["AadharNo"].ToString();
                        FacModel.Address = dt.Rows[0]["Address"].ToString();
                        FacModel.State = dt.Rows[0]["State"].ToString();
                        FacModel.District = dt.Rows[0]["District"].ToString();
                        FacModel.PinCode = dt.Rows[0]["PinCode"].ToString();
                        FacModel.MaxQualification = dt.Rows[0]["MaxQualification"].ToString();
                        FacModel.Department = dt.Rows[0]["Department"].ToString();
                        FacModel.TotalExp = dt.Rows[0]["TotalExp"].ToString();
                        FacModel.Password = dt.Rows[0]["Password"].ToString();
                        FacModel.FacPicturePath = dt.Rows[0]["FProfilePic"].ToString();
                    }
                }
                return FacModel;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return new FacultyModel();
            }
        }


        public Dictionary<string, string> RemoveFaculty(string facultyID)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM TBL_FacultyMaster WHERE FacultyID = @FacultyID", con);
                    cmd.Parameters.AddWithValue("@FacultyID", facultyID);

                    int Status = cmd.ExecuteNonQuery();

                    if (Status > 0)
                    {
                        Result.Add("status", "0");
                        Result.Add("msg", "faculty details removed successfully");
                    }
                    else
                    {
                        Result.Add("status", "1");
                        Result.Add("msg", "Failed to remove faculty details");
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to remove faculty details");
                return Result;
            }
        }


    }
}
using AccuFintech.Models;
using AccuFintech.DataAccess;
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
    public class UploadCertificateDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public UploadCertificateDal()
        {
            global = new GlobalFunction();
        }

        public List<DropdownModel> GetStudentList(string Course, string Franchaise)
        {
            List<DropdownModel> StudentLists = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT StudentID, Studentname FROM TBL_Student WHERE Course = @Course AND Franchaise = @Franchaise", con);
                    da.SelectCommand.Parameters.AddWithValue("@Franchaise", Franchaise);
                    da.SelectCommand.Parameters.AddWithValue("@Course", Course);
                    
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["StudentID"].ToString();
                            DM.Value = dr["Studentname"].ToString() + "-" + dr["StudentID"].ToString();
                            StudentLists.Add(DM);
                        }
                    }
                }
                return StudentLists;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return StudentLists;
            }
        }

        public Dictionary<string, string> UploadDocuments(UploadCertificateModel UM)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                var SaveFolderPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Student/" + UM.Student);
                if (!Directory.Exists(SaveFolderPath))
                {
                    Directory.CreateDirectory(SaveFolderPath);
                }

                if (UM.Certificate != null)
                {
                    string Image1 = UM.Student + "_Certificate" + Path.GetExtension(UM.Certificate.FileName);
                    string DocImagePath = Path.Combine(SaveFolderPath, Image1);
                    UM.Certificate.SaveAs(DocImagePath);

                    UM.StrCertificate = "/Student/" + UM.Student + "/" + Image1;
                }

                if (UM.Marksheet != null)
                {
                    string Image2 = UM.Student + "_Marksheet" + Path.GetExtension(UM.Marksheet.FileName);
                    string DocImagePath = Path.Combine(SaveFolderPath, Image2);
                    UM.Marksheet.SaveAs(DocImagePath);

                    UM.StrMarksheet = "/Student/" + UM.Student + "/" + Image2;
                }

                UM = global.NullFilter<UploadCertificateModel>(UM);
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_UploadDocuments", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", UM.Student);
                    cmd.Parameters.AddWithValue("@FranchaiseID", UM.Franchaise);
                    cmd.Parameters.AddWithValue("@CourseID", UM.Course);
                    cmd.Parameters.AddWithValue("@Certificate", UM.StrCertificate);
                    cmd.Parameters.AddWithValue("@Marksheet", UM.StrMarksheet);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);
                    if (ERRORCODE == 0)
                    {
                        Result.Add("status", "0");
                        Result.Add("msg", "Document uploaded successfully");
                    }
                    else
                    {
                        Result.Add("status", "1");
                        Result.Add("msg", "Failed to upload documents");
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to upload documents");
                return Result;
            }
        }

        public Dictionary<string, string> GetUploadedDocuments(string Student, string Course, string Franchaise)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("SELECT Certificate, Marksheet FROM TBL_Documents WHERE Student = @Stduent AND Franchaise = @Franchaise AND Course = @Course", con);
                    da.SelectCommand.Parameters.AddWithValue("@Stduent", Student);
                    da.SelectCommand.Parameters.AddWithValue("@Franchaise", Franchaise);
                    da.SelectCommand.Parameters.AddWithValue("@Course", Course);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        Result.Add("Certificate", tbl.Rows[0]["Certificate"].ToString());
                        Result.Add("Marksheet", tbl.Rows[0]["Marksheet"].ToString());
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("Certificate", "");
                Result.Add("Marksheet", "");
                return Result;
            }
        }
    }
}
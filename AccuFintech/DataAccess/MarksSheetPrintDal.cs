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
    public class MarksSheetPrintDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public MarksSheetPrintDal()
        {
            global = new GlobalFunction();
        }

        public List<DropdownModel> GetStudentListForMarkshhetPrint()
        {
            List<DropdownModel> MarksSheetList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT DISTINCT S.StudentID, S.Studentname FROM TBL_StudentMarks ST
                                                                LEFT JOIN TBL_Student S ON ST.StudentID = S.StudentID", con);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["StudentID"].ToString();
                            DM.Value = dr["Studentname"].ToString() + "-" + dr["StudentID"].ToString();
                            MarksSheetList.Add(DM);
                        }
                    }
                }
                return MarksSheetList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return MarksSheetList;
            }
        }

        public List<DropdownModel> GetCourseList(string StudentID)
        {
            List<DropdownModel> CourseList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT C.Programcode, C.Coursename FROM TBL_Student S
                                                                INNER JOIN TBL_CourseMaster C ON S.Course = C.Programcode WHERE S.StudentID = @StudentID", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["Programcode"].ToString();
                            DM.Value = dr["Coursename"].ToString();
                            CourseList.Add(DM);
                        }
                    }
                }
                return CourseList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CourseList;
            }
        }

        public List<MarksheetDetails> GetMarksheetDetails(string StudentID)
        {
            List<MarksheetDetails> MarksheetDetails = new List<Models.MarksheetDetails>();
            if (HttpContext.Current.Session["_UserType"].ToString().ToUpper() == "STUDENT")
            {
                StudentID = HttpContext.Current.Session["_UserId"].ToString();
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_MarksheetPrint", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            MarksheetDetails MD = new MarksheetDetails();
                            MD.StudentID = dr["StudentID"].ToString();
                            MD.Studentname = dr["Studentname"].ToString();
                            MD.Gurdian = dr["Gurdian"].ToString();
                            MD.DOB = dr["DOB"].ToString();
                            MD.Theory = dr["Theory"].ToString();
                            MD.Practical = dr["Practical"].ToString();
                            MD.MarksObtained = dr["MarksObtained"].ToString();
                            MD.Subject = dr["Subject"].ToString();
                            MD.Coursename = dr["Coursename"].ToString();
                            MD.FranchaiseID = dr["FranchaiseID"].ToString();
                            MD.Center = dr["Center"].ToString();
                            MD.Address = dr["Address"].ToString();
                            MD.FullMarks = dr["FullMarks"].ToString();
                            MD.Passmarks = dr["Passmarks"].ToString();
                            MD.TotalMarksobtained = dr["TotalMarksobtained"].ToString();
                            MD.PercentageMarks = dr["PercentageMarks"].ToString();
                            MarksheetDetails.Add(MD);
                        }
                    }
                }
                return MarksheetDetails;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return MarksheetDetails;
            }
        }

        public List<CertificatePrintDetails> GetCertificatePrintDetails(string StudentID)
        {
            List<CertificatePrintDetails> CertificatePrintList = new List<CertificatePrintDetails>();
            if (HttpContext.Current.Session["_UserType"].ToString().ToUpper() == "STUDENT")
            {
                StudentID = HttpContext.Current.Session["_UserId"].ToString();
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_CertificatePrintDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        var SaveFolderPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                        foreach (DataRow dr in tbl.Rows)
                        {
                            CertificatePrintDetails CP = new CertificatePrintDetails();
                            CP.StudentID = dr["StudentID"].ToString();
                            CP.Studentname = dr["Studentname"].ToString();
                            CP.Gurdian = dr["Gurdian"].ToString();
                            CP.StartingMonth = dr["StartingMonth"].ToString();
                            CP.EndingMonth = dr["EndingMonth"].ToString();
                            CP.FranchaiseID = dr["FranchaiseID"].ToString();
                            CP.Center = dr["Center"].ToString();
                            CP.Address = dr["Address"].ToString();
                            CP.DateOfIssue = dr["DateOfIssue"].ToString();
                            CP.Coursename = dr["Coursename"].ToString();

                            string ImagePath = SaveFolderPath + "/" + dr["StudentPicture"].ToString();
                            byte[] imageArray = System.IO.File.ReadAllBytes(ImagePath);
                            CP.StudentPicture = Convert.ToBase64String(imageArray);

                            CertificatePrintList.Add(CP);
                        }
                    }
                }
                return CertificatePrintList;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CertificatePrintList;
            }
        }

        public List<IcardModel> GetICardDetails(string StudentID)
        {
            List<IcardModel> ICardDetails = new List<IcardModel>();
            if (HttpContext.Current.Session["_UserType"].ToString().ToUpper() == "STUDENT")
            {
                StudentID = HttpContext.Current.Session["_UserId"].ToString();
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("USP_StudentIcardDetails", con);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        var SaveFolderPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
                        foreach (DataRow dr in tbl.Rows)
                        {
                            IcardModel IM = new IcardModel();
                            IM.StudentID = dr["StudentID"].ToString();
                            IM.Studentname = dr["Studentname"].ToString();
                            IM.DOB = dr["DOB"].ToString();
                            IM.JoiningDate = dr["JoiningDate"].ToString();
                            IM.Examdate = dr["Examdate"].ToString();
                            IM.Course = dr["Course"].ToString();
                            IM.FranchiseAddress = dr["FranchiseAddress"].ToString();


                            string ImagePath = SaveFolderPath + "/" + dr["StudentPicture"].ToString();
                            byte[] imageArray = System.IO.File.ReadAllBytes(ImagePath);
                            IM.StudentPicture = Convert.ToBase64String(imageArray);

                            string ImagePath2 = SaveFolderPath + "/" + dr["StudentSignature"].ToString();
                            byte[] imageArray2 = System.IO.File.ReadAllBytes(ImagePath2);
                            IM.StudentSignature = Convert.ToBase64String(imageArray2);

                            ICardDetails.Add(IM);
                        }
                    }
                }
                return ICardDetails;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return ICardDetails;
            }
        }
    }
}
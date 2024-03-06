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
    public class StudentRegistrationDal
    {
        string ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public GlobalFunction global = null;
        public StudentRegistrationDal()
        {
            global = new GlobalFunction();
        }
        public bool AddOrUpdateStudent(StudentModel SM)
        {
            bool UpdateStatus = false;
            try
            {
                SM = global.NullFilter<StudentModel>(SM);
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("USP_AddOrUpdateStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@StudentID", SM.StudentID);
                    cmd.Parameters.AddWithValue("@Studentname", SM.Studentname);
                    cmd.Parameters.AddWithValue("@Gurdian", SM.Gurdian);
                    cmd.Parameters.AddWithValue("@Age", SM.Age);
                    cmd.Parameters.AddWithValue("@Email", SM.Email);
                    cmd.Parameters.AddWithValue("@Phone", SM.Phone);
                    cmd.Parameters.AddWithValue("@DOB", global.ConvertStringToInt(SM.DOB));
                    cmd.Parameters.AddWithValue("@Gender", SM.Gender);
                    cmd.Parameters.AddWithValue("@Aadharno", SM.Aadharno);
                    cmd.Parameters.AddWithValue("@Address", SM.Address);
                    cmd.Parameters.AddWithValue("@District", SM.District);
                    cmd.Parameters.AddWithValue("@State", SM.State);
                    cmd.Parameters.AddWithValue("@Pincode", SM.Pincode);
                    cmd.Parameters.AddWithValue("@Course", SM.Course);
                    cmd.Parameters.AddWithValue("@MaritalStatus", SM.MartialStatus);
                    cmd.Parameters.AddWithValue("@StudentPicture", "");
                    cmd.Parameters.AddWithValue("@StudentIdProof", "");
                    cmd.Parameters.AddWithValue("@AcademicQualification", SM.AcademicQualification);
                    cmd.Parameters.AddWithValue("@Password", SM.Password);
                    cmd.Parameters.AddWithValue("@Franchaise", SM.Franchaise);
                    cmd.Parameters.AddWithValue("@JoiningDate", global.ConvertStringToInt(SM.Joinindate));
                    cmd.Parameters.AddWithValue("@Husband", SM.Husband);
                    cmd.Parameters.AddWithValue("@AltPhone", SM.AltPhoneno);
                    cmd.Parameters.AddWithValue("@Religion", SM.Religion);
                    cmd.Parameters.AddWithValue("@Nationality", SM.Nationality);
                    cmd.Parameters.AddWithValue("@Medium", SM.Medium);
                    cmd.Parameters.AddWithValue("@IsPhysicallyChallange", SM.IsPhysicallyChallanged);
                    cmd.Parameters.AddWithValue("@BatchID", SM.Batch);
                    cmd.Parameters.AddWithValue("@PayType", SM.PayType);
                    cmd.Parameters.AddWithValue("@CourseFees", SM.CFees);
                    cmd.Parameters.AddWithValue("@PaidFees", SM.RegAmt);
                    cmd.Parameters.AddWithValue("@opsection", SM.opsection);

                    cmd.Parameters.Add("@ERRORCODE", SqlDbType.Int);
                    cmd.Parameters["@ERRORCODE"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@StudentIDReturn", SqlDbType.VarChar, 100);
                    cmd.Parameters["@StudentIDReturn"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    int ERRORCODE = Convert.ToInt32(cmd.Parameters["@ERRORCODE"].Value);

                    if (ERRORCODE == 0)
                    {
                        string StudentID = cmd.Parameters["@StudentIDReturn"].Value.ToString();

                        var SaveFolderPath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "Student/" + StudentID);
                        if (!Directory.Exists(SaveFolderPath))
                        {
                            Directory.CreateDirectory(SaveFolderPath);
                        }

                        if (SM.StuPicture != null)
                        {
                            string Image1 = StudentID + "_ProfileImage" + Path.GetExtension(SM.StuPicture.FileName);
                            string DocImagePath = Path.Combine(SaveFolderPath, Image1);
                            SM.StuPicture.SaveAs(DocImagePath);

                            string Folderpath = "/Student/" + StudentID + "/" + Image1;

                            SqlCommand ImgCmd = new SqlCommand("UPDATE [TBL_Student] SET [StudentPicture] = @StudentPicture WHERE [StudentID] = @StudentID", con);
                            ImgCmd.Parameters.AddWithValue("@StudentPicture", Folderpath);
                            ImgCmd.Parameters.AddWithValue("@StudentID", StudentID);
                            ImgCmd.ExecuteNonQuery();
                        }
                        if (SM.StuIDPicture != null)
                        {
                            string Image2 = StudentID + "_IdProofImage" + Path.GetExtension(SM.StuIDPicture.FileName);
                            string DocImagePath = Path.Combine(SaveFolderPath, Image2);
                            SM.StuIDPicture.SaveAs(DocImagePath);

                            string Folderpath = "/Student/" + StudentID + "/" + Image2;

                            SqlCommand ImgCmd = new SqlCommand("UPDATE [TBL_Student] SET [StudentIDProof] = @StudentIDProof WHERE [StudentID] = @StudentID", con);
                            ImgCmd.Parameters.AddWithValue("@StudentIDProof", Folderpath);
                            ImgCmd.Parameters.AddWithValue("@StudentID", StudentID);
                            ImgCmd.ExecuteNonQuery();
                        }
                        if (SM.StuSignPicture != null)
                        {
                            string Image3 = StudentID + "_SignatureImage" + Path.GetExtension(SM.StuSignPicture.FileName);
                            string DocImagePath = Path.Combine(SaveFolderPath, Image3);
                            SM.StuSignPicture.SaveAs(DocImagePath);

                            string Folderpath = "/Student/" + StudentID + "/" + Image3;

                            SqlCommand ImgCmd = new SqlCommand("UPDATE [TBL_Student] SET [StudentSignature] = @StudentSign WHERE [StudentID] = @StudentID", con);
                            ImgCmd.Parameters.AddWithValue("@StudentSign", Folderpath);
                            ImgCmd.Parameters.AddWithValue("@StudentID", StudentID);
                            ImgCmd.ExecuteNonQuery();
                        }
                        UpdateStatus = true;
                    }
                    else
                    {
                        UpdateStatus = false;
                    }
                }
                return UpdateStatus;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return UpdateStatus;
            }
        }

        public List<StudentList> GetStudentsCourseWise(string CourseID)
        {
            string UserType = HttpContext.Current.Session["_UserType"].ToString();
            string UserID = HttpContext.Current.Session["_UserId"].ToString();
            List<StudentList> StudentLists = new List<StudentList>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter();
                    if (UserType.ToUpper() == "ADMINISTRATOR")
                    {
                        da = new SqlDataAdapter(@"SELECT StudentID, Studentname, Gurdian, S.Phone, Coursename, Examdate, joiningDate, ExamRegdate, F.Name As Franchaise FROM TBL_Student S
                                                                LEFT JOIN TBL_FranchaiseMaster F ON F.FranchaiseCode = S.Franchaise
                                                                LEFT JOIN TBL_CourseMaster C ON S.Course = C.Programcode WHERE Course = @Course", con);
                    }
                    else
                    {
                        da = new SqlDataAdapter(@"SELECT StudentID, Studentname, Gurdian, S.Phone, Coursename, Examdate, joiningDate, ExamRegdate, F.Name As Franchaise FROM TBL_Student S
                                                    LEFT JOIN TBL_FranchaiseMaster F ON F.FranchaiseCode = S.Franchaise
                                                    LEFT JOIN TBL_CourseMaster C ON S.Course = C.Programcode WHERE Course = @Course AND Franchaise = @Franchaise", con);
                        da.SelectCommand.Parameters.AddWithValue("@Franchaise", UserID);
                    }
                    da.SelectCommand.Parameters.AddWithValue("@Course", CourseID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            StudentList SL = new StudentList();
                            SL.StudentID = dr["StudentID"].ToString();
                            SL.Studentname = dr["Studentname"].ToString();
                            SL.Gurdian = dr["Gurdian"].ToString();
                            SL.Phone = dr["Phone"].ToString();
                            SL.Course = dr["Coursename"].ToString();
                            SL.Franchaise = dr["Franchaise"].ToString();
                            SL.Examdate = global.ConvertIntToString(Convert.ToInt32(dr["Examdate"]));
                            SL.JoiningDate = global.ConvertIntToString(Convert.ToInt32(dr["joiningDate"]));
                            SL.ExamRegDate = global.ConvertIntToString(Convert.ToInt32(dr["ExamRegdate"]));
                            StudentLists.Add(SL);
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

        public StudentModel GetStudentDetails(string StudentID)
        {
            StudentModel SM = new StudentModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT StudentID, Studentname, Gurdian, Age, Email, Phone, DOB, Gender, 
                                                                MaritalStatus, Aadharno, Address, District,State, Pincode, Course, Husband,
                                                                AcademicQualification, StudentPicture, StudentIDproof,StudentSignature, Password, JoiningDate, AltPhone, Medium, Religion, Nationality, IsPhysicallyChallange,
                                                                Examregdate, Examdate, Franchaise, AltPhone FROM TBL_Student WHERE StudentID = @StudentID", con);
                    da.SelectCommand.Parameters.AddWithValue("@StudentID", StudentID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        SM.StudentID = tbl.Rows[0]["StudentID"].ToString();
                        SM.Studentname = tbl.Rows[0]["Studentname"].ToString();
                        SM.Gurdian = tbl.Rows[0]["Gurdian"].ToString();
                        SM.Age = tbl.Rows[0]["Age"].ToString();
                        SM.Email = tbl.Rows[0]["Email"].ToString();
                        SM.Phone = tbl.Rows[0]["Phone"].ToString();
                        SM.DOB = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["DOB"]));
                        SM.Gender = tbl.Rows[0]["Gender"].ToString();
                        SM.MartialStatus = tbl.Rows[0]["MaritalStatus"].ToString();
                        SM.Aadharno = tbl.Rows[0]["Aadharno"].ToString();
                        SM.Address = tbl.Rows[0]["Address"].ToString();
                        SM.District = tbl.Rows[0]["District"].ToString();
                        SM.State = tbl.Rows[0]["State"].ToString();
                        SM.Pincode = tbl.Rows[0]["Pincode"].ToString();
                        SM.Course = tbl.Rows[0]["Course"].ToString();
                        SM.AcademicQualification = tbl.Rows[0]["AcademicQualification"].ToString();
                        SM.StudentPicture = tbl.Rows[0]["StudentPicture"].ToString();
                        SM.StudentIDPicture = tbl.Rows[0]["StudentIDproof"].ToString();
                        SM.StudentSignPicture = tbl.Rows[0]["StudentSignature"].ToString();
                        SM.Husband = tbl.Rows[0]["Husband"].ToString();
                        SM.Password = tbl.Rows[0]["Password"].ToString();
                        SM.Franchaise = tbl.Rows[0]["Franchaise"].ToString();
                        SM.Joinindate = global.ConvertIntToString(Convert.ToInt32(tbl.Rows[0]["JoiningDate"]));
                        SM.AltPhoneno = tbl.Rows[0]["AltPhone"].ToString();
                        SM.Religion = tbl.Rows[0]["Religion"].ToString();
                        SM.Nationality = tbl.Rows[0]["Nationality"].ToString();
                        SM.IsPhysicallyChallanged = tbl.Rows[0]["IsPhysicallyChallange"].ToString();
                        SM.Medium = tbl.Rows[0]["Medium"].ToString();
                    }
                }
                return SM;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return SM;
            }
        }

        public Dictionary<string, string> RemoveStudent(string StudentID)
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM TBL_Student WHERE StudentID = @StudentID", con);
                    cmd.Parameters.AddWithValue("@StudentID", StudentID);

                    int Status = cmd.ExecuteNonQuery();

                    if (Status > 0)
                    {
                        Result.Add("status", "0");
                        Result.Add("msg", "Student details removed successfully");
                    }
                    else
                    {
                        Result.Add("status", "1");
                        Result.Add("msg", "Failed to remove student details");
                    }
                }
                return Result;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                Result.Add("status", "1");
                Result.Add("msg", "Failed to remove student details");
                return Result;
            }
        }

        public StudentCourseModel GetCourseDetails(string CourseID, string CourseStartdate)
        {
            StudentCourseModel CM = new StudentCourseModel();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(@"SELECT Programcode,Fees,FORMAT(DATEADD(MONTH, CONVERT(INT,MonthDuration), CONVERT(Datetime,(CONVERT(char(8),@CourseStartdate)),112)),'dd/MM/yyyy') AS CourseEndDate
                                                            FROM TBL_CourseMaster WHERE Programcode = @Programcode", con);
                    da.SelectCommand.Parameters.AddWithValue("@Programcode", CourseID);
                    da.SelectCommand.Parameters.AddWithValue("@CourseStartdate", global.ConvertStringToInt(CourseStartdate));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        CM.Coursecode = tbl.Rows[0]["Programcode"].ToString();
                        CM.CourseEnddate = tbl.Rows[0]["CourseEndDate"].ToString();
                        CM.Fees = tbl.Rows[0]["Fees"].ToString();
                    }
                }
                return CM;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return CM;
            }
        }


        public List<DropdownModel> GetBatchesCourseWise(string CourseID)
        {
            List<DropdownModel> BatchList = new List<DropdownModel>();
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    if (con.State == ConnectionState.Open) { con.Close(); }
                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter("select BatchID , BatchName from TBL_BatchMaster where CourseID = @CourseID", con);
                    da.SelectCommand.Parameters.AddWithValue("@CourseID" , CourseID);
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    if (tbl.Rows.Count > 0)
                    {
                        foreach (DataRow dr in tbl.Rows)
                        {
                            DropdownModel DM = new DropdownModel();
                            DM.Key = dr["BatchID"].ToString();
                            DM.Value = dr["BatchName"].ToString();
                            BatchList.Add(DM);
                        }
                    }
                    return BatchList;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return BatchList;
            }
        }
    }
}
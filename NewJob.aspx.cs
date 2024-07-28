using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineJobPortal.Admin
{
    public partial class NewJob : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        string str = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        string query;

        public string Type { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["admin"] == null)
            {
                Response.Redirect("../User/Login.aspx");
            }
            Session["title"] = "Add Job";

            if (!IsPostBack)
            {
                FillData();
            }


        }
        private void FillData()
        {
            if (Request.QueryString["id"] != null)
            {
                con = new SqlConnection(str);
                query = "Select * from Jobs where JobId = '" + Request.QueryString["id"] + "' ";
                cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        txtJobTitle.Text = sdr["Title"].ToString();
                        txtNoOfPost.Text = sdr["NoOfPost"].ToString();
                        txtDescription.Text = sdr["Description"].ToString();
                        txtQualification.Text = sdr["Qualification"].ToString();
                        txtExperience.Text = sdr["Experience"].ToString();
                        txtSpecialization.Text = sdr["Specialization"].ToString();
                        txtLastDate.Text = Convert.ToDateTime(sdr["LastDateToApply"]).ToString("dd-MMMM-yyyy");
                        txtSalary.Text = sdr["Salary"].ToString();
                        ddlJobType.SelectedValue = sdr["JobType"].ToString();
                        txtCompany.Text = sdr["CompanyName"].ToString();
                        txtWebsite.Text = sdr["Website"].ToString();
                        txtEmail.Text = sdr["Email"].ToString();
                        txtAddress.Text = sdr["Address"].ToString();
                        ddlCountry.SelectedValue = sdr["Country"].ToString();
                        txtState.Text = sdr["State"].ToString();
                        btnAdd.Text = "Update";
                        linkBack.Visible = true;
                        Session["title"] = "Edit Job";

                    }
                }
                else
                {
                    lblMsg.Text = "Job Not Found..!";
                    lblMsg.CssClass = "alert alert-danger";
                }

                con.Close();
            }
        }




        protected void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                string Type, concatQuery,
                imagePath = string.Empty;
                bool isValidToExecute = false;
                con = new SqlConnection(str);
                if (Request.QueryString["id"] != null)
                {
                    if (fuCompanyLogo.HasFile)
                    {
                        if (Utils.IsValidExtension(fuCompanyLogo.FileName))
                        {
                            concatQuery = "CompanyLogo= @CompanyLogo";
                        }
                        else
                        {
                            concatQuery = string.Empty;
                        }
                    }
                    else
                    {
                        concatQuery = string.Empty;
                    }

                    query = @"Update Jobs set Title=@Title,NoOfPost=@NoOfPost,Description=@Description,Qualification=@Qualification,
                     Experience=@Experience,Specialization=@Specialization,LastDateToApply=@LastDateToApply,
                    Salary=@Salary,JobType=@JobType,CompanyName=@CompanyName," + concatQuery + @"Website=@Website,Email=@Email,
                    Address= @Address,Country=@Country,State=@State where JobId=@id";
                    Type = "updated";
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@id", Request.QueryString["id"].ToString());
                    if (fuCompanyLogo.HasFile)
                    {
                        if (Utils.IsValidExtension(fuCompanyLogo.FileName))
                        {
                            Guid obj = Guid.NewGuid();
                            imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);

                            cmd.Parameters.AddWithValue("@CompanyLogo", imagePath);
                            isValidToExecute = true;
                        }
                        else
                        {
                            lblMsg.Text = "Please select .jpg, .jpeg,.png file for Logo";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {

                        isValidToExecute = true;
                    }


                }


                else
                {
                    query = @"Insert into Jobs values(@Title,@NoOfPost,@Description,@Qualification,@Experience,@Specialization,@LastDateToApply,
                    @Salary,@JobType,@CompanyName,@CompanyLogo,@Website,@Email,@Address,@Country,@State,@CreateDate)";
                    Type = "saved";
                    DateTime time = DateTime.Now;
                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Title", txtJobTitle.Text.Trim());
                    cmd.Parameters.AddWithValue("@NoOfPost", txtNoOfPost.Text.Trim());
                    cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@Qualification", txtQualification.Text.Trim());
                    cmd.Parameters.AddWithValue("@Experience", txtExperience.Text.Trim());
                    cmd.Parameters.AddWithValue("@Specialization", txtSpecialization.Text.Trim());
                    cmd.Parameters.AddWithValue("@LastDateToApply", txtLastDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@Salary", txtSalary.Text.Trim());
                    cmd.Parameters.AddWithValue("@JobType", ddlJobType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CompanyName", txtCompany.Text.Trim());
                    cmd.Parameters.AddWithValue("@Website", txtWebsite.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Country", ddlCountry.SelectedValue);
                    cmd.Parameters.AddWithValue("@State", txtState.Text.Trim());
                    cmd.Parameters.AddWithValue("@CreateDate", time.ToString("yyyy-MM-dd HH:mm:ss"));
                    if (fuCompanyLogo.HasFile)
                    {
                        if (Utils.IsValidExtension(fuCompanyLogo.FileName))
                        {
                            Guid obj = Guid.NewGuid();
                            imagePath = "Images/" + obj.ToString() + fuCompanyLogo.FileName;
                            fuCompanyLogo.PostedFile.SaveAs(Server.MapPath("~/Images/") + obj.ToString() + fuCompanyLogo.FileName);

                            cmd.Parameters.AddWithValue("@CompanyLogo", imagePath);
                            isValidToExecute = true;
                        }
                        else
                        {
                            lblMsg.Text = "Please select .jpg, .jpeg,.png file for Logo";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CompanyLogo", imagePath);
                        isValidToExecute = true;
                    }
                }

                if (isValidToExecute)
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        lblMsg.Text = "Job " + Type + " successful..!";
                        lblMsg.CssClass = "alert alert-success";
                        Clear();
                    }
                    else
                    {
                        lblMsg.Text = "Cannot " + Type + " the records,please try after sometime..!";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }

            }


            catch (Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message + "');</script>");
            }
            finally
            {
                con.Close();
            }

        }

        private void Clear()
        {
            txtJobTitle.Text = String.Empty;
            txtNoOfPost.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtQualification.Text = String.Empty;
            txtExperience.Text = String.Empty;
            txtSpecialization.Text = String.Empty;
            txtLastDate.Text = String.Empty;
            txtSalary.Text = String.Empty;
            txtCompany.Text = String.Empty;
            txtWebsite.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtState.Text = String.Empty;
            ddlJobType.ClearSelection();
            ddlCountry.ClearSelection();

        }

        //private bool IsValidExtension(string fileName)
        //{	
        //    bool isValid = false;
        //    string[] fileExtension = { ".jpg", ".png", ".jpeg" };
        //    for (int i = 0; i <= fileExtension.Length - 1; i++)
        //    {
        //        if (fileName.Contains(fileExtension[i]))
        //        {
        //            isValid = true;
        //            break;

        //        }
        //    }
        //    return isValid;
        //}


    }
}

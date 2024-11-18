using System;
using System.Web;
using System.Web.UI;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Master.UserMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class UserMaster : System.Web.UI.Page
    {
        UserMasterManager userMasterManager = new UserMasterManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    //string userId = Session["UserId"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["USER_ID"]))
                    {
                        string userId = Request.QueryString["USER_ID"];
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        txtUserId.Attributes.Add("readOnly", "readOnly");
                        LoadUserMasterData(userId);
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        public void LoadUserMasterData(string userId)
        {
            try
            {
                UserMasterEntity userMaster = userMasterManager.LoadUserMasterData(userId);

                if (userMaster != null)
                {
                    txtUserId.Text = userMaster.UserId;
                    txtUserName.Text = userMaster.UserName;
                    txtPassword.Text = userMaster.UserPassword;
                    ddlActive.SelectedValue = userMaster.UserActiveYn;
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "UserMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string currentUser = Session["UserId"].ToString();
                UserMasterEntity userMaster = new UserMasterEntity
                {
                    UserId = txtUserId.Text.Trim(),
                    UserName = txtUserName.Text.Trim(),
                    UserPassword = txtPassword.Text.Trim(),
                    UserActiveYn = ddlActive.SelectedValue,
                    UserCrBy = currentUser
                };

                if (userMasterManager.SaveUserMaster(userMaster))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"UserMaster.aspx?USER_ID={userMaster.UserId}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
                
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "UserMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string currentUser = Session["UserId"].ToString();
                UserMasterEntity userMaster = new UserMasterEntity
                {
                    UserId = txtUserId.Text.Trim(),
                    UserName = txtUserName.Text.Trim(),
                    UserPassword = txtPassword.Text.Trim(),
                    UserActiveYn = ddlActive.SelectedValue,
                    UserUpBy = currentUser
                };

                if (userMasterManager.UpdateUserMaster(userMaster))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"UserMaster.aspx?USER_ID={userMaster.UserId}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "UserMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
        [System.Web.Services.WebMethod]
        public static bool CheckUserIdExists(string userId)
        {
            try
            {
               
                UserMasterManager userMasterManager = new UserMasterManager();
                return userMasterManager.IsUserIdExists(userId);
            }
            catch (Exception ex)
            {
              
                throw new Exception("An error occurred while checking if the user ID exists.", ex);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserMasterListing.aspx");
        }
    }
}

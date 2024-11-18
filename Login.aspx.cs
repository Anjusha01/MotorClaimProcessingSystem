using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Master.UserMaster;

namespace MotorClaimProcessingSystem
{
    public partial class Login : System.Web.UI.Page
    {
        UserMasterManager user = new UserMasterManager();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            LoginUser();
        }
        public void LoginUser()
        {
            string pUserId = txtUserName.Text;
            string pPassword = txtPassword.Text;
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            try
            {
                object obj = user.LoginUser(pUserId, pPassword);
                if (obj != null && obj.ToString() == "Y")
                {
                    Session["UserId"] = pUserId;
                    string redirectUrl = pUserId == "ADMIN001" ? "Master/AdminDasBoard.aspx" : "Transaction/UserDashBoard.aspx";

                    string successMsg = errorCodeMaster.FnSlctErrMsg("E007");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    Response.Redirect(redirectUrl);

                    //string successScript = $@"showSuccessAlert('Login', '{safeSuccessMsg}', '{redirectUrl}');";
                    //ClientScript.RegisterStartupScript(this.GetType(), "SuccessScript", successScript, true);
                }
                else
                {
                    string errorMsg = errorCodeMaster.FnSlctErrMsg("E009");
                    string safeErrorMsg = HttpUtility.JavaScriptStringEncode(errorMsg);

                    string errorScript = $@"showErrorAlert('Login', '{safeErrorMsg}', null);";
                    ClientScript.RegisterStartupScript(this.GetType(), "ErrorScript", errorScript, true);
                }
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E008");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);

                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}: {HttpUtility.JavaScriptStringEncode(ex.Message)}', null);";
                ClientScript.RegisterStartupScript(this.GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
    }
}
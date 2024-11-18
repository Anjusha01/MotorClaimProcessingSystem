using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.ErrorCodeMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class ErrorCodeMaster : System.Web.UI.Page
    {
        ErrorCodeMasterManager errorCodeMasterManager = new ErrorCodeMasterManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    string user = Session["UserId"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["ERR_CODE"]))
                    {
                        string code = Request.QueryString["ERR_CODE"];
                        btnUpdate.Visible = true;
                        btnSave.Visible = false;
                        txtErrorCode.Attributes.Add("readOnly", "readOnly");
                        LoadErrorCodeMasterData(code);
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        private void LoadErrorCodeMasterData(string code)
        {
            try
            {
                ErrorCodeMasterEntity errorCodeMaster = errorCodeMasterManager.LoadErrorCodeMasterData(code);

                if (errorCodeMaster != null)
                {
                    txtErrorCode.Text = errorCodeMaster.ErrCode;
                    txtErrorType.Text = errorCodeMaster.ErrType;
                    txtErrorDescription.Text = errorCodeMaster.ErrDesc;
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ErrorCodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserId"].ToString();
                ErrorCodeMasterEntity errorCodeMasterEntity = new ErrorCodeMasterEntity
                {
                    ErrCode = txtErrorCode.Text.Trim(),
                    ErrType = txtErrorType.Text.Trim(),
                    ErrDesc = txtErrorDescription.Text.Trim(),
                    ErrCrBy = user, 
                    ErrCrDt = DateTime.Now,
                    ErrUpBy = null,
                    ErrUpDt = null
                };

                bool isSaved = errorCodeMasterManager.SaveErrorCodeMaster(errorCodeMasterEntity);

                if (isSaved)
                {
                    
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"ErrorCodeMaster.aspx?ERR_CODE={errorCodeMasterEntity.ErrCode}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ErrorCodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserId"].ToString();
                ErrorCodeMasterEntity errorCodeMasterEntity = new ErrorCodeMasterEntity
                {
                    ErrCode = txtErrorCode.Text.Trim(),
                    ErrType = txtErrorType.Text.Trim(),
                    ErrDesc = txtErrorDescription.Text.Trim(),
                    ErrUpBy = user,
                    ErrUpDt = DateTime.Now
                };

                bool isUpdated = errorCodeMasterManager.UpdateErrorCodeMaster(errorCodeMasterEntity);

                if (isUpdated)
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"ErrorCodeMaster.aspx?ERR_CODE={errorCodeMasterEntity.ErrCode}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ErrorCodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ErrorCodeMasterListing.aspx");
        }

        [System.Web.Services.WebMethod]
        public static bool CheckErrorCodeExists(string code)
        {
            ErrorCodeMasterManager errorCodeMasterManager = new ErrorCodeMasterManager();

            return errorCodeMasterManager.IsErrorCodeExists(code);
        }

    }
}

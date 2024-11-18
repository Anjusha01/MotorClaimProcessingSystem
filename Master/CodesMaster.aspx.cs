using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.CodeMaster;
using BuisnessLogicLayer.Master.ErrorCodeMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class CodesMaster : System.Web.UI.Page
    {
        CodeMasterManager codeMasterManager = new CodeMasterManager();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (Session["UserId"] != null)
                {
                    string user = Session["UserId"].ToString();
                    if (!string.IsNullOrEmpty(Request.QueryString["CM_CODE"]) && !string.IsNullOrEmpty(Request.QueryString["CM_TYPE"]))
                    {
                        string code = Request.QueryString["CM_CODE"];
                        string type = Request.QueryString["CM_TYPE"];
                        btnUpdate.Visible = true;
                        txtCode.Attributes.Add("readOnly", "readOnly");
                        txtType.Attributes.Add("readOnly", "readOnly");
                        btnSave.Visible = false;
                        LoadCodeMasterData(code, type);
                    }
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
               
            }
        }
        public void LoadCodeMasterData(string code, string type)
        {
            try
            {
                CodeMasterEntity codeMaster = codeMasterManager.LoadCodeMasterData(code, type);

                if (codeMaster != null)
                {
                    txtCode.Text = codeMaster.CmCode;
                    txtType.Text = codeMaster.CmType;
                    txtDescription.Text = codeMaster.CmDesc;
                    txtValue.Text = codeMaster.CmValue.HasValue ? codeMaster.CmValue.ToString() : string.Empty;
                    txtParentCode.Text = codeMaster.CmParentCode;
                    ddlActive.SelectedValue = codeMaster.CmActiveYn;
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "CodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserId"].ToString();
                CodeMasterEntity codeMaster = new CodeMasterEntity
                {
                    CmCode = txtCode.Text.Trim(),
                    CmType = txtType.Text.Trim(),
                    CmDesc = txtDescription.Text.Trim(),
                    CmValue = string.IsNullOrEmpty(txtValue.Text) ? (decimal?)null : Convert.ToDecimal(txtValue.Text),
                    CmParentCode = txtParentCode.Text.Trim(),
                    CmCrBy = user, 
                    CmCrDt = DateTime.Now,
                    CmUpBy = null, 
                    CmUpDt = null, 
                    CmActiveYn = ddlActive.SelectedValue
                };

               
                bool isSaved = codeMasterManager.SaveCodeMaster(codeMaster);

                if (isSaved)
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"CodesMaster.aspx?CM_CODE={codeMaster.CmCode}&CM_TYPE={codeMaster.CmType}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "CodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("CodeMasterListing.aspx");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserId"].ToString();
                CodeMasterEntity codeMaster = new CodeMasterEntity
                {
                    CmCode = txtCode.Text.Trim(),
                    CmType = txtType.Text.Trim(),
                    CmDesc = txtDescription.Text.Trim(),
                    CmValue = string.IsNullOrEmpty(txtValue.Text) ? (decimal?)null : Convert.ToDecimal(txtValue.Text),
                    CmParentCode = txtParentCode.Text.Trim(),
                    CmUpBy = user, 
                    CmUpDt = DateTime.Now,
                    CmActiveYn = ddlActive.SelectedValue
                };

                bool isUpdated = codeMasterManager.UpdateCodeMaster(codeMaster);

                if (isUpdated)
                {

                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = $"CodesMaster.aspx?CM_CODE={codeMaster.CmCode}&CM_TYPE={codeMaster.CmType}";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "CodeMasterListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        [System.Web.Services.WebMethod]
        public static bool CheckCodeExists(string code)
        {
                CodeMasterManager codeMasterManager = new CodeMasterManager();
                return codeMasterManager.IsCodeExists(code);
        }
    }
}
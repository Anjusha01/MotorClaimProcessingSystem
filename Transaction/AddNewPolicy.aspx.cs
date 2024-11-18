using BuisnessLogicLayer.Master.CodeMaster;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Transaction.MotorPolicy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MotorClaimProcessingSystem
{
    public partial class AddNewPolicy : System.Web.UI.Page
    {
        CodeMasterManager codeMasterManager = new CodeMasterManager();
        MotorPolicyManager motorPolicyManager = new MotorPolicyManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    if (Session["RedirectData"] is Dictionary<string, object> redirectData)
                    {
                        string policyId = redirectData.ContainsKey("policyUid") ? redirectData["policyUid"].ToString() : null;
                        string actionType = redirectData.ContainsKey("Action") ? redirectData["Action"].ToString() : null;

                        if (!string.IsNullOrEmpty(policyId))
                        {
                            LoadPolicyData(policyId);
                            if (actionType == "Edit")
                            {
                                ShowUpdateAndAppBtn();
                            }
                            else if (actionType == "View")
                            {
                                ViewPolicyDetailsMode();
                            }
                        }
                    }
                    else
                    {
                        txtIssueDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    }
                    LoadVehicleMakes();
                    LoadVehicleBodyType();
                }

                txtNetLcPremium.Attributes.Add("readonly", "readonly");
                txtVatFcAmount.Attributes.Add("readonly", "readonly");
                txtVatLcAmount.Attributes.Add("readonly", "readonly");
                txtToDate.Attributes.Add("readonly", "readonly");
                txtPolicyNumber.Attributes.Add("readonly", "readonly");
                txtIssueDate.Attributes.Add("readonly", "readonly");
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }

        }

        public void ShowUpdateAndAppBtn()
        {
            btnUpdate.Visible = true;
            btnApprove.Visible = true;
            btnSavePolicy.Visible = false;
            btnSavePolicy.Enabled = false;
        }

        public void ViewPolicyDetailsMode()
        {
            txtPolicyNumber.Attributes.Add("readonly", "readonly");
            txtIssueDate.Attributes.Add("readonly", "readonly");
            txtAssuredName.Attributes.Add("readonly", "readonly");
            txtAssuredAddress.Attributes.Add("readonly", "readonly");
            txtAssuredAge.Attributes.Add("readonly", "readonly");
            txtAssuredMobile.Attributes.Add("readonly", "readonly");
            txtAssuredEmail.Attributes.Add("readonly", "readonly");
            txtChassisNumber.Attributes.Add("readonly", "readonly");
            txtRegistrationNumber.Attributes.Add("readonly", "readonly");
            txtFromDate.Attributes.Add("readonly", "readonly");
            txtToDate.Attributes.Add("readonly", "readonly");
            txtNetFcPremium.Attributes.Add("readonly", "readonly");
            txtNetLcPremium.Attributes.Add("readonly", "readonly");
            txtVatFcAmount.Attributes.Add("readonly", "readonly");
            txtVatLcAmount.Attributes.Add("readonly", "readonly");


            ddlVehicleMake.Enabled = false;
            ddlVehicleModel.Enabled = false;
            ddlVehicleBodyType.Enabled = false;


            btnSavePolicy.Visible = false;
            btnUpdate.Visible = false;
            btnApprove.Visible = false;
        }

        private void LoadPolicyData(string pPolicyUId)
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            try
            {
                MotorPolicyEntity policy = motorPolicyManager.LoadPolicy(Convert.ToInt32(pPolicyUId));
                if (policy != null)
                {

                    txtPolicyNumber.Text = policy.PolNo;
                    txtAssuredName.Text = policy.PolAssrName;
                    txtAssuredAddress.Text = policy.PolAssrAddress;
                    txtAssuredAge.Text = policy.PolAssrAge?.ToString("dd-MM-yyyy");
                    txtAssuredMobile.Text = policy.PolAssrMobile;
                    txtAssuredEmail.Text = policy.PolAssrEmail;
                    ddlVehicleMake.SelectedValue = policy.PolVehMake;
                    LoadVehicleModel(policy.PolVehMake);
                    ddlVehicleModel.SelectedValue = policy.PolVehModel;
                    ddlVehicleBodyType.SelectedValue = policy.PolVehBodyType;
                    txtChassisNumber.Text = policy.PolVehChassisNo;
                    txtRegistrationNumber.Text = policy.PolVehRegnNo;
                    txtIssueDate.Text = policy.PolIssDt?.ToString("dd-MM-yyyy");
                    txtFromDate.Text = policy.PolFmDt?.ToString("dd-MM-yyyy");
                    txtToDate.Text = policy.PolToDt?.ToString("dd-MM-yyyy");
                    txtNetFcPremium.Text = policy.PolNetFcPrem.ToString("F2");
                    txtNetLcPremium.Text = policy.PolNetLcPrem.ToString("F2");
                    txtVatFcAmount.Text = policy.PolVatFcAmt.ToString("F2");
                    txtVatLcAmount.Text = policy.PolVatLcAmt.ToString("F2");
                }
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);

            }
        }

        private void LoadVehicleMakes()
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            try
            {
                DataTable makeTable = codeMasterManager.LoadVehicleMake();
                ddlVehicleMake.DataSource = makeTable;
                ddlVehicleMake.DataTextField = "CM_DESC";
                ddlVehicleMake.DataValueField = "CM_CODE";
                ddlVehicleMake.DataBind();
                ddlVehicleMake.Items.Insert(0, new ListItem("Select Vehicle Make", ""));
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        private void LoadVehicleModel(string pMakeCode)
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            try
            {
                DataTable modelsTable = codeMasterManager.LoadVehicleModel(pMakeCode);
                ddlVehicleModel.DataSource = modelsTable;
                ddlVehicleModel.DataTextField = "CM_DESC";
                ddlVehicleModel.DataValueField = "CM_CODE";
                ddlVehicleModel.DataBind();
                ddlVehicleModel.Items.Insert(0, new ListItem("Select Vehicle Model", ""));
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        private void LoadVehicleBodyType()
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            try
            {
                DataTable bodyTypesTable = codeMasterManager.LoadVehicleBodyType();
                ddlVehicleBodyType.DataSource = bodyTypesTable;
                ddlVehicleBodyType.DataTextField = "CM_DESC";
                ddlVehicleBodyType.DataValueField = "CM_CODE";
                ddlVehicleBodyType.DataBind();
                ddlVehicleBodyType.Items.Insert(0, new ListItem("Select Vehicle Body Type", ""));
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnSavePolicy_Click(object sender, EventArgs e)
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
            string user = Session["UserId"].ToString();
            MotorPolicyEntity policy = new MotorPolicyEntity
            {
                PolAssrName = txtAssuredName.Text,
                PolAssrAddress = txtAssuredAddress.Text,
                PolAssrAge = DateTime.TryParse(txtAssuredAge.Text, out DateTime assuredAge) ? (DateTime?)assuredAge : null,
                PolAssrMobile = txtAssuredMobile.Text,
                PolAssrEmail = txtAssuredEmail.Text,
                PolVehMake = ddlVehicleMake.SelectedValue,
                PolVehModel = ddlVehicleModel.SelectedValue,
                PolVehBodyType = ddlVehicleBodyType.SelectedValue,
                PolVehChassisNo = txtChassisNumber.Text,
                PolVehRegnNo = txtRegistrationNumber.Text,
                PolIssDt = DateTime.Now,
                PolFmDt = DateTime.TryParse(txtFromDate.Text, out DateTime fromDate) ? (DateTime?)fromDate : null,
                PolToDt = DateTime.TryParse(txtToDate.Text, out DateTime toDate) ? (DateTime?)toDate : null,
                PolNetFcPrem = string.IsNullOrEmpty(txtNetFcPremium.Text) ? 0 : Convert.ToDecimal(txtNetFcPremium.Text),
                PolNetLcPrem = string.IsNullOrEmpty(txtNetLcPremium.Text) ? 0 : Convert.ToDecimal(txtNetLcPremium.Text),
                PolVatFcAmt = string.IsNullOrEmpty(txtVatFcAmount.Text) ? 0 : Convert.ToDecimal(txtVatFcAmount.Text),
                PolVatLcAmt = string.IsNullOrEmpty(txtVatLcAmount.Text) ? 0 : Convert.ToDecimal(txtVatLcAmount.Text),
                PolApprStatus = "P",
                PolApprDt = null,
                PolCrBy = user,
                PolCrDt = DateTime.Now,
                PolUpBy = null,
                PolUpDt = null,
            };

            try
            {
                int polUid = motorPolicyManager.SavePolicy(policy);
                if (polUid > 0)
                {
                    Dictionary<string, object> sessionData = new Dictionary<string, object>
                    {
                        { "policyUid", polUid },
                        { "Action", "Edit" }
                    };

                    Session["RedirectData"] = sessionData;

                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "AddNewPolicy.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "AddNewPolicy.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["RedirectData"] is Dictionary<string, object> redirectData)
                {
                    string policyId = redirectData.ContainsKey("policyUid") ? redirectData["policyUid"].ToString() : null;
                    string actionType = redirectData.ContainsKey("Action") ? redirectData["Action"].ToString() : null;

                    if (!string.IsNullOrEmpty(policyId))
                    {
                        string user = Session["UserId"].ToString(); 
                        MotorPolicyEntity policy = new MotorPolicyEntity
                        {
                            PolAssrName = txtAssuredName.Text,
                            PolAssrAddress = txtAssuredAddress.Text,
                            PolAssrMobile = txtAssuredMobile.Text,
                            PolAssrAge = DateTime.TryParse(txtAssuredAge.Text, out DateTime assuredAge) ? (DateTime?)assuredAge : null,
                            PolAssrEmail = txtAssuredEmail.Text,
                            PolVehMake = ddlVehicleMake.SelectedValue,
                            PolVehModel = ddlVehicleModel.SelectedValue,
                            PolVehBodyType = ddlVehicleBodyType.SelectedValue,
                            PolVehChassisNo = txtChassisNumber.Text,
                            PolVehRegnNo = txtRegistrationNumber.Text,
                            PolIssDt = DateTime.Now,
                            PolFmDt = DateTime.TryParse(txtFromDate.Text, out DateTime fromDate) ? (DateTime?)fromDate : null,
                            PolToDt = DateTime.TryParse(txtToDate.Text, out DateTime toDate) ? (DateTime?)toDate : null,
                            PolNetFcPrem = string.IsNullOrEmpty(txtNetFcPremium.Text) ? 0 : Convert.ToDecimal(txtNetFcPremium.Text),
                            PolNetLcPrem = string.IsNullOrEmpty(txtNetLcPremium.Text) ? 0 : Convert.ToDecimal(txtNetLcPremium.Text),
                            PolVatFcAmt = string.IsNullOrEmpty(txtVatFcAmount.Text) ? 0 : Convert.ToDecimal(txtVatFcAmount.Text),
                            PolVatLcAmt = string.IsNullOrEmpty(txtVatLcAmount.Text) ? 0 : Convert.ToDecimal(txtVatLcAmount.Text),
                            PolApprStatus = "P",
                            PolApprDt = null,
                            PolUpBy = user,
                            PolUid = Convert.ToInt32(policyId)
                        };

                        int rows = motorPolicyManager.UpdatePolicy(policy);
                        if (rows > 0)
                        {

                            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                            string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                            string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                            string redirectUrl = "AddNewPolicy.aspx";
                            string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                            ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                        }
                        else
                        {
                            lblMessage.Text = "No changes were made to the policy.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "AddNewPolicy.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);

            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {

            try
            {
                if (Session["RedirectData"] is Dictionary<string, object> redirectData)
                {
                    string policyId = redirectData.ContainsKey("policyUid") ? redirectData["policyUid"].ToString() : null;

                    if (!string.IsNullOrEmpty(policyId))
                    {
                        string user = Session["UserId"].ToString();

                        MotorPolicyEntity policy = new MotorPolicyEntity
                        {
                            PolIssDt = DateTime.Now,
                            PolApprDt = DateTime.Now,
                            PolApprBy = user,
                            PolUid = Convert.ToInt32(policyId)
                        };

                        if (motorPolicyManager.ApprovePolicy(policy))
                        {
                            Dictionary<string, object> sessionData = new Dictionary<string, object>
                            {
                                { "policyUid", policy.PolUid },
                                { "Action", "View" }
                            };

                            Session["RedirectData"] = sessionData;
                            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                            string successMsg = errorCodeMaster.FnSlctErrMsg("E006");
                            string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                            string redirectUrl = "AddNewPolicy.aspx";
                            string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                            ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);

                        }
                        else
                        {
                            lblMessage.Text = "Policy could not be approved.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "AddNewPolicy.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void ddlVehicleMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVehicleModel(ddlVehicleMake.SelectedValue);
        }

        protected void ddlVehicleModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadVehicleBodyType();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("RedirectData");
            Response.Redirect("PolicyListing.aspx");
        }

        [System.Web.Services.WebMethod]
        public static void ClearSession()
        {
            HttpContext.Current.Session["RedirectData"] = null;
        }

    }
}

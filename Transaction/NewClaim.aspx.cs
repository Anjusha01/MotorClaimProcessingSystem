using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.CodeMaster;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Transaction.MotorClaim;
using BuisnessLogicLayer.Transaction.MotorClaimEstimate;
using BuisnessLogicLayer.Transaction.MotorPolicy;

namespace MotorClaimProcessingSystem.Transaction
{
    public partial class NewClaim : System.Web.UI.Page
    {
        CodeMasterManager codeMasterManager = new CodeMasterManager();
        MotorClaimManager claim = new MotorClaimManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    if (Session["RedirectClaimData"] is Dictionary<string, object> redirectData)
                    {
                        string claimUid = redirectData.ContainsKey("claimUid") ? redirectData["claimUid"].ToString() : null;
                        string actionType = redirectData.ContainsKey("Action") ? redirectData["Action"].ToString() : null;

                        if (!string.IsNullOrEmpty(claimUid))
                        {
                            LoadClaimData(claimUid);
                            if (actionType == "Edit")
                            {
                                ShowUpdateAndAppBtn();
                                ddlPolNo.Attributes.Add("readonly", "readonly");
                                //txtPolicyNumber.Attributes.Add("readonly", "readonly");


                            }
                            else if (actionType == "View")
                            {
                                ViewMode();
                            }
                            LoadClaimEstimateGrid();
                        }
                    }
                    else
                    {
                        ltCaption.Visible = false;
                        txtIntimationDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                        txtRegistrationDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                    }

                    LoadDropDowns();
                }
                txtRegistrationDate.Attributes.Add("readonly", "readonly");
                txtClaimNumber.Attributes.Add("readonly", "readonly");
                txtVehicleMake.Attributes.Add("readonly", "readonly");
                txtVehicleModel.Attributes.Add("readonly", "readonly");
                txtVehicleBodyType.Attributes.Add("readonly", "readonly");
                txtRegistrationNumber.Attributes.Add("readonly", "readonly");
                txtChassisNumber.Attributes.Add("readonly", "readonly");
                btnUpdateEstimate.Attributes.Add("style", "display:none;");
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        public void ViewMode()
        {
            btnSaveClaim.Visible = false;
            btnUpdateClaim.Visible = false;
            ddlPolNo.Attributes.Add("readonly", "readonly");
            //txtPolicyNumber.Attributes.Add("readonly", "readonly");
            txtLossDate.Attributes.Add("readonly", "readonly");
            txtIntimationDate.Attributes.Add("readonly", "readonly");
            txtRegistrationDate.Attributes.Add("readonly", "readonly");
            txtPoliceRepNo.Attributes.Add("readonly", "readonly");
            txtDescription.Attributes.Add("readonly", "readonly");

            ddlLossType.Enabled = false;
            ddlFaultType.Enabled = false;
            ddlAccidentLocation.Enabled = false;

        }
        public void LoadClaimData(string claimId)
        {
            try
            {
                Session["ClaimId"] = claimId;
                //MotorClaimEntity claimEntity = claim.LoadClaimData(Convert.ToInt32(claimId));
                DataTable dt = claim.LoadClaimData(Convert.ToInt32(claimId));
                if (dt.Rows.Count == 0)
                {
                    return;
                }
                DataRow dataRow = dt.Rows[0];

                MotorClaimEntity claimEntity = new MotorClaimEntity();

                claimEntity.ClmPolUid = Convert.ToInt32(dataRow["CLM_POL_UID"]);
                claimEntity.ClmPolNo = dataRow["CLM_POL_NO"].ToString();
                claimEntity.ClmNo = dataRow["CLM_NO"].ToString();
                claimEntity.ClmLossDt = Convert.ToDateTime(dataRow["CLM_LOSS_DT"]);
                claimEntity.ClmLossType = dataRow["CLM_LOSS_TYPE"].ToString();
                claimEntity.ClmFaultType = dataRow["CLM_FAULT_TYPE"].ToString();
                claimEntity.ClmAccLoaction = dataRow["CLM_ACC_LOCATION"].ToString();
                claimEntity.ClmIntmDt = Convert.ToDateTime(dataRow["CLM_INTM_DT"]);
                claimEntity.ClmRegnDt = Convert.ToDateTime(dataRow["CLM_REGN_DT"]);
                claimEntity.ClmDesc = dataRow["CLM_DESC"]?.ToString();
                claimEntity.ClmPolRepNo = dataRow["CLM_POL_REP_NO"].ToString();
                claimEntity.ClmCrBy = dataRow["CLM_CR_BY"].ToString();
                claimEntity.ClmCrDt = Convert.ToDateTime(dataRow["CLM_CR_DT"]);
                claimEntity.ClmVehMake = dataRow["VEHICLE_MAKE_DESC"].ToString();
                claimEntity.ClmVehModel = dataRow["VEHICLE_MODEL_DESC"].ToString();
                claimEntity.ClmVehBodyType = dataRow["VEHICLE_BODY_TYPE_DESC"].ToString();
                claimEntity.ClmChassisNo = dataRow["CLM_CHASSIS_NO"].ToString();
                claimEntity.ClmRegnNo = dataRow["CLM_REGN_NO"].ToString();
                claimEntity.ClmUpBy = dataRow["CLM_UP_BY"].ToString();
                claimEntity.ClmUpDt = dataRow.Field<DateTime?>("CLM_UP_DT");
                claimEntity.ClmStatus = dataRow["CLM_STATUS"].ToString();
                DateTime policyStartDate = Convert.ToDateTime(dataRow["POLICY_START_DATE"]);
                DateTime policyEndDate = Convert.ToDateTime(dataRow["POLICY_END_DATE"]);

                if (claimEntity != null)
                {
                    Session["PolicyId"] = claimEntity.ClmPolUid;
                    ddlPolNo.SelectedValue = claimEntity.ClmPolNo;
                    //txtPolicyNumber.Text = claimEntity.ClmPolNo;
                    txtClaimNumber.Text = claimEntity.ClmNo;
                    txtVehicleMake.Text = claimEntity.ClmVehMake;
                    txtVehicleModel.Text = claimEntity.ClmVehModel;
                    txtVehicleBodyType.Text = claimEntity.ClmVehBodyType;
                    txtChassisNumber.Text = claimEntity.ClmChassisNo;
                    txtRegistrationNumber.Text = claimEntity.ClmRegnNo;
                    txtLossDate.Text = claimEntity.ClmLossDt?.ToString("d");
                    ddlLossType.SelectedValue = claimEntity.ClmLossType;
                    ddlFaultType.SelectedValue = claimEntity.ClmFaultType;
                    ddlAccidentLocation.SelectedValue = claimEntity.ClmAccLoaction;
                    txtIntimationDate.Text = claimEntity.ClmIntmDt?.ToString("dd-MM-yyyy");
                    txtRegistrationDate.Text = claimEntity.ClmRegnDt?.ToString("dd-MM-yyyy");
                    txtPoliceRepNo.Text = claimEntity.ClmPolRepNo;
                    txtDescription.Text = claimEntity.ClmDesc;
                    txtPolicyStartDate.Value = policyStartDate.ToString("dd-MM-yyyy");
                    txtPolicyEndDate.Value = policyEndDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    lblMessage.Text = "Claim not found.";
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ClaimListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        public void ShowUpdateAndAppBtn()
        {
            btnSaveClaim.Visible = false;
            btnUpdateClaim.Visible = true;
            btnAddClaimEstimate.Visible = true;
            btnCloseClaim.Visible = true;
        }
        protected void btnSaveClaim_Click(object sender, EventArgs e)
        {
            try
            {

                string user = Session["UserId"].ToString();
                MotorClaimEntity claimEntity = new MotorClaimEntity
                {
                    ClmPolUid = Session["PolicyId"] != null ? Convert.ToInt32(Session["PolicyId"].ToString()) : 0,
                    ClmPolNo = string.IsNullOrEmpty(ddlPolNo.SelectedValue) ? string.Empty : ddlPolNo.SelectedValue,
                    ClmLossDt = !string.IsNullOrEmpty(txtLossDate.Text.Trim()) ? DateTime.Parse(txtLossDate.Text.Trim()) : (DateTime?)null,
                    ClmLossType = string.IsNullOrEmpty(ddlLossType.SelectedValue) ? string.Empty : ddlLossType.SelectedValue,
                    ClmFaultType = string.IsNullOrEmpty(ddlFaultType.SelectedValue) ? string.Empty : ddlFaultType.SelectedValue,
                    ClmAccLoaction = string.IsNullOrEmpty(ddlAccidentLocation.SelectedValue) ? string.Empty : ddlAccidentLocation.SelectedValue,
                    ClmIntmDt = !string.IsNullOrEmpty(txtIntimationDate.Text.Trim()) ? DateTime.Parse(txtIntimationDate.Text.Trim()) : (DateTime?)null,
                    ClmRegnDt = !string.IsNullOrEmpty(txtRegistrationDate.Text.Trim()) ? DateTime.Parse(txtRegistrationDate.Text.Trim()) : (DateTime?)null,
                    ClmDesc = string.IsNullOrEmpty(txtDescription.Text.Trim()) ? string.Empty : txtDescription.Text.Trim(),
                    ClmPolRepNo = string.IsNullOrEmpty(txtPoliceRepNo.Text.Trim()) ? string.Empty : txtPoliceRepNo.Text.Trim(),
                    ClmCrBy = string.IsNullOrEmpty(user) ? "Unknown" : user, // Assuming 'user' variable holds the current user, you can set a default value if it's null
                    ClmCrDt = DateTime.Now,
                    ClmVehMake = string.IsNullOrEmpty(txtVehicleMake.Text.Trim()) ? string.Empty : txtVehicleMake.Text.Trim(),
                    ClmVehModel = string.IsNullOrEmpty(txtVehicleModel.Text.Trim()) ? string.Empty : txtVehicleModel.Text.Trim(),
                    ClmVehBodyType = string.IsNullOrEmpty(txtVehicleBodyType.Text.Trim()) ? string.Empty : txtVehicleBodyType.Text.Trim(),
                    ClmChassisNo = string.IsNullOrEmpty(txtChassisNumber.Text.Trim()) ? string.Empty : txtChassisNumber.Text.Trim(),
                    ClmRegnNo = string.IsNullOrEmpty(txtRegistrationNumber.Text.Trim()) ? string.Empty : txtRegistrationNumber.Text.Trim()
                };



                int claimId = claim.SaveClaim(claimEntity);
                if (claimId > 0)
                {
                    Dictionary<string, object> sessionData = new Dictionary<string, object>
                    {
                        { "claimUid", claimId },
                        { "Action", "Edit" }
                    };
                    Session["RedirectClaimData"] = sessionData;
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
                else
                {
                    lblMessage.Text = "Failed to save claim. Please try again.";
                }

            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }

        }

        //protected void txtPolicyNumber_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //string policyNo = txtPolicyNumber.Text.Trim();
        //        DataSet ds = claim.LoadPolicyDetails(policyNo);

        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            lblMessage.Text = "";
        //            DataRow dataRow = ds.Tables[0].Rows[0];
        //            DateTime polStartDate = Convert.ToDateTime(dataRow["POL_FM_DT"]);
        //            DateTime polEndDate = Convert.ToDateTime(dataRow["POL_TO_DT"]);
        //            Session["PolicyId"] = Convert.ToInt32(dataRow["POL_UID"]);
        //            txtVehicleMake.Text = dataRow["POL_VEH_MAKE"].ToString();
        //            txtVehicleModel.Text = dataRow["POL_VEH_MODEL"].ToString();
        //            txtVehicleBodyType.Text = dataRow["POL_VEH_BODY_TYPE"].ToString();
        //            txtChassisNumber.Text = dataRow["POL_VEH_CHASSIS_NO"].ToString();
        //            txtRegistrationNumber.Text = dataRow["POL_VEH_REGN_NO"].ToString();
        //            txtPolicyStartDate.Value = polStartDate.ToString("dd-MM-yyyy");
        //            txtPolicyEndDate.Value = polEndDate.ToString("dd-MM-yyyy");
        //        }
        //        else
        //        {
        //            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
        //            string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E012");
        //            string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
        //            string redirectLink = "NewClaim.aspx";
        //            string exceptionScript = $@"showInfoAlert('Info', '{safeExceptionMsg}', '{redirectLink}');";
        //            ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
        //        string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
        //        string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
        //        string redirectLink = "NewClaim.aspx";
        //        string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
        //        ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
        //    }
        //}

        public void LoadDropDowns()
        {
            try
            {
                DataTable lossTable = codeMasterManager.LoadVehicleLoss();
                ddlLossType.DataSource = lossTable;
                ddlLossType.DataTextField = "CM_DESC";
                ddlLossType.DataValueField = "CM_CODE";
                ddlLossType.DataBind();
                ddlLossType.Items.Insert(0, new ListItem("Select", ""));

                DataTable faultTable = codeMasterManager.LoadFaultType();
                ddlFaultType.DataSource = faultTable;
                ddlFaultType.DataTextField = "CM_DESC";
                ddlFaultType.DataValueField = "CM_CODE";
                ddlFaultType.DataBind();
                ddlFaultType.Items.Insert(0, new ListItem("Select", ""));

                DataTable accTable = codeMasterManager.LoadAccidentType();
                ddlAccidentLocation.DataSource = accTable;
                ddlAccidentLocation.DataTextField = "CM_DESC";
                ddlAccidentLocation.DataValueField = "CM_CODE";
                ddlAccidentLocation.DataBind();
                ddlAccidentLocation.Items.Insert(0, new ListItem("Select", ""));

                DataTable currTable = codeMasterManager.LoadCurrency();
                ddlEstimateCurrency.DataSource = currTable;
                ddlEstimateCurrency.DataTextField = "CM_DESC";
                ddlEstimateCurrency.DataValueField = "CM_CODE";
                ddlEstimateCurrency.DataBind();
                ddlEstimateCurrency.Items.Insert(0, new ListItem("Select", ""));

                MotorPolicyManager policyManager = new MotorPolicyManager();
                DataTable polTable = policyManager.LoadPolicyDropDown();
                ddlPolNo.DataSource = polTable;
                ddlPolNo.DataTextField = "POL_NO";
                ddlPolNo.DataValueField = "POL_NO";
                ddlPolNo.DataBind();
                ddlPolNo.Items.Insert(0, new ListItem("Select", ""));
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ClaimListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnUpdateClaim_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["RedirectClaimData"] is Dictionary<string, object> redirectData)
                {
                    string claimUid = redirectData.ContainsKey("claimUid") ? redirectData["claimUid"].ToString() : null;
                    if (!string.IsNullOrEmpty(claimUid))
                    {

                        int policyId;
                        MotorClaimEntity claimEntity = null;
                        string user = Session["UserId"].ToString();
                        if (Session["PolicyId"] != null && int.TryParse(Session["PolicyId"].ToString(), out policyId))
                        {

                            claimEntity = new MotorClaimEntity
                            {
                                ClmUid = Convert.ToInt32(claimUid),
                                ClmPolUid = policyId,
                                //ClmPolNo = txtPolicyNumber.Text.Trim(),
                                ClmPolNo = ddlPolNo.SelectedValue,
                                ClmLossDt = DateTime.TryParse(txtLossDate.Text.Trim(), out DateTime lossDate) ? lossDate : throw new FormatException("Loss Date is in an incorrect format."),
                                ClmLossType = ddlLossType.SelectedValue,
                                ClmFaultType = ddlFaultType.SelectedValue,
                                ClmAccLoaction = ddlAccidentLocation.SelectedValue,
                                ClmIntmDt = DateTime.TryParse(txtIntimationDate.Text.Trim(), out DateTime intimationDate) ? intimationDate : throw new FormatException("Intimation Date is in an incorrect format."),
                                ClmRegnDt = DateTime.TryParse(txtRegistrationDate.Text.Trim(), out DateTime registrationDate) ? registrationDate : throw new FormatException("Registration Date is in an incorrect format."),
                                ClmDesc = txtDescription.Text.Trim(),
                                ClmPolRepNo = txtPoliceRepNo.Text.Trim(),
                                ClmUpBy = user,
                                ClmUpDt = DateTime.Now,
                                ClmVehMake = txtVehicleMake.Text.Trim(),
                                ClmVehModel = txtVehicleModel.Text.Trim(),
                                ClmVehBodyType = txtVehicleBodyType.Text.Trim(),
                                ClmChassisNo = txtChassisNumber.Text.Trim(),
                                ClmRegnNo = txtRegistrationNumber.Text.Trim(),
                                ClmStatus = "O"
                            };
                        }
                        if (claimEntity != null)
                        {
                            bool isUpdated = claim.UpdateClaim(claimEntity);
                            if (isUpdated)
                            {
                                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                                string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                                string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                                string redirectUrl = "NewClaim.aspx";
                                string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                                ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                            }
                            else
                            {
                                lblMessage.Text = "Failed to update claim. Please try again.";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("RedirectClaimData");
            Response.Redirect("ClaimListing.aspx");
        }

        public void LoadClaimEstimateGrid()
        {
            try
            {
                int claimId = Convert.ToInt32(Session["ClaimId"].ToString());
                MotorClaimEstimateManager motorClaimEstimateManager = new MotorClaimEstimateManager();
                DataTable dt = motorClaimEstimateManager.LoadClaimEstimateData(claimId);
                gvClaimEstimates.DataSource = null;
                gvClaimEstimates.DataBind();
                if (dt != null && dt.Rows.Count > 0)
                {
                    gvClaimEstimates.DataSource = dt;
                    gvClaimEstimates.DataBind();
                }
                else
                {
                    gvClaimEstimates.Columns.Clear();
                    gvClaimEstimates.DataSource = new DataTable();

                    BoundField messageField = new BoundField
                    {
                        DataField = "Message",
                        HeaderText = "Info"
                    };
                    gvClaimEstimates.Columns.Add(messageField);

                    DataTable emptyDt = new DataTable();
                    emptyDt.Columns.Add("Message");

                    DataRow dr = emptyDt.NewRow();
                    dr["Message"] = "No Estimate found.";
                    emptyDt.Rows.Add(dr);

                    gvClaimEstimates.DataSource = emptyDt;
                    gvClaimEstimates.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "ClaimListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnSaveEstimate_Click(object sender, EventArgs e)
        {
            try
            {
                string user = Session["UserId"].ToString();
                int claimId = Convert.ToInt32(Session["ClaimId"].ToString());

                string currencyCode = ddlEstimateCurrency.SelectedValue;
                decimal estimatedFcAmount = decimal.Parse(txtEstimateFcAmount.Text);

                CurrencyExchange currencyService = new CurrencyExchange();
                var estimateResult = currencyService.CalculateEstimateAmounts(estimatedFcAmount, currencyCode);

                MotorClaimEstimateEntity estimateEntity = new MotorClaimEstimateEntity
                {
                    CeClmUid = claimId,
                    CeCurr = currencyCode,
                    CeCurrRate = estimateResult.EstLcAmt / estimatedFcAmount,
                    CeEstLcAmt = estimateResult.EstLcAmt,
                    CeEstFcAmt = estimatedFcAmount,
                    CeEstVatFcAmt = estimateResult.EstVatFcAmt,
                    CeEstValLcAmt = estimateResult.EstVatLcAmt,
                    CeStatus = "O",
                    CeCrBy = user,
                    CeCrDt = DateTime.Now
                };


                MotorClaimEstimateManager motorClaimEstimateManager = new MotorClaimEstimateManager();
                bool isSaved = motorClaimEstimateManager.AddClaimEstimate(estimateEntity);
                if (isSaved)
                {
                    ClearEstimateForm();
                    LoadClaimEstimateGrid();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#addEstimateModal').modal('hide');", true);
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E001");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }




        protected void btnUpdateEstimate_Click(object sender, EventArgs e)
        {

            string user = Session["UserId"].ToString();
            int claimId = Convert.ToInt32(Session["ClaimId"].ToString());

            string ceUid = hfCeUid.Value;

            int estimateId = Convert.ToInt32(ceUid);
            string currencyCode = ddlEstimateCurrency.SelectedValue;
            decimal estimatedFcAmount = decimal.Parse(txtEstimateFcAmount.Text);
            CurrencyExchange currencyService = new CurrencyExchange();
            var estimateResult = currencyService.CalculateEstimateAmounts(estimatedFcAmount, currencyCode);

            MotorClaimEstimateEntity estimateEntity = new MotorClaimEstimateEntity
            {
                CeUid = estimateId,
                CeCurr = currencyCode,
                CeCurrRate = estimateResult.EstLcAmt / estimatedFcAmount,
                CeEstLcAmt = estimateResult.EstLcAmt,
                CeEstFcAmt = estimatedFcAmount,
                CeEstVatFcAmt = estimateResult.EstVatFcAmt,
                CeEstValLcAmt = estimateResult.EstVatLcAmt,
                CeStatus = "O",
                CeUpBy = user,
                CeUpDt = DateTime.Now
            };
            try
            {
                MotorClaimEstimateManager motorClaimEstimateManager = new MotorClaimEstimateManager();
                bool isUpdated = motorClaimEstimateManager.UpdateClaimEstimate(estimateEntity);
                if (isUpdated)
                {
                    ClearEstimateForm();
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E002");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                    LoadClaimEstimateGrid();
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal", "$('#addEstimateModal').modal('hide');", true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
        private void ClearEstimateForm()
        {
            txtEstimateFcAmount.Text = string.Empty;
            ddlEstimateCurrency.SelectedValue = string.Empty;
        }
        protected void gvClaimEstimates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ApproveEstimate")
            {
                string[] args = e.CommandArgument.ToString().Split(',');

                int ceUid = Convert.ToInt32(args[0]);
                string currency = args[1];
                decimal estimatedFcAmount = Convert.ToDecimal(args[2]);
                decimal estimatedLcAmount = Convert.ToDecimal(args[3]);
                decimal vatFcAmount = Convert.ToDecimal(args[4]);
                decimal vatLcAmount = Convert.ToDecimal(args[5]);

                ApproveClaim(ceUid, estimatedFcAmount, estimatedLcAmount, vatFcAmount, vatLcAmount);
                LoadClaimEstimateGrid();
            }
            else if (e.CommandName == "CloseEstimate")
            {
                int ceUid = Convert.ToInt32(e.CommandArgument.ToString());
                CloseEstimate(ceUid);
                LoadClaimEstimateGrid();
            }
            else if (e.CommandName == "EditEstimate")
            {
                string[] args = e.CommandArgument.ToString().Split(',');
                int ceUid = Convert.ToInt32(args[0]);

            }

        }
        private void ApproveClaim(int ceUid, decimal estimatedFcAmount, decimal estimatedLcAmount, decimal vatFcAmount, decimal vatLcAmount)
        {
            decimal paidFc = estimatedFcAmount + vatFcAmount;
            decimal paidLc = estimatedLcAmount + vatLcAmount;
            MotorClaimEstimateEntity estimateEntity = new MotorClaimEstimateEntity
            {
                CeUid = ceUid,
                CeStatus = "A",
                CePaidFcAmt = paidFc,
                CePaidLcAmt = paidLc
            };
            try
            {
                MotorClaimEstimateManager estimateManager = new MotorClaimEstimateManager();
                if (estimateManager.ApproveClaimEstimate(estimateEntity))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E006");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }


        private void CloseEstimate(int ceUid)
        {
            MotorClaimEstimateEntity estimateEntity = new MotorClaimEstimateEntity
            {
                CeUid = ceUid,
                CeStatus = "C",
            };
            try
            {
                MotorClaimEstimateManager estimateManager = new MotorClaimEstimateManager();
                if (estimateManager.CloseClaimEstimate(estimateEntity))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E011");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void gvClaimEstimates_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem != null)
                {
                    DataRowView dataRow = (DataRowView)e.Row.DataItem;
                    if (dataRow.DataView.Table.Columns.Contains("CE_STATUS"))
                    {
                        string claimStatus = dataRow["CE_STATUS"]?.ToString();

                        ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                        ImageButton btnApprove = (ImageButton)e.Row.FindControl("btnApprove");
                        ImageButton btnClose = (ImageButton)e.Row.FindControl("btnClose");

                        Image imgEstimateOpen = (Image)e.Row.FindControl("imgEstimateOpen");
                        Image imgEstimateApproved = (Image)e.Row.FindControl("imgEstimateApproved");
                        Image imgEstimateClosed = (Image)e.Row.FindControl("imgEstimateClosed");

                        if (claimStatus == "A")
                        {
                            imgEstimateApproved.Visible = true;
                            imgEstimateClosed.Visible = false;
                            imgEstimateOpen.Visible = false;

                            btnEdit.Visible = false;
                            btnApprove.Visible = false;
                            btnClose.Visible = false;
                        }
                        else if (claimStatus == "O")
                        {
                            imgEstimateApproved.Visible = false;
                            imgEstimateClosed.Visible = false;
                            imgEstimateOpen.Visible = true;

                            btnEdit.Visible = true;
                            btnApprove.Visible = true;
                            btnClose.Visible = true;
                        }
                        else if (claimStatus == "C")
                        {
                            imgEstimateApproved.Visible = false;
                            imgEstimateClosed.Visible = true;
                            imgEstimateOpen.Visible = false;

                            btnEdit.Visible = false;
                            btnApprove.Visible = false;
                            btnClose.Visible = false;
                        }
                    }

                }
            }
        }
        [System.Web.Services.WebMethod]
        public static void ClearSession()
        {
            HttpContext.Current.Session["RedirectClaimData"] = null;
        }

        protected void btnCloseClaim_Click(object sender, EventArgs e)
        {
            int claimUid = Convert.ToInt32(Session["ClaimId"].ToString());
            UpdateClaimStatusToClosed(claimUid);
        }

        private void UpdateClaimStatusToClosed(int claimUid)
        {
            try
            {
                MotorClaimEntity claimEntity = new MotorClaimEntity
                {
                    ClmUid = claimUid,
                    ClmStatus = "C"
                };
                MotorClaimManager claimManager = new MotorClaimManager();
                if (claimManager.CloseClaim(claimEntity))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E011");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "NewClaim.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
            }
            catch (Exception)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void ddlPolNo_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ddlPolNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string policyNo = ddlPolNo.SelectedValue;
                DataSet ds = claim.LoadPolicyDetails(policyNo);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lblMessage.Text = "";
                    DataRow dataRow = ds.Tables[0].Rows[0];
                    DateTime polStartDate = Convert.ToDateTime(dataRow["POL_FM_DT"]);
                    DateTime polEndDate = Convert.ToDateTime(dataRow["POL_TO_DT"]);
                    Session["PolicyId"] = Convert.ToInt32(dataRow["POL_UID"]);
                    txtVehicleMake.Text = dataRow["POL_VEH_MAKE"].ToString();
                    txtVehicleModel.Text = dataRow["POL_VEH_MODEL"].ToString();
                    txtVehicleBodyType.Text = dataRow["POL_VEH_BODY_TYPE"].ToString();
                    txtChassisNumber.Text = dataRow["POL_VEH_CHASSIS_NO"].ToString();
                    txtRegistrationNumber.Text = dataRow["POL_VEH_REGN_NO"].ToString();
                    txtPolicyStartDate.Value = polStartDate.ToString("dd-MM-yyyy");
                    txtPolicyEndDate.Value = polEndDate.ToString("dd-MM-yyyy");
                }
                else
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E012");
                    string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                    string redirectLink = "NewClaim.aspx";
                    string exceptionScript = $@"showInfoAlert('Info', '{safeExceptionMsg}', '{redirectLink}');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "NewClaim.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
    }
}
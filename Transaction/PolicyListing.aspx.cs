using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Transaction.MotorPolicy;

namespace MotorClaimProcessingSystem.Transaction
{
    public partial class PolicyListing : System.Web.UI.Page
    {
        MotorPolicyEntity motorPolicyEntity = new MotorPolicyEntity();
        MotorPolicyManager motorPolicyManager = new MotorPolicyManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    LoadPolicyData();
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }
        public void LoadPolicyData(string policyNo = "", string assuredName = "", string status = "")
        {
            try
            {

                DataTable dt = motorPolicyManager.LoadAllPolicy(policyNo, assuredName, status);
                gridViewPolicy.DataSource = null;
                gridViewPolicy.DataBind();


                if (dt != null && dt.Rows.Count > 0)
                {
                    gridViewPolicy.DataSource = dt;
                    gridViewPolicy.DataBind();
                }
                else
                {

                    //gridViewPolicy.Columns.Clear();
                    //gridViewPolicy.DataSource = new DataTable();

                    //BoundField messageField = new BoundField
                    //{
                    //    DataField = "Message",
                    //    HeaderText = "Info"
                    //};
                    //gridViewPolicy.Columns.Add(messageField);

                    //DataTable emptyDt = new DataTable();
                    //emptyDt.Columns.Add("Message");

                    //DataRow dr = emptyDt.NewRow();
                    //dr["Message"] = "No policies found.";
                    //emptyDt.Rows.Add(dr);

                    //gridViewPolicy.DataSource = emptyDt;
                    //gridViewPolicy.DataBind();
                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "UserDashBoard.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}:{HttpUtility.JavaScriptStringEncode(ex.Message)}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void gridViewPolicy_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    var dataItem = e.Row.DataItem as DataRowView;

                    if (dataItem != null && dataItem.Row.Table.Columns.Contains("POL_APPR_STATUS"))
                    {
                        var approvalStatusObj = dataItem["POL_APPR_STATUS"];
                        string approvalStatus = approvalStatusObj != DBNull.Value ? approvalStatusObj.ToString() : string.Empty;

                        ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                        ImageButton btnView = (ImageButton)e.Row.FindControl("btnView");
                        Image imgApproval = (Image)e.Row.FindControl("imageApproval");
                        Image imgPending = (Image)e.Row.FindControl("imagePending");
                        if (approvalStatus == "P")
                        {
                            imgPending.Visible = true;
                            imgApproval.Visible = false;
                            btnView.Visible = false;
                            btnEdit.Visible = true;
                        }
                        else
                        {
                            imgPending.Visible = false;
                            imgApproval.Visible = true;
                            btnView.Visible = true;
                            btnEdit.Visible = false;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "UserDashBoard.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void gridViewPolicy_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                // Implement delete logic here if required
            }
            catch (Exception ex)
            {
                // Log the exception
                lblMessage.Text = "An error occurred while deleting the policy: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        protected void btnAddPolicy_Click(object sender, EventArgs e)
        {
            try
            {
                Session.Remove("RedirectData");
                Response.Redirect("AddNewPolicy.aspx");
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }



        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btnAction = (ImageButton)sender;
                GridViewRow row = (GridViewRow)btnAction.NamingContainer;

                string policyUid = btnAction.CommandArgument;

                Dictionary<string, object> sessionData = new Dictionary<string, object>
                {
                    { "policyUid", policyUid },
                    { "Action", "Edit" }
                };

                Session["RedirectData"] = sessionData;
                Response.Redirect("AddNewPolicy.aspx");
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btnAction = (ImageButton)sender;
                GridViewRow row = (GridViewRow)btnAction.NamingContainer;

                string policyUid = btnAction.CommandArgument;

                Dictionary<string, object> sessionData = new Dictionary<string, object>
                {
                    { "policyUid", policyUid },
                    { "Action", "View" }
                };

                Session["RedirectData"] = sessionData;
                Response.Redirect("AddNewPolicy.aspx");
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        protected void gridViewPolicy_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewPolicy.PageIndex = e.NewPageIndex;
            LoadPolicyData();
        }

        protected void gridViewPolicy_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                foreach (Control control in e.Row.Cells[0].Controls)
                {
                    if (control is LinkButton linkButton)
                    {
                        linkButton.CssClass = "page-link";
                    }
                    else if (control is Label label)
                    {
                        label.CssClass = "selected-page";
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string policyNo = txtPolicyNumber.Text.Trim();
                string assuredName = txtAssuredName.Text.Trim();
                string status = ddlStatus.SelectedValue;
                LoadPolicyData(policyNo, assuredName, status);
            }
            catch (Exception ex)
            {
                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "PolicyListing.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}:{HttpUtility.JavaScriptStringEncode(ex.Message)}','{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }
    }
}

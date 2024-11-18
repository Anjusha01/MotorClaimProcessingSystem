using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Transaction.MotorClaim;

namespace MotorClaimProcessingSystem.Transaction
{
    public partial class ClaimListing : System.Web.UI.Page
    {
        MotorClaimManager claim = new MotorClaimManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] != null)
            {
                if (!IsPostBack)
                {
                    LoadAllClaims();
                }
            }
            else
            {
                Response.Redirect("../Login.aspx");
            }
        }

        public void LoadAllClaims(string claimNo = "", string policyNo = "", string status = "")
        {
            DataTable dt = claim.LoadAllClaims(claimNo, policyNo, status);
           
            gridViewClaim.DataSource = new DataTable();
            gridViewClaim.DataBind();

            if (dt != null && dt.Rows.Count > 0)
            {

                gridViewClaim.DataSource = null;
                gridViewClaim.DataSource = dt;
                gridViewClaim.DataBind();
            }
            else
            {

                //gridViewClaim.Columns.Clear();
                //gridViewClaim.DataSource = new DataTable();

                //BoundField messageField = new BoundField
                //{
                //    DataField = "Message",
                //    HeaderText = "Info"
                //};
                //gridViewClaim.Columns.Add(messageField);

                //DataTable emptyDt = new DataTable();
                //emptyDt.Columns.Add("Message");

                //DataRow dr = emptyDt.NewRow();
                //dr["Message"] = "No claims found.";
                //emptyDt.Rows.Add(dr);

                //gridViewClaim.DataSource = emptyDt;
                //gridViewClaim.DataBind();


            }
        }

        protected void btnAddClaim_Click(object sender, EventArgs e)
        {
            Session.Remove("RedirectClaimData");
            Response.Redirect("NewClaim.aspx");
        }

        protected void gridViewClaim_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as DataRowView;

                if (dataItem != null && dataItem.Row.Table.Columns.Contains("CLM_STATUS"))
                {
                    var claimStatus = dataItem["CLM_STATUS"];
                    string sStatus = claimStatus != DBNull.Value ? claimStatus.ToString() : string.Empty;

                    ImageButton btnEdit = (ImageButton)e.Row.FindControl("btnEdit");
                    ImageButton btnView = (ImageButton)e.Row.FindControl("btnView");
                    ImageButton btnClose = (ImageButton)e.Row.FindControl("btnClose");

                    Image imgClmStatusOpen = (Image)e.Row.FindControl("imgClmStatusOpen");
                    Image imgClmStatusClose = (Image)e.Row.FindControl("imgClmStatusClose");

                    if (sStatus == "O")
                    {
                        imgClmStatusClose.Visible = false;
                        imgClmStatusOpen.Visible = true;
                        btnEdit.Visible = true;
                        btnView.Visible = false;
                        btnClose.Visible = true;
                    }
                    else
                    {
                        imgClmStatusClose.Visible = true;
                        imgClmStatusOpen.Visible = false;
                        btnEdit.Visible = false;
                        btnView.Visible = true;
                        btnClose.Visible = false;
                    }
                }
            }
        }



        protected void btnClose_Click(object sender, EventArgs e)
        {
            ImageButton btnClose = (ImageButton)sender;
            int claimUid = Convert.ToInt32(btnClose.CommandArgument);
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
                    LoadAllClaims();
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E011");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "ClaimListing.aspx";
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

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            ImageButton btnAction = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btnAction.NamingContainer;

            string claimUid = btnAction.CommandArgument;
            Dictionary<string, object> sessionData = new Dictionary<string, object>
                {
                    { "claimUid", claimUid },
                    { "Action", "Edit" }
                };
            Session["RedirectClaimData"] = sessionData;
            Response.Redirect("NewClaim.aspx");
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            ImageButton btnAction = (ImageButton)sender;
            GridViewRow row = (GridViewRow)btnAction.NamingContainer;

            string claimUid = btnAction.CommandArgument;
            Dictionary<string, object> sessionData = new Dictionary<string, object>
                {
                    { "claimUid", claimUid },
                    { "Action", "View" }
                };
            Session["RedirectClaimData"] = sessionData;
            Response.Redirect("NewClaim.aspx");
        }

        protected void gridViewClaim_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewClaim.PageIndex = e.NewPageIndex;
            LoadAllClaims();
        }

        protected void gridViewClaim_RowCreated(object sender, GridViewRowEventArgs e)
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
            string claimNo = txtClaimNo.Text.Trim();
            string policyNo = txtPolicyNo.Text.Trim();
            string status = ddlStatus.SelectedValue;

            LoadAllClaims(claimNo, policyNo, status);
        }
    }
}

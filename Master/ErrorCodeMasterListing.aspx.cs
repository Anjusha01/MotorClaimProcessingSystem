using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.ErrorCodeMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class ErrorCodeMasterListing : System.Web.UI.Page
    {
        ErrorCodeMasterManager errorCodeMasterManager = new ErrorCodeMasterManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    BindErrorCodeGrid();
                    string user = Session["UserId"].ToString();
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }     
            }
        }

        private void BindErrorCodeGrid()
        {
            try
            {
                DataTable dt = errorCodeMasterManager.ViewAllErrorCodeMaster();
                gvErrorCodeMaster.DataSource = null;
                gvErrorCodeMaster.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvErrorCodeMaster.DataSource = dt;
                    gvErrorCodeMaster.DataBind();
                }
                else
                {
                    gvErrorCodeMaster.Columns.Clear();
                    gvErrorCodeMaster.DataSource = new DataTable();

                    BoundField messageField = new BoundField
                    {
                        DataField = "Message",
                        HeaderText = "Info"
                    };
                    gvErrorCodeMaster.Columns.Add(messageField);

                    DataTable emptyDt = new DataTable();
                    emptyDt.Columns.Add("Message");

                    DataRow dr = emptyDt.NewRow();
                    dr["Message"] = "No codes found.";
                    emptyDt.Rows.Add(dr);

                    gvErrorCodeMaster.DataSource = emptyDt;
                    gvErrorCodeMaster.DataBind();
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

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("ErrorCodeMaster.aspx");
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

        protected void gvErrorCodeMaster_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditErrorCode")
            {
                string code = e.CommandArgument.ToString();
                Response.Redirect("ErrorCodeMaster.aspx?ERR_CODE=" + code);
            }
        }

        protected void gvErrorCodeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gvErrorCodeMaster.Rows[e.RowIndex];
                string errorCode = (row.FindControl("lblErrorCode") as Label).Text;

                if (errorCodeMasterManager.DeleteErrorCodeData(errorCode))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E003");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "ErrorCodeMasterListing.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
                BindErrorCodeGrid();
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

    }


}

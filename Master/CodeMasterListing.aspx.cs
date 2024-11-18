using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.CodeMaster;
using BuisnessLogicLayer.Master.ErrorCodeMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class CodeMasterListing : System.Web.UI.Page
    {
        CodeMasterManager codeMasterManager = new CodeMasterManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    LoadCodeMasterData();
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        public void LoadCodeMasterData()
        {
            try
            {
                DataTable dt = codeMasterManager.ViewAllCodeMaster();

                gridViewCodeMaster.DataSource = null;
                gridViewCodeMaster.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridViewCodeMaster.DataSource = dt;
                    gridViewCodeMaster.DataBind();
                }
                else
                {
                    gridViewCodeMaster.Columns.Clear();
                    gridViewCodeMaster.DataSource = new DataTable();

                    BoundField messageField = new BoundField
                    {
                        DataField = "Message",
                        HeaderText = "Info"
                    };
                    gridViewCodeMaster.Columns.Add(messageField);

                    DataTable emptyDt = new DataTable();
                    emptyDt.Columns.Add("Message");

                    DataRow dr = emptyDt.NewRow();
                    dr["Message"] = "No codes found.";
                    emptyDt.Rows.Add(dr);

                    gridViewCodeMaster.DataSource = emptyDt;
                    gridViewCodeMaster.DataBind();
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

        protected void gridViewCodeMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as DataRowView;

                if (dataItem != null && dataItem.Row.Table.Columns.Contains("CM_ACTIVE_YN"))
                {
                    var activeStatus = dataItem["CM_ACTIVE_YN"];
                    string sStatus = activeStatus != DBNull.Value ? activeStatus.ToString() : string.Empty;

                    Image imgActiveYes = (Image)e.Row.FindControl("imgActiveYes");
                    Image imgActiveNo = (Image)e.Row.FindControl("imgActiveNo");

                    if (sStatus == "Y")
                    {
                        imgActiveYes.Visible = true;
                        imgActiveNo.Visible = false;
                    }
                    else
                    {
                        imgActiveYes.Visible = false;
                        imgActiveNo.Visible = true;
                    }
                }
            }
        }

        protected void gridViewCodeMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gridViewCodeMaster.Rows[e.RowIndex];
                string cmCode = (row.FindControl("lblCode") as Label).Text;

                if (codeMasterManager.DeleteCodeMasterData(cmCode))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E003");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "CodeMasterListing.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
                LoadCodeMasterData();
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

        protected void btnAddCode_Click(object sender, EventArgs e)
        {
            Response.Redirect("CodesMaster.aspx");
        }

        protected void btnAction_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btnAction = (ImageButton)sender;
                string[] args = btnAction.CommandArgument.Split(',');
                string code = args[0];
                string type = args[1];

                Response.Redirect("CodesMaster.aspx?CM_CODE=" + code + "&CM_TYPE=" + type);
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
    }
}

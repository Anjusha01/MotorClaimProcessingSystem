using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Master.UserMaster;

namespace MotorClaimProcessingSystem.Master
{
    public partial class UserMasterListing : System.Web.UI.Page
    {
        UserMasterManager userMasterManager = new UserMasterManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] != null)
                {
                    LoadUserMasterData();
                }
                else
                {
                    Response.Redirect("../Login.aspx");
                }
            }
        }

        public void LoadUserMasterData()
        {
            try
            {
                DataTable dt = userMasterManager.ViewAllUsers();

                gridViewUserMaster.DataSource = null;
                gridViewUserMaster.DataBind();

                if (dt != null && dt.Rows.Count > 0)
                {
                    gridViewUserMaster.DataSource = dt;
                    gridViewUserMaster.DataBind();
                }
                else
                {
                    gridViewUserMaster.Columns.Clear();
                    gridViewUserMaster.DataSource = new DataTable();

                    BoundField messageField = new BoundField
                    {
                        DataField = "Message",
                        HeaderText = "Info"
                    };
                    gridViewUserMaster.Columns.Add(messageField);

                    DataTable emptyDt = new DataTable();
                    emptyDt.Columns.Add("Message");

                    DataRow dr = emptyDt.NewRow();
                    dr["Message"] = "No codes found.";
                    emptyDt.Rows.Add(dr);

                    gridViewUserMaster.DataSource = emptyDt;
                    gridViewUserMaster.DataBind();
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

        protected void gridViewUserMaster_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = gridViewUserMaster.Rows[e.RowIndex];
                string userId = (row.FindControl("lblUserID") as Label).Text;
                if (userMasterManager.DeleteUser(userId))
                {
                    ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                    string successMsg = errorCodeMaster.FnSlctErrMsg("E003");
                    string safeSuccessMsg = HttpUtility.JavaScriptStringEncode(successMsg);
                    string redirectUrl = "UserMasterListing.aspx";
                    string script = $@"showSuccessAlert('Success', '{safeSuccessMsg}', '{redirectUrl}')";
                    ScriptManager.RegisterStartupScript(this, GetType(), "SuccessAlert", script, true);
                }
                LoadUserMasterData();
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

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserMaster.aspx");
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton btnEdit = (ImageButton)sender;
                string userId = btnEdit.CommandArgument;

                Response.Redirect("UserMaster.aspx?USER_ID=" + userId);
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

        protected void gridViewUserMaster_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var dataItem = e.Row.DataItem as DataRowView;

                if (dataItem != null && dataItem.Row.Table.Columns.Contains("USER_ACTIVE_YN"))
                {
                    var activeStatus = dataItem["USER_ACTIVE_YN"];
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
    }
}

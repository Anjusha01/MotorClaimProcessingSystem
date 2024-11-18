using BuisnessLogicLayer.Master.ErrorCodeMaster;
using BuisnessLogicLayer.Transaction.MotorClaimEstimate;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MotorClaimProcessingSystem
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                MotorClaimEstimateManager estimateManager = new MotorClaimEstimateManager();
                //DateTime startDate, endDate;
                DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
                DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

                if (startDate != null && endDate != null)
                {
                    DataTable dt = estimateManager.GetClaimEstimateReport(startDate, endDate);
                    ReportDocument reportDocument = new ReportDocument();
                    reportDocument.Load(Server.MapPath("ClaimStatusReport.rpt"));
                    reportDocument.SetDataSource(dt);
                    reportDocument.SetParameterValue("pStartDate", startDate.ToString("dd-MM-yyyy"));
                    reportDocument.SetParameterValue("pEndDate", endDate.ToString("dd-MM-yyyy"));

                    ExportToPdf(reportDocument);

                }
            }
            catch (Exception)
            {

                ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
                string exceptionMsg = errorCodeMaster.FnSlctErrMsg("E004");
                string safeExceptionMsg = HttpUtility.JavaScriptStringEncode(exceptionMsg);
                string redirectLink = "Report.aspx";
                string exceptionScript = $@"showErrorAlert('Error', '{safeExceptionMsg}', '{redirectLink}');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ExceptionScript", exceptionScript, true);
            }
        }

        private void ExportToPdf(ReportDocument reportDocument)
        {
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "UserInformation" + "Admin");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
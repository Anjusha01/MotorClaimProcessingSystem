using BuisnessLogicLayer.Transaction.MotorClaim;
using BuisnessLogicLayer.Transaction.MotorClaimEstimate;
using BuisnessLogicLayer.Transaction.MotorPolicy;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MotorClaimProcessingSystem.Transaction
{
    public partial class UserDashBoard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("../Login.aspx");
                return;
            }
            else
            {
                if (!IsPostBack)
                {
                    CountPolicy();
                }
            }
            
        }

        private void CountPolicy()
        {
            MotorPolicyManager motorPolicy = new MotorPolicyManager();
            MotorClaimManager claimManager = new MotorClaimManager();
            int pendingPolicyCount = motorPolicy.CountPolicy("P");
            int approvedPolicyCount = motorPolicy.CountPolicy("A");
            int totalPolicyCount = motorPolicy.CountPolicy();
            int totalClaim = claimManager.CountClaim();
            string script = $@"
                <script type='text/javascript'>
                    var pendingPolicyCount = {pendingPolicyCount};
                    var approvedPolicyCount = {approvedPolicyCount};
                    var totalPolicyCount = {totalPolicyCount};
                    var totalClaim = {totalClaim};
                </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "PolicyCounts", script);
        }

        [WebMethod]
        public static object GetClaimsData(int year)
        {
            var result = new
            {
                monthlyClaims = new int[12],
                totalClaims = new int[3]  // [Pending, Approved, Total]
            };

            try
            {
                MotorClaimManager claimManager = new MotorClaimManager();
                MotorClaimEstimateManager estimateManager = new MotorClaimEstimateManager();
                DataTable dtClaims = claimManager.GetClaimsByYear(year);

                foreach (DataRow row in dtClaims.Rows)
                {
                    int month = Convert.ToInt32(row["Month"]);
                    int claimsCount = Convert.ToInt32(row["ClaimsCount"]);
                    result.monthlyClaims[month - 1] = claimsCount;  // Populate monthly claims
                }

                // Assuming you have methods to get total counts for pending, approved, and total claims
                result.totalClaims[0] = estimateManager.CountClmEsiByStatus("O");  // Pending claims count
                result.totalClaims[1] = estimateManager.CountClmEsiByStatus("C");  // Approved claims count
                result.totalClaims[2] = estimateManager.CountClmEsiByStatus("A");  // Approved claims count
                //result.totalClaims[3] = estimateManager.CountClmEsi();  // Approved claims count
                //result.totalClaims[2] = claimManager.CountClaim();  // Total claims count
            }
            catch (Exception ex)
            {
                // Handle exception
                throw;
            }

            return result;
        }

    }
}
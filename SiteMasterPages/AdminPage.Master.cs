using BuisnessLogicLayer.Master.ErrorCodeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MotorClaimProcessingSystem.SiteMasterPages
{
    public partial class AdminPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            ErrorCodeMasterManager errorCodeMaster = new ErrorCodeMasterManager();
           
            Session.Clear();
            Session.Abandon();
            Response.Redirect("/Login.aspx");
        }
    }
}
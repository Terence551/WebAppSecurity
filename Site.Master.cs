using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppSecurity
{
    public partial class SiteMaster : MasterPage
    {
        // session fixation
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Logged"] != null)
            {
                @out.Visible = true;
                @in.Visible = false;
                register.Visible = false;
            }
            else
            {
                @in.Visible = true;
                register.Visible = true;
                @out.Visible = false;
            }
        }
        // logout
        protected void logOut(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
        }


    }
}
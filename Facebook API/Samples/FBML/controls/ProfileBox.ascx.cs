using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Configuration;
using Facebook.Web;

namespace FBMLSample.controls
{
    public partial class ProfileBox : System.Web.UI.UserControl, IRenderableFBML<string>
    {
        protected string link;
        private string suffix = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            link = "<a href=\"http://apps.facebook.com/" + suffix + "/ requirelogin=1>Visit Smiley</a>";
        }

        #region IRenderableFBML<string> Members

        public void PopulateData(string data)
        {
            suffix = data;
        }

        #endregion
    }
}
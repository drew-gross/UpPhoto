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
using Facebook;

namespace FBToolkit.Samples.FBMLLoginControlSample
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var uid = login.Api.Users.GetLoggedInUser();
            fbml.Text = string.Format("<fb:profile-pic uid=\"{0}\" linked=\"true\" />",uid.ToString());
        }
    }
}

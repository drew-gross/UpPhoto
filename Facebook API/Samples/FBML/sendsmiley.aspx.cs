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

namespace FBMLSample
{
    public partial class sendsmiley : BasePage
    {
        protected string multiFeedHandler = string.Empty;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Master.RequireLogin = true;
            Master.SetSelectedTab("sendsmiley");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            multiFeedHandler = string.Format("{0}handlers/multiFeedHandler.aspx", Master.callback);
            grid.Text = BuildEmoticonGrid(getOtherMoods(), Master.callback, Master.suffix,false);

        }
    }
}

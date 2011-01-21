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
    public partial class PublisherHeader : System.Web.UI.UserControl, IRenderableFBML<string>
    {
        protected string callback = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region IRenderableFBML<string> Members

        public void PopulateData(string data)
        {
            callback = data;
        }

        #endregion
    }
}
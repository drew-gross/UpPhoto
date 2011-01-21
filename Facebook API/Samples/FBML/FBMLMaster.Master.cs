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
using Facebook.Web;
using System.Collections.Generic;
using Facebook.Schema;
namespace FBMLSample
{
    public partial class FBMLMaster : Facebook.Web.CanvasFBMLMasterPage
    {
        protected string selected = string.Empty;
        public FBMLMaster() : base()
        {
            this.RequiredPermissions = new List<Enums.ExtendedPermissions>() { Enums.ExtendedPermissions.publish_stream };

        }
        public void SetSelectedTab(string page)
        {
            selected = page;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(Request.Params["fb_sig_in_profile_tab"]))
            {
                header.Visible = false;
                css.Text = FBMLControlRenderer.RenderFBML("~/controls/FBMLCSS.ascx");
            }
            else
            {
                css.Text = string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}css/page.css?id={1}\" />", callback, cssVersion);
                js.Text = FBMLControlRenderer.RenderFBML("~/controls/FBMLJS.ascx");
                // js.Text = string.Format("<script src=\"{0}js/base.js?id={1}\" />", callback, jsVersion);

            }
        }
    }
}

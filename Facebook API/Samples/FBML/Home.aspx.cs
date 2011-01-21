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
using System.Collections.Generic;
using Facebook.Schema;
using FBMLSample.controls;
using System.Web.Configuration;
using Facebook.Web;
using Facebook.Session;

namespace FBMLSample
{
    public partial class Home : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Master.RequireLogin = true;
            Master.SetSelectedTab("home");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master.RequiredPermissions != null && Master.RequiredPermissions.Count > 0)
            {
                l.Text = "I need some permissions:";
                var aa = this.Master.Api.Session.CheckPermissions();
                l.Text += aa;
            }
            else
            {
                l.Text = "I need no permissions:";
            }
            // You need to set info or profile box in order for the buttons on the page to show up.
            // Don't set them every time.
            var pref = this.Master.Api.Data.GetUserPreference(1);
            if (pref != "set")
            {
                this.Master.Api.Profile.SetInfo("My Smiles", 5, getSampleInfo(), this.Master.Api.Session.UserId);
                this.Master.Api.Profile.SetFBML(this.Master.Api.Session.UserId, null, getUserProfileBox(), null); 
                this.Master.Api.Data.SetUserPreference(1, "set");
            }
        }

		private List<info_field> getSampleInfo()
		{
			var callback = WebConfigurationManager.AppSettings["Callback"];
			var options = new List<info_item>();
			options.Add(new info_item { label = "Happy", image = callback + "images/smile0.jpg", sublabel = "", description = "The original and still undefeated.", link = "http://apps.facebook.com/" + Master.suffix + "/smile.aspx?smile=1" });
			options.Add(new info_item { label = "Indifferent", image = callback + "images/smile1.jpg", sublabel = "", description = "meh....", link = "http://apps.facebook.com/" + Master.suffix + "/smile.aspx?smile=2" });
			options.Add(new info_item { label = "Sad", image = callback + "images/smile2.jpg", sublabel = "", description = "Oh my god! you killed my dog!", link = "http://apps.facebook.com/" + Master.suffix + "/smile.aspx?smile=3" });
			options.Add(new info_item { label = "Cool", image = callback + "images/smile3.jpg", sublabel = "", description = "Yeah. whatever", link = "http://apps.facebook.com/" + Master.suffix + "/smile.aspx?smile=4" });

			var field = new info_field();
			field.field = "Good Smilies";
			field.items = new info_fieldItems();
			field.items.info_item = options;

			return new List<info_field> { field };
		}

        private string getUserProfileBox()
        {
            return FBMLControlRenderer.RenderFBML<string>(string.Format("~/controls/{0}.ascx", typeof(ProfileBox).Name), Master.suffix);
        }
    }
}

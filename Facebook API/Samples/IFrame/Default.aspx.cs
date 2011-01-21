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
using Facebook.Schema;

namespace IFrameSample
{
    public partial class Default : System.Web.UI.Page
    {
		private const string SCRIPT_BLOCK_NAME = "dynamicScript";

        protected void Page_Load(object sender, EventArgs e)
        {
			if (IsPostBack)
			{
				if (Master.Api.Users.HasAppPermission(Enums.ExtendedPermissions.email))
				{
					SendThankYouEmail();
				}

				Response.Redirect("ThankYou.aspx");
			}
			else
			{
				if (Master.Api.Users.HasAppPermission(Enums.ExtendedPermissions.email))
				{
					emailPermissionPanel.Visible = false;
				}

				CreateScript();
			}
        }

		private void SendThankYouEmail()
		{
			var subject = "Thank you for telling us your favorite color";
			var body = "Thank you for telling us what your favorite color is. We hope you have enjoyed using this application. Encourage your friends to tell us their favorite color as well!";
			this.Master.Api.Notifications.SendEmail(this.Master.Api.Session.UserId.ToString(), subject, body, string.Empty);
		}

		private void CreateScript()
		{
			var saveColorScript =
					@"<script>

					function saveColor(color) {
						document.getElementById('" + colorInput.ClientID + @"').value = color;
					}

					function submitForm() {
						document.getElementById('" + form.ClientID + @"').submit();
					}

					</script>";

			if (!ClientScript.IsClientScriptBlockRegistered(SCRIPT_BLOCK_NAME))
			{
				ClientScript.RegisterClientScriptBlock(this.GetType(), SCRIPT_BLOCK_NAME, saveColorScript);
			}
		}
    }
}

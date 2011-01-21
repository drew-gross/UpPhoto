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
using System.Text;
using System.Collections.Generic;

namespace FBMLSample
{
    public partial class newsmiley : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Master.RequireLogin = true;
            Master.SetSelectedTab("newsmiley");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var moods = getMoods();
            lblSetCount.Text = BuildSetCount();
            lblfeedHandler.Text = BuildFeedHandlerForm();
            lblEmoticonGrid.Text = BuildEmoticonGrid(moods,Master.callback,Master.suffix);

        }
        private string BuildFeedHandlerForm()
        {
            return string.Format("<form fbtype=\"feedStory\" action=\"{0}handlers/feedHandler.aspx\">", Master.callback);
        }
        private string BuildSetCount()
        {
            var setCount = !string.IsNullOrEmpty(this.Master.Api.Data.GetUserPreference(2)) ? int.Parse(this.Master.Api.Data.GetUserPreference(2)): 0;
            if (setCount > 1) 
            {
                return string.Format("<h3>You've set your mood {0} times in the past.</h3>", setCount);
            }
            if (setCount > 0) 
            {
                return string.Format("<h3>You've set your mood {0} time in the past.</h3>", setCount);
            }
            else
            {
                return "You've never set your mood before.";
            }
        }
    }
}

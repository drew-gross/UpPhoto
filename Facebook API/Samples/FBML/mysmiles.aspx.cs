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
using Facebook.Utility;
using System.Text;
using System.Web.Configuration;

namespace FBMLSample
{
    public partial class mysmiles : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Master.RequireLogin = true;
            Master.SetSelectedTab("mysmiles");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO, Check Form POST to see if it is inside of a tab or not...
            List<string> moodList = JSONHelper.ConvertFromJSONArray(this.Master.Api.Data.GetUserPreference(0));
                
            lblMoodList.Text = buildMoodListFBML(moodList);
            if(!string.IsNullOrEmpty(lblMoodList.Text))
            {
                announce.Text = string.Format("We are pleased to announce that <fb:name useyou=\"false\" uid=\"{0}\"/> has been feeling:",Master.Api.Session.UserId);
            }
            else
            {
                announce.Text = string.Format("<fb:name useyou=\"false\" uid=\"{0}\"/> has never told us their feelings.",Master.Api.Session.UserId);
            }

            lblLink.Text = "<a href=\"http://apps.facebook.com/" + Master.suffix + "\">Check out Smiley</a>";
        }

        private string buildMoodListFBML(List<string> moodList)
        {
            int len = 3;
            var fbml = new StringBuilder();
            var moods = getMoods();
            if(moodList.Count < len)
                len = moodList.Count;
            for(int i=0;i<len;i++)
            {
                int pos = int.Parse(moodList[i]);
                fbml.Append(string.Format("<a class=\"box\" href=\"smile.aspx?smile={0}\"><div class=\"smiley\">{1}</div><div>{2}</div></a>",i.ToString(),moods.ElementAt(pos).Value,moods.ElementAt(pos).Key));
            }
            return fbml.ToString();
        }
    }
}

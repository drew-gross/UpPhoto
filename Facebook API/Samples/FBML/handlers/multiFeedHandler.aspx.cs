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
using Facebook.Web;
using System.Web.Configuration;
using Facebook;
using Facebook.Schema;
using Facebook.Utility;

namespace FBMLSample.handlers
{
    public partial class multiFeedHandler : CanvasFBMLBasePage
    {
        protected string json = string.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Params["picked"]))
            {
                var picked = int.Parse(Request.Params["picked"]);
                var basePage = new BasePage();
                var moods = basePage.getOtherMoods();
                var canvas = string.Format("http://apps.facebook.com/{0}/mysmiles.aspx", suffix);
                var templateData = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "emote", moods.ElementAt(picked).Value }, { "emoteaction", moods.ElementAt(picked).Key } });
                var feed = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "template_id", basePage.FeedTemplate2.ToString() }, { "template_data", templateData } });
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "feed", feed }, { "next", canvas } });
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "method", "multiFeedStory" }, { "content", content } });
                json = data;
            }
            else
            {
                throw new Exception("no smile picked");
            }
        }
    }
}

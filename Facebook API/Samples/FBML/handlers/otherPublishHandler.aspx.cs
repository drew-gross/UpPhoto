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
    public partial class otherPublishHandler : CanvasFBMLBasePage
    {
        protected string json = string.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var basePage = new BasePage();
            var moods = basePage.getOtherMoods();
            if (!string.IsNullOrEmpty(Request.Params["method"]) && Request.Params["method"] == "publisher_getFeedStory")
            {
                //TODO: need to figure out how to pull picked out of this
                var picked = 0; //int.Parse(Request.Params["app_params"]);
                var canvas = string.Format("http://apps.facebook.com/{0}/mysmiles.aspx", suffix);
                var templateData = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "emote", moods.ElementAt(picked).Value }, { "emoteaction", moods.ElementAt(picked).Value } });
                var feed = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "template_id", basePage.FeedTemplate2.ToString() }, { "template_data", templateData } });
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "feed", feed } });
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "method", "publisher_getFeedStory" }, { "content", content } });
                json = data;
            }
            else if (!string.IsNullOrEmpty(Request.Params["method"]) && Request.Params["method"] == "publisher_getInterface")
            {
                //TODO: need to figure out how to pull picked out of this
                var picked = 0; //int.Parse(Request.Params["app_params"]);
                var fbml = FBMLControlRenderer.RenderFBML<string>(string.Format("~/controls/PublisherHeader.ascx"),callback);
                fbml += string.Format("<form>{0}<input type=\"hidden\" id=\"picked\" name=\"picked\" value=\"-1\"></form>",basePage.BuildEmoticonGrid(moods,callback,suffix,false));
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "fbml", fbml}, {"publishEnabled","false"}, {"commentEnabled","false"} });
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "method", "publisher_getInterface" }, { "content", content } });
                json = data;
            }
            else
            {
                throw new Exception("no smile picked");
            }
        }
    }
}

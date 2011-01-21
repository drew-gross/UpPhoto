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
    public partial class publishHandler : CanvasFBMLBasePage
    {
        protected string json = string.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var basePage = new BasePage();
            var moods = basePage.getMoods();
            if (!string.IsNullOrEmpty(Request.Params["method"]) && Request.Params["method"] == "publisher_getFeedStory")
            {
                //TODO: need to figure out how to pull picked out of this
                var picked = 0; //int.Parse(Request.Params["app_params"]);
                var canvas = string.Format("http://apps.facebook.com/{0}/mysmiles.aspx", suffix);
                var image = string.Format("{0}images/smile{1}.jpg", callback, picked);
                var images = JSONHelper.ConvertToJSONAssociativeArray(new Dictionary<string, string> { { "src", image }, { "href", canvas } });
                var templateData = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "mood", moods.ElementAt(picked).Value }, { "emote", moods.ElementAt(picked).Key }, { "images", images }, { "mood_src", image } });
                var feed = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "template_id", basePage.FeedTemplate1.ToString() }, { "template_data", templateData } });
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "feed", feed } });
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "method", "publisher_getFeedStory" }, { "content", content } });
                json = data;
            }
            else if (!string.IsNullOrEmpty(Request.Params["method"]) && Request.Params["method"] == "publisher_getInterface")
            {
                //TODO: need to figure out how to pull picked out of this
                var fbml = FBMLControlRenderer.RenderFBML<string>(string.Format("~/controls/PublisherHeader.ascx"), callback).Replace("\n","");
                //var fbml = @"<style>
                //</style>";
                //fbml += "test";
                fbml += string.Format("<form>{0}<input type=\"hidden\" id=\"picked\" name=\"picked\" value=\"-1\"></form>", basePage.BuildEmoticonGrid(moods, callback, suffix, false));
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "fbml", fbml }, { "publishEnabled", "true" }, { "commentEnabled", "true" } });
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "content", content }, { "method", "publisher_getInterface" } });
                json = data;
            }
            else
            {
                throw new Exception("no smile picked");
            }

        }
    }
}

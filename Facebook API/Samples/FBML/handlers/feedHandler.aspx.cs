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
    public partial class feedHandler : CanvasFBMLBasePage
    {
        protected string json = string.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Request.Params["picked"]))
            {
                var picked = int.Parse(Request.Params["picked"]);
                var basePage = new BasePage();
                var moods = basePage.getMoods();
                var canvas = string.Format("http://apps.facebook.com/{0}/mysmiles.apsx", suffix);
                var moodList = JSONHelper.ConvertFromJSONArray(this.Api.Data.GetUserPreference(0));
                moodList.Insert(0, picked.ToString());
                this.Api.Data.SetUserPreference(0, JSONHelper.ConvertToJSONArray(moodList));
                var oldCount =  0;
                if(!string.IsNullOrEmpty(this.Api.Data.GetUserPreference(2)))
                {
                    oldCount = int.Parse(this.Api.Data.GetUserPreference(2));
                }
                this.Api.Data.SetUserPreference(2, (oldCount+1).ToString());
                var image = string.Format("{0}images/smile{1}.jpg",callback,picked);
                var images = JSONHelper.ConvertToJSONAssociativeArray(new Dictionary<string, string> { { "src", image }, { "href", canvas } });
                var templateData = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string, string> { { "mood", moods.ElementAt(picked).Value }, { "emote", moods.ElementAt(picked).Key}, { "images", images }, { "mood_src", image } });
                var feed = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string,string>{ {"template_id",basePage.FeedTemplate1.ToString()},{"template_data",templateData}});
                var content = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string,string>{ {"feed",feed},{"next",canvas}});
                var data = JSONHelper.ConvertToJSONAssociativeArray(
                    new Dictionary<string,string>{ {"method","feedStory"},{"content",content}}); 
                json = data;
            }
            else
            {
                throw new Exception("no smile picked");
            }

        }
    }
}

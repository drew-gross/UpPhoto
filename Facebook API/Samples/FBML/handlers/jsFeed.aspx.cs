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
    public partial class jsFeed : CanvasFBMLBasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Params["picked"]))
            {
                var picked = int.Parse(Request.Params["picked"]);
                var moodList = JSONHelper.ConvertFromJSONArray(this.Api.Data.GetUserPreference(0));
                moodList.Insert(0, picked.ToString());
                this.Api.Data.SetUserPreference(0, JSONHelper.ConvertToJSONArray(moodList));
                var oldCount = 0;
                if (!string.IsNullOrEmpty(this.Api.Data.GetUserPreference(2)))
                {
                    oldCount = int.Parse(this.Api.Data.GetUserPreference(2));
                }
                this.Api.Data.SetUserPreference(2, (oldCount + 1).ToString());
            }
        }
    }
}

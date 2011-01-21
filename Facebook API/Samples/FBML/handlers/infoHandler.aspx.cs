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
    public partial class infoHandler : CanvasFBMLBasePage
    {
        protected string json = string.Empty;
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: need to figure out how to trigger this so I can see what the fields param looks like 
            var fields = JSONHelper.ConvertFromJSONAssoicativeArray(Request.Params["fields"]);
            foreach(KeyValuePair<string,string> field in fields)
            {
                var label = field.Key;
                var elements = field.Value;
            }
        }
    }
}

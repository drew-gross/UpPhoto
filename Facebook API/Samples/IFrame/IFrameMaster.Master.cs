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
using Facebook;
using Facebook.Web;
namespace IFrameSample
{
    public partial class IFrameMaster : Facebook.Web.CanvasIFrameMasterPage
    {
		public IFrameMaster()
		{
			RequireLogin = true;
		}
    }
}

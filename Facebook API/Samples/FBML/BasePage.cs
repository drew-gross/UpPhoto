using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Web.Configuration;
using Facebook.Web;
using System.Collections.Generic;
using System.Text;

namespace FBMLSample
{
    public class BasePage : Page
    {
        public long FeedTemplate1
        { 
            get{ return long.Parse(WebConfigurationManager.AppSettings["TemplateID1"]);}
        }
        public long FeedTemplate2
        {
            get { return long.Parse(WebConfigurationManager.AppSettings["TemplateID2"]); }
        }

        public Dictionary<string, string> getMoods()
        {
            return new Dictionary<string, string>
            {
                {"Happy",":)"},
                {"Indifferent",":|"},
                {"Sad",":("},
                {"Cool","B-|"},
                {"Emo","//.-"},
                {"Surprised","=O"},
                {"Laughing","XD"},
                {"Vampire",": ["},
                {"Evil",">:|"}
            };
        }
        public Dictionary<string, string> getOtherMoods()
        {
            return new Dictionary<string, string>
            {
                {"Kiss",":-*"},
                {"Wink",";]"},
                {"Confused",":@"},
                {"Tear",":\'-)"},
                {"Tongue",":P"},
                {"Bite",":-K"},
            };
        }
        public string BuildEmoticonGrid(Dictionary<string, string> moods, string callback, string suffix)
        {
            return BuildEmoticonGrid(moods, callback, suffix, true);
        }
        public string BuildEmoticonGrid(Dictionary<string, string> moods, string callback, string suffix, bool useFinalFunction)
        {
            var ret = new StringBuilder();
            ret.Append("<div class=\"table\"><div class=\"row\">");
            for (int i = 0; i < moods.Count; i++)
            {
                var js = string.Format("final('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", FeedTemplate1, callback + "images", "http://apps.facebook.com/" + suffix, callback, moods.ElementAt(i).Key, moods.ElementAt(i).Value, i);
                if (!useFinalFunction)
                {
                    js = string.Format("select('{0}','{1}','{2}')", moods.ElementAt(i).Key, moods.ElementAt(i).Value, i);
                }
                if (i > 0 && i % 3 == 0)
                {
                    ret.Append("</div><div class=\"row\">");
                }
                ret.Append(string.Format("<div onclick=\"{0}\" onmouseover=\"over('{3}')\" onmouseout=\"out('{3}')\" class=\"box\" id=\"sm_{3}\"><div class=\"smiley\">{2}</div><div id=\"smt_{3}\" class=\"title\">{1}</div></div>", js, moods.ElementAt(i).Key, moods.ElementAt(i).Value, i));
            }
            ret.Append("</div></div>");
            return ret.ToString();
        }

    }
}

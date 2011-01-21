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
using Facebook.Rest;

namespace FBMLSample.config
{
    public partial class Setup : CanvasFBMLBasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.RequireLogin = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var t1 = WebConfigurationManager.AppSettings["TemplateID1"];
            var t2= WebConfigurationManager.AppSettings["TemplateID2"];
            var dict = new Dictionary<string, string>
            {
                {"application_name","Smiley.NET"},
                {"callback_url",callback},
                {"tab_default_name","Smile.NET"},
                {"profile_tab_url","mysmiles.aspx"},
                {"publish_action","Smile at!"},
                {"publish_url",callback + "handlers/otherPublishHandler.aspx"},
                {"publish_self_action","Smile!"},
                {"publish_self_url",callback + "handlers/publishHandler.aspx"},
                {"info_changed_url",callback + "handlers/infoHandler.aspx"},
                {"wide_mode","1"}
            };
            this.Api.Admin.SetAppProperties(dict);
            var one_line_story = new List <string>{"{*actor*} is feeling {*mood*} today"};
            var short_story = new List<feedTemplate>();
            var short_story_template = new feedTemplate
                                    {
                                        TemplateTitle = "{*actor*} is feeling so {*mood*} today",
                                        TemplateBody = "{*actor*} just wanted to let you know that he is so {*mood*} today",
                                        PreferredLayout = "1"
                                    };
            short_story.Add(short_story_template);

            var full_story = new feedTemplate
                                    {
                                        TemplateTitle = "{*actor*} is feeling very {*mood*} today",
                                        TemplateBody = "<div style=\"padding: 10px;width : 200px;height : 200px;margin: auto;text-align: center;border: black 1px;cursor: pointer;border: black solid 2px;background: orange;color: black;text-decoration: none;\"><div style=\"font-size: 60pt;font-weight: bold;padding: 40px;\">{*emote*}</div><div style=\"font-size: 20px; font-weight:bold;\">{*mood*}</div></div>"
                                    };
            if (string.IsNullOrEmpty(t1))
            {
                long bundle1id = this.Api.Feed.RegisterTemplateBundle(one_line_story, short_story, full_story);
                this.template1.Text = "Bundle 1 is " + bundle1id;
            }
            else
            {
                this.template1.Text = "Bundle 1 is " + t1;

            }
            
            one_line_story = new List <string>{"{*actor*} just wanted to {*emote*} at {*target*} today"};
            short_story = new List<feedTemplate>();
            short_story_template = new feedTemplate
                                    {
                                        TemplateTitle = "{*actor*} just wanted to {*emote*} at {*target*} today",
                                        TemplateBody = "Always a great day to {*emoteaction*}",
                                        PreferredLayout = "1"
                                    };
            short_story.Add(short_story_template);

            full_story = new feedTemplate
                                    {
                                        TemplateTitle = "{*actor*} just wanted to {*emote*} at {*target*} today",
                                        TemplateBody = "Always a great day to {*emoteaction*}"
                                    };


            if (string.IsNullOrEmpty(t2))
            {
                long bundle2id = this.Api.Feed.RegisterTemplateBundle(one_line_story, short_story, full_story);
                this.template2.Text = "Bundle 2 is " + bundle2id;
            }
            else
            {
                this.template2.Text = "Bundle 2 is " + t2;

            }

             var options  = new List<info_item>();
             options.Add(new info_item{label="Happy",image=callback + "images/smile0.jpg", sublabel= "",description="The original and still undefeated.", link="http://apps.facebook.com/"+suffix+"/smile.aspx?smile=1"});
             options.Add(new info_item{label="Indifferent",image=callback + "images/smile1.jpg", sublabel= "",description="meh....", link="http://apps.facebook.com/"+suffix+"/smile.aspx?smile=2"});
             options.Add(new info_item{label="Sad",image=callback + "images/smile2.jpg", sublabel= "",description="Oh my god! you killed my dog!", link="http://apps.facebook.com/"+suffix+"/smile.aspx?smile=3"});
             options.Add(new info_item { label = "Cool", image = callback + "images/smile3.jpg", sublabel = "", description = "Yeah. whatever", link = "http://apps.facebook.com/" + suffix + "/smile.aspx?smile=4"});
             this.Api.Profile.SetInfoOptions("My Smiles", options);                                                            

        }
    }
}

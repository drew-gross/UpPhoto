Smiley is a sample app for the Facebook platform. It was designed
specifically to test the many new features for the profile
redesign. In particular it features -

New profile box
Info section
Tab
Feed stories
Publisher (for self and others)
Feed Form (for self and others)

Further documentation is available at http://wiki.developers.facebook.com/index.php/New_Design_Integration_Guide

--- Setup ---

Smiley is packaged to set up on your own server. To configure Smiley,
follow these steps .

1) Create a new application at
(http://www.facebook.com/developers/). The only fields that you
need to give are application name and an app.facebook.com/{APP_SUFFIX}
url. The app will configure the rest.
2) Open web.config fill in the following fields: APIKey,Secret,Callback,Suffix
An example is shown below.  It is vital that you fill in all 4 values with your own values.
	<add key="APIKey" value="e739cf9a3580497cbbd7e6e4e98f7627"/>
	<add key="Secret" value="f6184d3fdab6d99baa018e3f61db7f77"/>
	<add key="Callback" value="http://fbtest2.claritycon.com/fbmlcanvassample/"/>
	<add key="Suffix" value="fbmlcanvassample"/>
3) Login to your app by going to http://apps.facebook.com/{APP_SUFFIX}/home.aspx and logging in.
4) Next manually run the setup file.  By using the following url http://apps.facebook.com/{APP_SUFFIX}/config/setup.aspx

The resulting page should show the id of template1 and template2.  Copy these from the screen and using them to set the following
values in the web.config: TemplateID1 and TemplateID2

An example is show here.
    <add key="TemplateID1" value="21373077566"/>
    <add key="TemplateID2" value="21373087566"/>

Now the app should be ready to use...
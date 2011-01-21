<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/FBMLMaster.Master" CodeBehind="Home.aspx.cs" Inherits="FBMLSample.Home" %>
<%@ MasterType VirtualPath="~/FBMLMaster.Master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="body">
<h2>Welcome to Smiley.NET!</h2>
<p>Smiley.NET is a sample app created to demonstrate the many platform integration points of the Facebook profile.</p>

<!-- Profile box -->
Here is an button for adding a box to your profile. This will go away if you add the box:
<div class="section_button"><fb:add-section-button section="profile"/></div>

<!-- Info section -->
Here is an button for adding an info section to your profile. This will go away if you add the section:
<div class="section_button"><fb:add-section-button section="info" /></div>

<!-- Permissions -->
These are FBML tags that can prompt users for extended permissions from the canvas page.<br />These will go away if you grant these permissions:<br />
<fb:prompt-permission perms="email">Enable Email</fb:prompt-permission>
<br />
<fb:prompt-permission perms="infinite_session">Enable Permanent Login</fb:prompt-permission>

<p>Upon submitting the form below, you will be prompted to grant email permissions (unless you've already done so for this app):</p>
<form promptpermission="email"><br />How often would you like to be notified of new smilies?<br /><input type="text" name="frequency"><input type="submit" value="Notify Me"></form></p>
<asp:Label runat="server" ID="l"></asp:Label>
</asp:Content>
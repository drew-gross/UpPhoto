<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FBToolkit.Samples.IFrameLoginControlSample._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="Facebook" Namespace="Facebook.Web" Assembly="Facebook.Web" %> 
<%@ Register TagPrefix="fb" Namespace="Facebook.Web.FbmlControls" Assembly="Facebook.Web" %> 

<Facebook:CanvasIFrameLoginControl runat="server" ID="login" RequireLogin="true" RequiredPermissions="offline_access,publish_stream"/>
<fb:Fbml ID="Fbml1" runat="server">
<ContentTemplate>

<fb:IfSectionNotAdded ID="IfSectionNotAdded1" runat="server" Section="Profile">
<ContentTemplate>
Display our box on your profile!<br />
<fb:AddSectionButton ID="AddSectionButton" runat="server" Section="Profile"></fb:AddSectionButton>
</ContentTemplate>
</fb:IfSectionNotAdded>    
<fb:Name ID="Name1" runat="server" Uid="loggedinuser"></fb:Name>
<fb:Board ID="Board" runat="server" Xid="aaaa" Title="TTT"></fb:Board>
<fb:Title ID="Title" runat="server" Text="T3"></fb:Title>
<fb:Dialog ID="Dialog" runat="server" Identifier="Dialog"><ContentTemplate><fb:DialogTitle ID="dialogtitle" runat="server" Text="Title of Dialog"></fb:DialogTitle><fb:DialogContent ID="content" runat="server"><ContentTemplate>this is the content of the dialog</ContentTemplate></fb:DialogContent></ContentTemplate></fb:Dialog>
<fb:Wall ID="wall" runat="server"><ContentTemplate><fb:WallPost ID="wallpost" runat="server" Uid="1"><ContentTemplate>this is some stuff<br /><fb:WallPostAction runat="server" ID="WallPostAction" Href="http://localhost" Text="the text for the link"></fb:WallPostAction></ContentTemplate></fb:WallPost></ContentTemplate></fb:Wall>
</ContentTemplate>
</fb:Fbml>
<asp:Label runat="server" ID="fbml"></asp:Label>

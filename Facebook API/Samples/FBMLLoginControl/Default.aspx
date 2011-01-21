<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FBToolkit.Samples.FBMLLoginControlSample._Default" %>

<%@ Register Assembly="Facebook.Web" Namespace="Facebook.Web.FbmlControls" TagPrefix="fb" %>
<%@ Register TagPrefix="Facebook" Namespace="Facebook.Web" Assembly="Facebook.Web" %> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <Facebook:CanvasFBMLLoginControl runat="server" ID="login"  RequireLogin="true"/>
    <asp:Label runat="server" ID="fbml"></asp:Label>
<fb:Name ID="Name1" runat="server" CapitalizeYou="false" Uid="loggedinuser">
</fb:Name>

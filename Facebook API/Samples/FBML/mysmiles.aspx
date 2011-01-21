<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/FBMLMaster.Master" CodeBehind="mysmiles.aspx.cs" Inherits="FBMLSample.mysmiles" %>
<%@ MasterType VirtualPath="~/FBMLMaster.Master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="body">
<div style="text-align: center">
    <h2><fb:name firstnameonly="true" useyou="false" possessive="true" linked="false" uid="<%=Master.Api.Session.UserId %>"/> Smilies</h2>
    <h3 style="padding: 7px 0px">
    <asp:Label runat="server" id="announce"></asp:Label>
    </h3>
    <div style="overflow:hidden">
        <div class="past">
            <asp:Label id="lblMoodList" runat="server"/>
        </div>
    </div>
    <br/>
    <asp:Label runat="server" id="lblLink" />
</div>
</asp:Content>
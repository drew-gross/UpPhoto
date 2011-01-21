<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/FBMLMaster.Master" CodeBehind="sendsmiley.aspx.cs" Inherits="FBMLSample.sendsmiley" %>
<%@ MasterType VirtualPath="~/FBMLMaster.Master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="body">
<h2>Send a friend a smiley</h2>
<form fbtype="multiFeedStory" action="<%=multiFeedHandler %>">
<div class="input_row"> 
<fb:multi-friend-input />
</div>
<asp:Label runat="server" id="grid" />
<input type="hidden" id="picked" name="picked" value="-1">
<div id="centerbutton" class="buttons">
<input type="submit" id="mood" label="Send Smiley"></div>
<div id="emoticon"></div>
</form>
</asp:Content>
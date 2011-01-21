<%@ Page Language="C#"  AutoEventWireup="true" MasterPageFile="~/FBMLMaster.Master" CodeBehind="newsmiley.aspx.cs" Inherits="FBMLSample.newsmiley" %>
<%@ MasterType VirtualPath="~/FBMLMaster.Master" %>
<asp:Content runat="server" ID="content" ContentPlaceHolderID="body">

<h2>What's your mood today?</h2>
<asp:Label runat="server" id="lblSetCount" />
<asp:Label runat="server" id="lblfeedHandler" />
<asp:Label runat="server" id="lblEmoticonGrid" />
<input type="hidden" id="picked" name="picked" value="-1">
<div id="emoticon">
</div>
</form>
</div>
  
  </asp:Content>
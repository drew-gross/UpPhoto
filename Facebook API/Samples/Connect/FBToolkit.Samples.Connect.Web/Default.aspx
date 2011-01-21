<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FBToolkit.Samples.Connect.Web.Default" %>
<%@ Import Namespace="FBToolkit.Samples.Connect.Web"%>
<%@ Import Namespace="Facebook.Schema"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:fb="http://www.facebook.com/2008/fbml">  
<head runat="server">
    <title></title>
    <script src="http://static.ak.connect.facebook.com/js/api_lib/v0.4/FeatureLoader.js.php" type="text/javascript"></script>     
    <link rel="stylesheet" type="text/css" href="Style1.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div runat="server" id="AuthenticationSection">
        <fb:login-button onlogin="window.location.reload()"></fb:login-button>
        <br />
        <asp:label id="lblStatus" runat="server" />
    </div>
    
    <table width="100%">
    <tr>
    <td width="50%" class="LeftPanelContainer">
    <div runat="server" id="NewEventSection">
        <h3>Create Event</h3>
        <div>
            <table>
                <tr>
                    <td colspan="2"><asp:HyperLink ID="NewEventGrantPermission" runat="server" 
                            Target="_blank" Text="Grant Create Event Permission" /></td>
                </tr>
                <tr>
                    <td>Name:</td>
                    <td><asp:TextBox runat="server" ID="NewEventName" Width="400" /></td>
                </tr>
                <tr>
                    <td>Description:</td>
                    <td><asp:TextBox runat="server" ID="NewEventDescription" Width="400" /></td>
                </tr>
                <tr>
                    <td>Date:</td>
                    <td><asp:TextBox runat="server" ID="NewEventDate" Width="400" /></td>
                </tr>
                <tr>
                    <td>Location:</td>
                    <td><asp:TextBox runat="server" ID="NewEventLocation" Width="400" /></td>
                </tr>
                <tr>
                    <td>Host:</td>
                    <td><asp:TextBox runat="server" ID="NewEventHost" Width="400" /></td>
                </tr>                
                <tr>
                    <td></td>
                    <td class="TopMarginSpacer"><asp:Button runat="server" ID="CreateNewEvent" Text="Create Event"
                         onclick="CreateNewEvent_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Label class="TopMarginSpacer" runat="server" ID="NewEventStatus" /></td>
                </tr>
            </table>
        </div>
    </div>
    </td>
    
    <td width="50%" class="RightPanelContainer">    
    <div runat="server" id="ManageEventSection">
        <h3>Existing Events</h3>
        <asp:DropDownList ID="EventList" runat="server" AutoPostBack="true" Width="200px"
            onselectedindexchanged="eventList_SelectedIndexChanged"  />            
        <div class="ExistingEventResponseList">
            <asp:ListView ID="ExistingEventFriendsList" runat="server" DataKeyNames="UserID">
                <LayoutTemplate>
                  <table cellpadding="2" border="1" style="margin:6px 0 0 0;">
                    <tr id="Tr1" runat="server">
                        <th id="Th1" runat="server"></th>
                        <th id="Th2" runat="server"></th>
                        <th id="Th3" class="ExistingEventResponseCell" runat="server">Attending</th>
                        <th id="Th4" class="ExistingEventResponseCell" runat="server">No Response</th>
                        <th id="Th5" class="ExistingEventResponseCell" runat="server">Undecided</th>
                        <th id="Th6" class="ExistingEventResponseCell" runat="server">Declined</th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder" />
                  </table>
                </LayoutTemplate>
                <ItemTemplate>
                  <tr>
                    <td>
                      <img src="<%# ((EventUser)Container.DataItem).PicSquare %>" height="30px" width="30px" alt="profile pic" />
                    </td>
                    <td valign="top">
                        <asp:Label ID="LastNameLabel" runat="Server" 
                                Text='<%# string.Format("{0} {1}", ((EventUser)Container.DataItem).FirstName, ((EventUser)Container.DataItem).LastName) %>' />
                    </td>                    
                    <td class="ExistingEventResponseCell">
                      <asp:CheckBox ID="Attending" runat="Server" Enabled="false" Checked='<%# ((EventUser)Container.DataItem).IsAttending %>' />
                    </td> 
                    <td class="ExistingEventResponseCell">
                      <asp:CheckBox ID="NoResponse" runat="Server" Enabled="false" Checked='<%# ((EventUser)Container.DataItem).IsNotResponded %>' />
                    </td> 
                    <td class="ExistingEventResponseCell">
                      <asp:CheckBox ID="Undecided" runat="Server" Enabled="false" Checked='<%# ((EventUser)Container.DataItem).IsUnsure %>' />
                    </td> 
                    <td class="ExistingEventResponseCell">
                      <asp:CheckBox ID="Declined" runat="Server" Enabled="false" Checked='<%# ((EventUser)Container.DataItem).IsDeclined %>' />
                    </td>                    
                  </tr>
                </ItemTemplate>  
            </asp:ListView>   
        </div>
    </div>
    </td>
    </tr>
    </table>
    </form>
</body>
<script type="text/javascript">
    FB.init("9a6e2f9bf3b0be5b695e95d8a6f71f34", "xd_receiver.htm");  
</script>  
</html>

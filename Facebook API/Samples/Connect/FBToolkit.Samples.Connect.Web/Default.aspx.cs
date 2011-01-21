using System;
using System.Collections.Generic;
using System.Web.UI;
using Facebook.Rest;
using Facebook.Schema;
using Facebook.Session;
using Facebook.Utility;

namespace FBToolkit.Samples.Connect.Web
{
    public partial class Default : Page
    {
        #region Private Members

        // NOTE: In production app, these keys would be stored in config file
        private const string APPLICATION_KEY = "9a6e2f9bf3b0be5b695e95d8a6f71f34";
        private const string SECRET_KEY = "c36784bbe946e8bb778791aaf2d92a69";

        private Api _facebookAPI;
        private List<EventUser> _eventUsers;
        private ConnectSession _connectSession;

        #endregion Private Members

        #region Page Load

        /// <summary>
        /// Handles Default Page Load event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Authenticated, created session and API object
            _connectSession = new ConnectSession(APPLICATION_KEY, SECRET_KEY);

            if (!_connectSession.IsConnected())
            {
                // Not authenticated, proceed as usual.
                lblStatus.Text = "Please sign-in with Facebook.";
            }
            else
            {
                // Authenticated, create API instance
                _facebookAPI = new Api(_connectSession);

                // Load user
                user user = _facebookAPI.Users.GetInfo();

                if (!Page.IsPostBack)
                {
                    lblStatus.Text = string.Format("Signed in as {0} {1}", user.first_name, user.last_name);
                    NewEventGrantPermission.NavigateUrl =
                        string.Format("http://www.facebook.com/authorize.php?api_key={0}&v=1.0&ext_perm={1}", APPLICATION_KEY, Enums.ExtendedPermissions.create_event);
                    NewEventDate.Text = DateTime.Now.AddDays(7).ToString();
                    NewEventHost.Text = string.Format("{0} {1}", user.first_name, user.last_name);

                    // Load Existing Events
                    LoadExistingEvents();
                }
            }

            if (!Page.IsPostBack)
            {
                // Set section visibility
                ToggleSectionVisibility();
            }
        }

        #endregion Page Load

        #region Private Methods

        private void ToggleSectionVisibility()
        {
            AuthenticationSection.Visible = !_connectSession.IsConnected();
            NewEventSection.Visible = _connectSession.IsConnected();
            ManageEventSection.Visible = _connectSession.IsConnected();
        }
        
        private void LoadEventMembers()
        {
            long eid;
            if (!long.TryParse(EventList.SelectedValue, out eid)) return;
            if(eid <= 0) return;

            var eventMemberInfo = _facebookAPI.Events.GetMembers(eid);

            // Reset event users
            _eventUsers = new List<EventUser>();

            // Load for each response type
            LoadUsers(eventMemberInfo.attending.uid, EventUser.EventUserStatus.Attending);
            LoadUsers(eventMemberInfo.declined.uid, EventUser.EventUserStatus.Declined);
            LoadUsers(eventMemberInfo.not_replied.uid, EventUser.EventUserStatus.NoResponse);
            LoadUsers(eventMemberInfo.unsure.uid, EventUser.EventUserStatus.Undecided);

            // Sort by last name
            _eventUsers.Sort((x, y) => x.LastName.CompareTo(y.LastName));

            ExistingEventFriendsList.DataSource = _eventUsers;
            ExistingEventFriendsList.DataBind();
        }

        private void LoadUsers(List<long> uids, EventUser.EventUserStatus eventUserStatus)
        {
            var users = _facebookAPI.Users.GetInfo(uids);
            foreach(var u in users)
            {
                _eventUsers.Add(new EventUser(u, eventUserStatus));
            }
        }
        
        private void LoadExistingEvents()
        {
            // Get user's events via Facebook Toolkit API object
            var facebookevents = _facebookAPI.Events.Get();

            // Insert dummy item
            facebookevents.Insert(0, new facebookevent { eid = -1, name = "Existing Events..."});

            // Bind list to existing events
            EventList.DataSource = facebookevents;
            EventList.DataTextField = "name";
            EventList.DataValueField = "eid";
            EventList.DataBind();
        }

        #endregion Private Methods

        #region Event Handlers

        /// <summary>
        /// Existing Event selection change event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void eventList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear current binding
            ExistingEventFriendsList.DataSource = null;
            ExistingEventFriendsList.DataBind();

            if (string.IsNullOrEmpty(EventList.SelectedValue)) return;
            LoadEventMembers();
        }

        /// <summary>
        /// Create new event button click event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateNewEvent_Click(object sender, EventArgs e)
        {
            try
            {
                var ev = new facebookevent
                             {
                                 name = NewEventName.Text.Trim(),
                                 description = NewEventDescription.Text.Trim(),
                                 event_type = "Online Meetup",
                                 event_subtype = "Facebook Connect Meetup",
                                 location = NewEventLocation.Text.Trim(),
                                 host = NewEventHost.Text.Trim()
                             };

                // Configure hardcoded (sample) event date/times
                DateTime eventDate = DateTime.Parse(NewEventDate.Text.Trim());
                ev.start_time = DateHelper.ConvertDateToFacebookDate(new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 14, 0, 0));
                ev.end_time = DateHelper.ConvertDateToFacebookDate(new DateTime(eventDate.Year, eventDate.Month, eventDate.Day, 16, 0, 0));

                long newEventId = _facebookAPI.Events.Create(ev);
                NewEventStatus.Text = string.Format("Event ({0}) Created Successfully.", newEventId);

                // Reload Events
                LoadExistingEvents();
                LoadEventMembers();
            }
            catch (Exception ex)
            {
                NewEventStatus.Text = ex.Message;
            }
        }

        #endregion Event Handlers
    }
}
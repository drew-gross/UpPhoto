using System;
using Facebook.Schema;

namespace FBToolkit.Samples.Connect.Web
{
    /// <summary>
    /// Wrapper class for facebook user object, allows assignment of event response status
    /// (Created for ListView Binding purposes only)
    /// </summary>
    public class EventUser
    {
        public enum EventUserStatus
        {
            Attending,
            Undecided,
            Declined,
            NoResponse
        }

        public user User { get; set; }
        public bool IsAttending { get; private set;}
        public bool IsUnsure { get; private set; }
        public bool IsNotResponded { get; private set; }
        public bool IsDeclined { get; private set; }

        public long? UserID
        {
            get
            {
                return User.uid;
            }
        }

        public string FirstName
        {
            get
            {
                return User.first_name;
            }
        }

        public string LastName
        {
            get
            {
                return User.last_name;
            }
        }

        public string PicSquare
        {
            get
            {
                return User.pic_square;
            }
        }

        public EventUser(user user, EventUserStatus eventUserStatus)
        {
            User = user;
            SetEventStatus(eventUserStatus);
        }

        private void SetEventStatus(EventUserStatus eventUserStatus)
        {
            switch (eventUserStatus)
            {
                case EventUserStatus.Attending:
                    IsAttending = true;
                    break;

                case EventUserStatus.Declined:
                    IsDeclined = true;
                    break;

                case EventUserStatus.NoResponse:
                    IsNotResponded = true;
                    break;

                case EventUserStatus.Undecided:
                    IsUnsure = true;
                    break;

                default:
                    throw new Exception("Unable to determine User Event Status");
            }
        }
    }
}
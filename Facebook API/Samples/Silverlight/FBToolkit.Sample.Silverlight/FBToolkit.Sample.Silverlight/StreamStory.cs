using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Facebook.BindingHelper;
using Facebook.Utility;

namespace FBToolkit.Sample.Silverlight
{
    public class StreamStoryComment : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public StreamStoryComment(FacebookComment c, FacebookProfile p)
        {
            comment = c;
            poster_profile = p;
        }

        public FacebookComment comment
        {
            get;
            set;
        }

        public FacebookProfile poster_profile
        {
            get;
            set;
        }

        public string poster_name
        {
            get
            {
                return poster_profile.name;
            }
        }

        public string pic_square
        {
            get
            {
                return poster_profile.Picture;
            }
        }

        public string UpdateTime
        {
            get
            {
                return " at " + comment.Time.ToString("G");

            }
        }
    }

    public class StreamStory : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public StreamStory(FacebookStreamPost p, FacebookProfile pro)
        {
            stream_post = p;
            profile = pro;
            comments = new List<StreamStoryComment>();
        }

        public List<StreamStoryComment> comments
        {
            set;
            get;
        }

        public FacebookStreamPost stream_post
        {
            get;
            set;
        }
        public FacebookProfile profile
        {
            get;
            set;
        }

        public string UpdateTime
        {
            get
            {
                DateTime now = DateTime.Now;
                TimeSpan span = now - stream_post.CreatedTime;
                if (span.TotalSeconds < 60)
                    return (int)span.TotalSeconds + " seconds ago";
                if (span.TotalMinutes < 60)
                    return (int)span.TotalMinutes + " minutes ago";
                if (stream_post.CreatedTime > DateTime.Today)
                    return (int)span.TotalHours + " hours ago";
                if (stream_post.CreatedTime > DateTime.Today.AddDays(-1))
                    return "Yesterday at " + stream_post.CreatedTime.ToString("t");
                TimeZoneInfo nzTimeZone = TimeZoneInfo.Local;
                DateTime nzDateTime = TimeZoneInfo.ConvertTime(stream_post.CreatedTime, nzTimeZone);
                return nzDateTime.ToString("G");
            }
        }

        //private void NotifyPropertyChanged(String info)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(info));
        //    }
        //}

        ///// <summary>
        ///// Parse the long, which is the number of seconds since 1/1/1970
        ///// </summary>
        ///// <param name="time"></param>
        ///// <returns></returns>
        //private DateTime ParseTime(long timeinSecondsSinceEpoch)
        //{
        //    DateTime unixEpoch = new DateTime(1970, 1, 1);
        //    TimeSpan t = new TimeSpan(0, 0, (int)timeinSecondsSinceEpoch);

        //    return unixEpoch + t;
        //}
    }

    public class StreamStoryCollection : ObservableCollection<StreamStory>
    {
    }

    class Utility
    {
        public static DateTime BaseUTCDateTime
        {
            get { return new DateTime(1970, 1, 1, 0, 0, 0); }
        }

        //Event dates are stored by assuming the time which the user input was in Pacific
        // time (PST or PDT, depending on the date), converting that to UTC, and then
        // converting that to Unix epoch time. This algorithm reverses that process.
        public static DateTime ConvertDoubleToEventDate(double secondsSinceEpoch)
        {
            DateTime utcDateTime = BaseUTCDateTime.AddSeconds(secondsSinceEpoch);
            int pacificZoneOffset = utcDateTime.IsDaylightSavingTime() ? -7 : -8;
            return utcDateTime.AddHours(pacificZoneOffset);
        }

    }

}

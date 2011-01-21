namespace FriendBarSample
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Facebook;
    using System.Xml;

    using System.Globalization;
    using System.Collections.Specialized;
    using Facebook.BindingHelper;

    /// <summary>
    /// Interaction logic for FriendBarControl.xaml
    /// </summary>
    public partial class FriendBarControl : UserControl
    {
        public FacebookContactCollection Friends { get; set; }
        public FilmStripControl FilmStripControl
        {
            get;
            private set;
        }
        public FriendBarControl()
        {
            Friends = ServiceProvider.FacebookService.Friends;
            
            InitializeComponent();
            FilmStripControl = this.FindName("FilmStrip") as FilmStripControl;

        }
       
    }
}


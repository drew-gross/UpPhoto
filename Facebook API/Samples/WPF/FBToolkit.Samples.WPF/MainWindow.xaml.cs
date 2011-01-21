namespace NewsFeedSample
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Facebook;
    using System.Xml;
    using System.Globalization;
    using Facebook.BindingHelper;
    using Facebook.Session;
    using System.Collections.Generic;
    using Facebook.Schema;
    using Facebook.Rest;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DesktopSession session;
        public static BindingManager FacebookService { get; private set; }

        public MainWindow()
        {
            session = new DesktopSession("aff9f004793a1d32d26fe2361d5fc723", null, null, true, new List<Enums.ExtendedPermissions>(){Enums.ExtendedPermissions.read_stream, Enums.ExtendedPermissions.publish_stream});
            session.Login();
            Api api = new Api(session);
            var friends = api.Friends.GetUserObjects();
            var service = BindingManager.CreateInstance(session);

            ServiceProvider.Initialize(service);
            
            InitializeComponent();           
        }

    }
}

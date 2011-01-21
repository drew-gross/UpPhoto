namespace FriendBarSample
{
    using System.Windows;
    using Facebook.BindingHelper;
    using Facebook.Session;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DesktopSession session = new DesktopSession("aff9f004793a1d32d26fe2361d5fc723", true);
        public static BindingManager FacebookService { get; private set; }

        public MainWindow()
        {
            session.Login();

            var service = BindingManager.CreateInstance(session);



            ServiceProvider.Initialize(service);
            Friends = ServiceProvider.FacebookService.Friends;
            InitializeComponent();           
        }
        public FacebookContactCollection Friends { get; set; }
        

    }
}

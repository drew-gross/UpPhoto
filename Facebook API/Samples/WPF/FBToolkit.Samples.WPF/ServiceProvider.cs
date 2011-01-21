namespace NewsFeedSample
{
    using System;
    using Facebook;
    using Facebook.BindingHelper;

    public static class ServiceProvider
    {
        public static BindingManager FacebookService { get; private set; }

        public static ActivityPostCollection NewsFeed
        {
            get
            {
                return (FacebookService != null) ? FacebookService.Stream : null; 
            }
        }

        public static FacebookContactCollection Friends
        {
            get
            {
                return (FacebookService != null) ? FacebookService.Friends : null;
            }
        }

        /// <summary>
        /// Shuts down the service provider.
        /// </summary>
        public static void Shutdown()
        {
            FacebookService = null;
        }

        public static void Initialize(BindingManager fb)
        {
            try
            {
                FacebookService = fb;
            }
            catch
            {
                Shutdown();
            }
        }
    }
}

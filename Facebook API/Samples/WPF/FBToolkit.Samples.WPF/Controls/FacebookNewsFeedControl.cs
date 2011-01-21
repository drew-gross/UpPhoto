namespace NewsFeedSample
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Facebook;
    using Facebook.BindingHelper;

    public class FacebookNewsFeedControl : Control
    {
        public static readonly DependencyProperty ActivityPostsProperty = DependencyProperty.Register("ActivityPosts", typeof(ActivityPostCollection), 
            typeof(FacebookNewsFeedControl));

        public ActivityPostCollection ActivityPosts
        {
            get { return (ActivityPostCollection)GetValue(ActivityPostsProperty); }
            set { SetValue(ActivityPostsProperty, value); }
        }

        public static readonly RoutedCommand NavigateToFriendCommand = new RoutedCommand("NavigateToFriend", typeof(FacebookNewsFeedControl));
        public static readonly RoutedCommand AddCommentCommand = new RoutedCommand("AddComment", typeof(FacebookNewsFeedControl));
        public static readonly RoutedCommand RemoveCommentCommand = new RoutedCommand("RemoveComment", typeof(FacebookNewsFeedControl));
        public static readonly RoutedCommand AddLikeCommand = new RoutedCommand("AddLike", typeof(FacebookNewsFeedControl));
        public static readonly RoutedCommand RemoveLikeCommand = new RoutedCommand("RemoveLike", typeof(FacebookNewsFeedControl));

        public FacebookNewsFeedControl()
        {
            this.CommandBindings.Add(new CommandBinding(NavigateToFriendCommand, new ExecutedRoutedEventHandler(OnNavigateToFriendCommand),
                new CanExecuteRoutedEventHandler(OnNavigateToFriendCommandCanExecute)));
            this.CommandBindings.Add(new CommandBinding(AddCommentCommand, new ExecutedRoutedEventHandler(OnAddCommentCommand),
                new CanExecuteRoutedEventHandler(OnAddCommentCommandCanExecute)));
            this.CommandBindings.Add(new CommandBinding(RemoveCommentCommand, new ExecutedRoutedEventHandler(OnRemoveCommentCommand),
                new CanExecuteRoutedEventHandler(OnRemoveCommentCommandCanExecute)));
            this.CommandBindings.Add(new CommandBinding(AddLikeCommand, new ExecutedRoutedEventHandler(OnAddLikeCommand),
                new CanExecuteRoutedEventHandler(OnAddLikeCommandCanExecute)));
            this.CommandBindings.Add(new CommandBinding(RemoveLikeCommand, new ExecutedRoutedEventHandler(OnRemoveLikeCommand),
                new CanExecuteRoutedEventHandler(OnRemoveLikeCommandCanExecute)));
        }

        private void OnNavigateToFriendCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            FacebookContact contact = e.Parameter as FacebookContact;
            if (contact == null)
            {
                return;
            }

            e.CanExecute = ServiceProvider.Friends.Contains(contact);
        }

        private void OnNavigateToFriendCommand(object sender, ExecutedRoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void OnAddCommentCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            object[] parameterList = e.Parameter as object[];
            if (parameterList == null)
            {
                return;
            }

            ActivityPost activityPost = parameterList[0] as ActivityPost;
            string comment = parameterList[1] as string;
            if (activityPost == null || comment == null)
            {
                return;
            }

            e.CanExecute = activityPost.CanComment;
        }

        private void OnAddCommentCommand(object sender, ExecutedRoutedEventArgs e)
        {
            object[] parameterList = e.Parameter as object[];
            ActivityPost activityPost = parameterList[0] as ActivityPost;
            string comment = parameterList[1] as string;

            activityPost.AddComment(comment);
            //ServiceProvider.FacebookService.AddComment(activityPost, comment);
        }

        private void OnRemoveCommentCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            object[] parameterList = e.Parameter as object[];
            if (parameterList == null)
                return;
            ActivityComment comment = parameterList[0] as ActivityComment;
            if (comment == null)
            {
                return;
            }

            e.CanExecute = comment.IsMine;
        }

        private void OnRemoveCommentCommand(object sender, ExecutedRoutedEventArgs e)
        {
            object[] parameterList = e.Parameter as object[];
            if (parameterList == null)
                return;
            FrameworkElement elem = parameterList[1] as FrameworkElement;
            ActivityPost post = elem.DataContext as ActivityPost;

            post.RemoveComment(parameterList[0] as ActivityComment);
        }

        private void OnAddLikeCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ActivityPost activityPost = e.Parameter as ActivityPost;
            if (activityPost == null)
            {
                return;
            }

            e.CanExecute = activityPost.CanLike && !activityPost.HasLiked;
        }

        private void OnAddLikeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ActivityPost activityPost = e.Parameter as ActivityPost;
            activityPost.AddLike();
            //ServiceProvider.FacebookService.AddLike(activityPost);
        }

        private void OnRemoveLikeCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ActivityPost activityPost = e.Parameter as ActivityPost;
            if (activityPost == null)
            {
                return;
            }

            e.CanExecute = activityPost.HasLiked;
        }

        private void OnRemoveLikeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            ActivityPost activityPost = e.Parameter as ActivityPost;
            activityPost.RemoveLike();
            //ServiceProvider.FacebookService.RemoveLike(activityPost);
        }
    }
}

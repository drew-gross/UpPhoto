using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Documents;

namespace NewsFeedSample
{
    [ContentProperty("Text")]
    public class BindableRun : Run
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BindableRun),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTextPropertyChanged)));

        new public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Run run = d as Run;
            run.Text = (string)e.NewValue;
        }
    }
}

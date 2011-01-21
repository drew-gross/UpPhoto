namespace NewsFeedSample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Navigation;
    using System.Xml;
    using System.Xml.Linq;
    using System.Text;

    public class HyperlinkTextContent : Span
    {
        private static Regex urlRegex = new Regex(@"[a-z]+://[^ \t\r\n\v\f]+"); // todo: this is naive.

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(HyperlinkTextContent),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTextPropertyChanged)));

        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HyperlinkTextContent element = (HyperlinkTextContent)d;
            element.Inlines.Clear();
            element.GenerateInlines();
        }

        private void GenerateInlines()
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                string text = this.Text;
                bool formatted = false;
                if (this.Text.StartsWith("<div", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        IEnumerable<Inline> inlines = _BuildInlinesFromHtml(this.Text);
                        foreach (var inline in inlines)
                        {
                            this.Inlines.Add(inline);
                        }
                        formatted = true;
                    }
                    catch
                    {
                        // If we're unable to parse the DIV tags then just display the HTML with the tags stripped out.
                        // This will only work if the HTML is mostly correctly formatted, e.g. start and end tags match up.
                        // TODO: Anecdotally, it seems like the most common reason for this is a '&' character in the content.
                        //    We should be able to handle that...
                        bool include = true;
                        text = text.Split('<', '>').Aggregate(new StringBuilder(), (sb, token) => { sb.Append(include ? token : ""); include = !include; return sb; }).ToString();
                    }
                }

                if (!formatted)
                {
                    this.Inlines.AddRange(_BuildInlinesFromRawText(text));
                }
            }
        }

        private static IEnumerable<Inline> _BuildInlinesFromHtml(string text)
        {
            // Enclose these in a redundant <div> tag pair because we're seeing multiple
            // top level elements in the text.
            XDocument xdoc = XDocument.Parse("<div>" + text + "</div>", LoadOptions.PreserveWhitespace);
            bool lineBreak = false;
            foreach (var node in xdoc.Root.Nodes())
            {
                IEnumerable<Inline> inlines = _BuildInlinesFromFragment(node, lineBreak);
                lineBreak = false;
                foreach (var inline in inlines)
                {
                    yield return inline;
                }
            }
        }

        private static IEnumerable<Inline> _BuildInlinesFromFragment(XNode fragment, bool skipDivNewline)
        {
            if (fragment.NodeType == XmlNodeType.Text)
            {
                yield return new Run(fragment.ToString());
                yield break;
            }

            if (fragment.NodeType == XmlNodeType.Element)
            {
                XElement element = (XElement)fragment;
                switch (element.Name.LocalName.ToUpper())
                {
                    case "I":
                    case "EM":
                        var italic = new Italic();
                        foreach (var subnode in element.Nodes())
                        {
                            italic.Inlines.AddRange(_BuildInlinesFromFragment(subnode, false));
                        }
                        yield return italic;                            
                        break;
                    case "B":
                    case "STRONG":
                        var bold = new Bold();
                        foreach (var subnode in element.Nodes())
                        {
                            bold.Inlines.AddRange(_BuildInlinesFromFragment(subnode, false));
                        }
                        yield return bold;
                        break;
                    case "U":
                        var underline = new Underline();
                        foreach (var subnode in element.Nodes())
                        {
                            underline.Inlines.AddRange(_BuildInlinesFromFragment(subnode, false));
                        }
                        yield return underline;
                        break;
                    case "DIV":
                        if (!skipDivNewline)
                        {
                            yield return new LineBreak();
                        }
                        var span = new Span();
                        foreach (var subnode in element.Nodes())
                        {
                            span.Inlines.AddRange(_BuildInlinesFromFragment(subnode, false));
                        }
                        yield return span;
                        break;
                    case "A":
                        var hyperlink = new Hyperlink();
                        foreach (var subnode in element.Nodes())
                        {
                            hyperlink.Inlines.AddRange(_BuildInlinesFromFragment(subnode, false));
                        }
                        Uri navigateUri;
                        // Attribute(string) is case sensitive... 
                        if (Uri.TryCreate(element.Attribute("href").Value, UriKind.RelativeOrAbsolute, out navigateUri))
                        {
                            hyperlink.NavigateUri = navigateUri;
                        }
                        hyperlink.RequestNavigate += new RequestNavigateEventHandler(OnRequestNavigate);

                        yield return hyperlink;
                        break;
                    default:
                        // Treat anything unknown as a linebreak?
                        yield return new LineBreak();
                        break;
                }
            }
            yield break;
        }


        private static IEnumerable<Inline> _BuildInlinesFromRawText(string text)
        {
            MatchCollection matches = urlRegex.Matches(text);

            int index = 0;
            foreach (Match match in matches)
            {
                if (match.Index > index)
                {
                    yield return new Run(text.Substring(index, match.Index - index));
                }

                string url = text.Substring(match.Index, match.Length);
                Hyperlink hyperlink = new Hyperlink(new Run(url));
                hyperlink.NavigateUri = new Uri(url);
                hyperlink.RequestNavigate += new RequestNavigateEventHandler(OnRequestNavigate);

                yield return hyperlink;
                index = match.Index + match.Length;
            }

            if (index < text.Length - 1)
            {
                yield return new Run(text.Substring(index, text.Length - index));
            }
        }

        private static void OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(hyperlink.NavigateUri.ToString()));
            e.Handled = true;
        }
    }
}

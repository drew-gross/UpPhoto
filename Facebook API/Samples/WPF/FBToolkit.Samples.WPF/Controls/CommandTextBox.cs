//-----------------------------------------------------------------------
// <copyright file="CommandTextBox.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//     Subclass of TextBox that exposes a CommitCommand and CommitCommandParameter.
// </summary>
//-----------------------------------------------------------------------

namespace NewsFeedSample
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Subclass of TextBox that exposes a CommitCommand and CommitCommandParameter. This class registers class-level 
    /// command bindings for the Enter key that invokes the CommitCommand. The AcceptsReturn property on this class is meaningless because
    /// the Enter key always invokes the command. Intended for use in Search control.
    /// </summary>
    public class CommandTextBox : TextBox
    {
        public static readonly DependencyProperty IsAvailableProperty = DependencyProperty.Register("IsAvailable", typeof(bool), typeof(CommandTextBox));

        public bool IsAvailable
        {
            get { return (bool)GetValue(IsAvailableProperty); }
            set { SetValue(IsAvailableProperty, value); }
        }

        /// <summary>
        /// The DependencyProperty for CommitCommand.
        /// </summary>
        public static readonly DependencyProperty CommitCommandProperty =
                DependencyProperty.Register(
                        "CommitCommand",
                        typeof(ICommand),
                        typeof(CommandTextBox),
                        new FrameworkPropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for the CommitCommandParameter.
        /// </summary>
        public static readonly DependencyProperty CommitCommandParameterProperty =
                DependencyProperty.Register(
                        "CommitCommandParameter",
                        typeof(object),
                        typeof(CommandTextBox),
                        new FrameworkPropertyMetadata((object)null));

        /// <summary>
        /// Gets or sets the Command property.
        /// </summary>
        public ICommand CommitCommand
        {
            get { return (ICommand)GetValue(CommitCommandProperty); }
            set { SetValue(CommitCommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter to pass to the CommitCommandProperty upon execution.
        /// </summary>
        public object CommitCommandParameter
        {
            get { return GetValue(CommitCommandParameterProperty); }
            set { SetValue(CommitCommandParameterProperty, value); }
        }

        /// <summary>
        /// When the Enter key is pressed, invoke CommitCommand on key up and clear search text. 
        /// </summary>
        /// <param name="e">Arguments describing the KeyDown event.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
                {
                    if (e.Key == Key.Enter)
                    {
                        // On commit, search and clear text
                        e.Handled = true;
                        this.ExecuteCommitCommand();

                        this.Clear();
                        this.IsAvailable = false;
                    }
                    else if (e.Key == Key.Escape)
                    {
                        this.Clear();
                        this.IsAvailable = false;
                    }
                }
            }

            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        /// <summary>
        /// Execute the CommitCommand.
        /// </summary>
        protected virtual void ExecuteCommitCommand()
        {
            if (this.CommitCommand != null && this.CommitCommand.CanExecute(this.CommitCommandParameter))
            {
                this.CommitCommand.Execute(this.CommitCommandParameter);
            }
        }
    }
}

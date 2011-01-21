//-----------------------------------------------------------------------
// <copyright file="ImageThumbnailControl.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//     Control used to display a photo thumbnail in an album.
// </summary>
//-----------------------------------------------------------------------

namespace NewsFeedSample
{
    using System;
    using System.Windows.Media;
    using Facebook;
    using Facebook.BindingHelper;

    /// <summary>
    /// Control used to display a friend image thumbnail.
    /// </summary>
    public class ImageThumbnailControl : ImageBaseControl
    {
        /// <summary>
        /// Initializes a new instance of the PhotoThumbnailControl class.
        /// Adds a render transform for animating via XAML styles.
        /// </summary>
        public ImageThumbnailControl()
        {
            ScaleTransform thumbnailScaleTransform = new ScaleTransform(1.0, 1.0);
            TransformGroup thumbnailTransformGroup = new TransformGroup();
            thumbnailTransformGroup.Children.Add(thumbnailScaleTransform);
            this.RenderTransform = thumbnailTransformGroup;
        }

        /// <summary>
        /// Updates the content of the control to contain the image at Photo.ThumbnailUri.
        /// </summary>
        protected override void OnUpdateContent()
        {
            FacebookImage image = FacebookImage;
            if (image != null)
            {
                ImageDownloadInProgress = true;
                image.GetImageAsync(FacebookImageDimensions.Big, OnGetImageSourceCompleted);
            }
            else
            {
                ImageSource = null;
            }
        }
    }
}


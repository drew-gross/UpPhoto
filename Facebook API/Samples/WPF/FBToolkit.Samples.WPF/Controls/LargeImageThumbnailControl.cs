//-----------------------------------------------------------------------
// <copyright file="LargeImageThumbnailControl.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
//     Control used to display a photo thumbnail in an album using the full-size image.
// </summary>
//-----------------------------------------------------------------------

namespace NewsFeedSample
{
    using System.Windows.Media;
    using Facebook;
    using System;
    using Facebook.BindingHelper;

    /// <summary>
    /// Control used to display a photo thumbnail in an album.
    /// </summary>
    public class LargeImageThumbnailControl : ImageThumbnailControl
    {
        /// <summary>
        /// Updates the content of the control to contain the image at Photo.ImageUri.
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


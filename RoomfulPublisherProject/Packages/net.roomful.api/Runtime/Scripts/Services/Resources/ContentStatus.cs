// Copyright Roomful 2013-2020. All rights reserved.

namespace net.roomful.api {
    /// <summary>
    /// Status of the content.
    /// </summary>
    public enum ContentStatus {
        /// <summary>
        /// Uploading or to be uploaded
        /// </summary>
        Pending,
        /// <summary>
        /// Unavailable until generation/conversion is complete.
        /// </summary>
        Processing,
        /// <summary>
        /// Is ready to be used.
        /// </summary>
        Ready,
        /// <summary>
        /// Error occured.
        /// </summary>
        Failed,
    }
}
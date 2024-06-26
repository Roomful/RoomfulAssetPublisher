using System.Collections.Generic;

namespace net.roomful.api.assets
{
    /// <summary>
    /// Builder for the asset search.
    /// </summary>
    public interface IPropAssetsSearchRequestBuilder
    {
        /// <summary>
        /// Defines asset id to search.
        /// </summary>
        /// <param name="id">Id of the asset.</param>
        void SetId(string id);

        /// <summary>
        /// Defines asset placement to search.
        /// </summary>
        /// <param name="placing">Asset placement.</param>
        void SetPlacing(string placing);

        /// <summary>
        /// Defines asset name to search.
        /// </summary>
        /// <param name="title">Asset title. Or part of the asset title.</param>
        void SetTitle(string title);

        /// <summary>
        /// Defines asset tags to search.
        /// </summary>
        /// <param name="tags">Asset tags.</param>
        void SetTags(IReadOnlyList<string> tags);

        /// <summary>
        /// Defines asset contains to search.
        /// </summary>
        /// <param name="content">Asset content types to search.</param>
        void SetContent(IReadOnlyList<string> content);
    }
}

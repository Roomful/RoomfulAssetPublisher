namespace net.roomful.api
{
    /// <summary>
    /// Social account model.
    /// </summary>
    public interface ISocialAccount
    {
        /// <summary>
        /// Social account kind, so far we have:
        /// google/apple/facebook/epam/verus
        /// </summary>
        SocialAccountKind Kind { get; }

        /// <summary>
        /// Social account unique identifier.
        /// </summary>
        string SocialId { get; }

        /// <summary>
        /// Indicates if social account is connected.
        /// </summary>
        bool Connected { get; }
    }
}

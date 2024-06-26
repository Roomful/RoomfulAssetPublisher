namespace net.roomful.api.authentication
{
    /// <summary>
    /// Contains domains related to the roomful builds with and without an editor.
    /// </summary>
    public interface IAuthenticationDomain
    {
        /// <summary>
        /// Default domain for presentation (without redactor).
        /// </summary>
        string PresentationDomain { get; }

        /// <summary>
        /// Default domain for studio (with redactor).
        /// </summary>
        string StudioDomain { get; }
    }
}

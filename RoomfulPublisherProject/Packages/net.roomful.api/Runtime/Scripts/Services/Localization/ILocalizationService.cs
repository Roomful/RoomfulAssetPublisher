namespace net.roomful.api.localization
{
    /// <summary>
    /// Use this service to support localized content.
    /// </summary>
    public interface ILocalizationService
    {
        /// <summary>
        /// Returns localized string by token.
        /// <param name="token"> Localization token.</param>
        /// </summary>
        string GetLocalizedString(string token);

        /// <summary>
        /// Returns localized string by token.
        /// <param name="token"> localization token.</param>
        /// <param name="section"> localization section.</param>
        /// </summary>
        string GetLocalizedString(string token, LangSection section);
    }
}

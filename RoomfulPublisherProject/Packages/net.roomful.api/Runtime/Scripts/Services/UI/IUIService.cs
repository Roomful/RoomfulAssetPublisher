namespace net.roomful.api.ui
{
    /// <summary>
    /// Use to perform common Roomful UI tools and effects.
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// Shows full screen transparent preloader.
        /// </summary>
        void ShowTransparentPreloader();

        /// <summary>
        /// Hides  full screen transparent preloader.
        /// </summary>
        void HideTransparentPreloader();
    }
}

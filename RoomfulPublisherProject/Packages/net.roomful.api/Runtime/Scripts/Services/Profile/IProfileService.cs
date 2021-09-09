namespace net.roomful.api.profile
{
    /// <summary>
    /// Used to display users profile
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Open profile page for specified user.
        /// </summary>
        /// <param name="userId">User if you want to view in the profile.</param>
        void Show(string userId);

        /// <summary>
        /// Hides profile if it was open.
        /// </summary>
        void Hide();
    }
}

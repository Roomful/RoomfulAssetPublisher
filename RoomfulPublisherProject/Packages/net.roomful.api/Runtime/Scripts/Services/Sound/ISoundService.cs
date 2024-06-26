namespace net.roomful.api.sound
{
    /// <summary>
    /// Use for playing sounds in Roomful.
    /// </summary>
    public interface ISoundService
    {

        /// <summary>
        /// Play generic sound one time.
        /// </summary>
        /// <param name="sound">Enum representation of the sounds you want to play.</param>
        void PlayGenericSound(GenericSound sound);
    }
}
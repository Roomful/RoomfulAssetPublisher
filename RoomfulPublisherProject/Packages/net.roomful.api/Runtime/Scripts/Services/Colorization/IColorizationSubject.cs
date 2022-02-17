namespace net.roomful.api.colorization
{
    public interface IColorizationSubject
    {
        /// <summary>
        /// Applies colorization scheme for a subject.
        /// </summary>
        void ApplyColorizationScheme(ColorizationScheme colorizationScheme);
    }
}

namespace net.roomful.api
{
    public static class ResourceEnum
    {
        /// <summary>
        /// Server lowers param keys, so we should not use upper case buttons.
        /// audioVolume used in the storyline only... so I would prefer not to touch it
        /// until we have some reports about that.
        /// </summary>
        public enum ParamsKeys {
            weblink,
            weblinkdescription,
            audio,
            audioVolume,
            author,
        }
        
        public enum Data {
            Audio,
            Video,
            Amazon,
            Imdb,
            Youtube,
            Pinterest,
            Pixabay,
            Facebook,
            RemoteUrl,
            Directory
        }
    }
}
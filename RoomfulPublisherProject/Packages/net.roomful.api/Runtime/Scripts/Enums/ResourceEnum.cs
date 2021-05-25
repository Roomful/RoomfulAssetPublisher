namespace net.roomful.api
{
    public static class ResourceEnum
    {
        public enum ParamsKeys {
            webLink,
            webLinkDescription,
            audio,
            audioVolume,
            cutoutColor,
            author
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
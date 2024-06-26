namespace net.roomful.assets.editor
{
    public static class RoomfulPublisher
    {

        static PublisherSession s_Session;
        public static PublisherSession Session => s_Session ??= new PublisherSession();
    }
}

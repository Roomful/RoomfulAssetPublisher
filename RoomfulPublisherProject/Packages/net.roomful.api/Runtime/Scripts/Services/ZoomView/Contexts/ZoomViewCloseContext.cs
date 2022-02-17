namespace net.roomful.api.zoom
{
    public struct ZoomViewCloseContext
    {
        public ZoomViewContext Context;
        public bool AutoExit;

        public ZoomViewCloseContext(ZoomViewContext context) : this(context, false) { }

        public ZoomViewCloseContext(ZoomViewContext context, bool autoExit) {
            Context = context;
            AutoExit = autoExit;
        }
    }
}

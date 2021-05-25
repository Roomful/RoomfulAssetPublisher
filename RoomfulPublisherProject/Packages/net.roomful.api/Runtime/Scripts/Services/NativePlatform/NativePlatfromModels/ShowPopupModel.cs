namespace net.roomful.api.native
{
    public class ShowPopupModel
    {
        public string Title { get; }
        public string Content { get; }

        public ShowPopupModel(string title, string content) {
            Title = title;
            Content = content;
        }
    }
}

namespace net.roomful.api
{
    public class AvatarCustomizationParams
    {
        public string UserId { get; }
        public UrlLink Url => new UrlLink($"{Roomful.WebAPIUrl}{Roomful.Session.Id}/user/{UserId}/edit/edit-3d-avatar");
        public string Token => Roomful.Session.Id;

        public AvatarCustomizationParams(string userId) {
            UserId = userId;
        }
    }
}
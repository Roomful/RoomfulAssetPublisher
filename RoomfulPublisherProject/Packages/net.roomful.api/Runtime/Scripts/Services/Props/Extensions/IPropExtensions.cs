namespace net.roomful.api.props
{
    // ReSharper disable once InconsistentNaming
    public static class IPropExtensions
    {
        public static bool IsDestroyed(this IProp prop) {
            return prop == null || prop.IsGameObjectDestroyed();
        }

        public static bool IsLoading(this IProp prop) {
            return prop.GetActiveLoader() != null;
        }

        public static string GetTitleWithAssetNameFallback(this IProp prop) {
            var title = prop.GetTitle();
            if (string.IsNullOrEmpty(title)) {
                title = prop.Asset.Title;
            }

            return title;
        }

        public static bool IsChildOfBooth(this IProp prop) {
            if (string.IsNullOrEmpty(prop.Template.ParentId))
                return false;

            var parent = Roomful.PropsService.GetCurrentRoomPropById(prop.Template.ParentId);
            return parent != null && parent.Template.Type == PropInvokeType.Container;
        }
    }
}

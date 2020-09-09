namespace net.roomful.assets.Editor
{
    internal static class RequestManager
    {
        public static void RemoveAsset(Template tpl) {
            var removeRequest = new Network.Request.RemoveAsset(tpl.Id);

            removeRequest.PackageCallbackData = removeCallback => {
                AssetBundlesSettings.Instance.RemoveSavedTemplate(tpl);
            };

            removeRequest.Send();
        }
    }
}
namespace net.roomful.assets.editor
{
    internal static class RequestManager
    {
        public static void RemoveAsset(AssetTemplate tpl) {
            var removeRequest = new RemoveAsset(tpl.Id);

            removeRequest.PackageCallbackData = removeCallback => {
                AssetBundlesSettings.Instance.RemoveSavedTemplate(tpl);
            };

            removeRequest.Send();
        }
    }
}
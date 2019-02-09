namespace RF.AssetWizzard.Results {

    public class GetResourceThumbnailUrlResult : BaseCommandResult {

        public string Url { get; }

        public GetResourceThumbnailUrlResult() : base(false) {}

        public GetResourceThumbnailUrlResult(string url) : base(true) {
            Url = url;
        }
    }
}
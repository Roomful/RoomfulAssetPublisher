using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public class AssetDataModel : PropRelatedTemplate
    {
        public string Title { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<AssetUrl> Urls { get; set; } = new List<AssetUrl>();
        public ResourceDataModel IconData { get; set; }

        public AssetDataModel() { }

        /// <summary>
        /// Parse values from `JSONData`.
        /// </summary>
        /// <param name="assetData">JSON Asset data.</param>
        public AssetDataModel(JSONData assetData):base(assetData) {
            if(assetData.HasValue("title"))
            {
                Title = assetData.GetValue<string>("title");
            }

            if (assetData.HasValue("tags")) {
                var tags = assetData.GetValue<List<object>>("tags");
                foreach (var tag in tags) {
                    var tagName = Convert.ToString(tag);
                    Tags.Add(tagName);
                }
            }

            if (assetData.HasValue("urls")) {
                var urlsList = assetData.GetValue<Dictionary<string, object>>("urls");
                if (urlsList != null) {
                    foreach (var pair in urlsList) {
                        var url = new AssetUrl(pair.Key, Convert.ToString(pair.Value));
                        Urls.Add(url);
                    }
                }
            }

            if (assetData.HasValue("thumbnail")) {
                var resInfo = new JSONData(assetData.GetValue<Dictionary<string, object>>("thumbnail"));
                IconData = new ResourceDataModel(resInfo);
            }
        }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            data.Add("title", Title);
            var tags = new List<object>();
            foreach (var t in Tags) {
                tags.Add(t);
            }

            data.Add("tags", tags);
            var urls = new Dictionary<string, object>();
            foreach (var url in Urls) {
                urls.Add(url.Platform, url.Url);
            }

            data.Add("urls", urls);
            if (IconData != null) {
                data.Add("thumbnail", IconData.ToDictionary());
            }
            return data;
        }
    }
}

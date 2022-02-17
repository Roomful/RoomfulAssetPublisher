using System;
using System.Collections.Generic;

namespace net.roomful.api.assets
{
    /// <summary>
    /// The asset data mode,l.
    /// </summary>
    public class AssetDataModel : PropRelatedTemplate
    {
        /// <summary>
        /// Title of the asset.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Asset tags.
        /// </summary>
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Asset download urls for each asset supported platform.
        /// </summary>
        public List<AssetUrl> Urls { get; set; } = new List<AssetUrl>();

        /// <summary>
        /// Asset icon data.
        /// </summary>
        public ResourceDataModel IconData { get; set; }

        /// <summary>
        /// Creates dummy asset mode.
        /// </summary>
        public AssetDataModel() { }

        /// <summary>
        /// Parse values from `JSONData`, and creates new asset mode instance.
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

        /// <summary>
        /// Converts asset model data to the key/value dictionary.
        /// </summary>
        /// <returns></returns>
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

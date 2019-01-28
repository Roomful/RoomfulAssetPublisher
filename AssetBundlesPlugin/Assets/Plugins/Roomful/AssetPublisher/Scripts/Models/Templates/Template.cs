using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RF.AssetWizzard
{

    [Serializable]
    public class Template 
    {

        public string Id = string.Empty;
        public string Title = string.Empty;
        public string ReleaseAssetId = string.Empty;
        public ReleaseStatus ReleaseStatus;
        public string DraftAssetId = string.Empty;
        public DateTime Created = DateTime.MinValue;
        public DateTime Updated = DateTime.MinValue;

        public List<string> Tags = new List<string>();
        public List<AssetUrl> Urls = new List<AssetUrl>();

        public Resource Icon = null;


        //--------------------------------------
        // Initialization
        //--------------------------------------


        public Template() {
            Icon = new Resource();
        }

        public Template(string data) {
            LoadData(data);
        }


        //--------------------------------------
        // Abstract Methods
        //--------------------------------------



        //--------------------------------------
        // Public Methods
        //--------------------------------------

        
        public void LoadData(string data) {
            ParseData(new JSONData(data));
        }

        public virtual Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> OriginalJSON = new Dictionary<string, object>();

            OriginalJSON.Add("id", Id);
            OriginalJSON.Add("title", Title);
            OriginalJSON.Add("created", SA.Common.Util.General.DateTimeToRfc3339(Created));
            OriginalJSON.Add("updated", SA.Common.Util.General.DateTimeToRfc3339(Updated));
            OriginalJSON.Add("releaseStatus", ReleaseStatus.ToString());
            OriginalJSON.Add("releasedAssetId", ReleaseAssetId);
            OriginalJSON.Add("draftAssetId", DraftAssetId);

            if (Icon != null) {
                OriginalJSON.Add("thumbnail", Icon.ToDictionary());
            }

            OriginalJSON.Add("tags", Tags);


            return OriginalJSON;
        }

        public virtual void ParseData(JSONData assetData) {
            Id = assetData.GetValue<string>("id");
            Created = assetData.GetValue<DateTime>("created");
            Updated = assetData.GetValue<DateTime>("updated");
            Title = assetData.GetValue<string>("title");

            if (assetData.HasValue("thumbnail")) {
                var resInfo = new JSONData(assetData.GetValue<Dictionary<string, object>>("thumbnail"));
                Icon = new Resource(resInfo);
            } else {
                Icon = new Resource();
            }

            ReleaseStatus = (ReleaseStatus) Enum.Parse(typeof(ReleaseStatus), assetData.GetValue<string>("releaseStatus"));
            ReleaseAssetId = assetData.GetValue<string>("releasedAssetId");
            DraftAssetId = assetData.GetValue<string>("draftAssetId");
            if (assetData.HasValue("tags")) {
                List<object> tags = assetData.GetValue<List<object>>("tags");

                if (tags != null) {
                    foreach (object tag in tags) {
                        string tagName = System.Convert.ToString(tag);
                        Tags.Add(tagName);
                    }
                }
            }


            if (assetData.HasValue("urls")) {
                var urlsList = assetData.GetValue<Dictionary<string, object>>("urls");
                if (urlsList != null) {
                    foreach (var pair in urlsList) {
                        AssetUrl url = new AssetUrl(pair.Key, System.Convert.ToString(pair.Value));
                        Urls.Add(url);
                    }
                }
            }


        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public GUIContent DisaplyContent {
            get {
                GUIContent content = new GUIContent();
                if (Icon != null && Icon.Thumbnail != null) {
                    content.image = Icon.Thumbnail;
                }

                content.text = Title;
                return content;
            }
        }

        public bool IsNew {
            get {
                return string.IsNullOrEmpty(Id);
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace RF.AssetWizzard
{
    public abstract class Template 
    {

        public string Id = string.Empty;
        public string Title = string.Empty;
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
            ParseData(new JSONData(data));
        }



        //--------------------------------------
        // Public Methods
        //--------------------------------------


        public virtual Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> OriginalJSON = new Dictionary<string, object>();

            OriginalJSON.Add("id", Id);
            OriginalJSON.Add("title", Title);
            OriginalJSON.Add("created", SA.Common.Util.General.DateTimeToRfc3339(Created));
            OriginalJSON.Add("updated", SA.Common.Util.General.DateTimeToRfc3339(Updated));
          
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

    }
}
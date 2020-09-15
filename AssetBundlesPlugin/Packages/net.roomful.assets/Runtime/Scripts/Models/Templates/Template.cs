﻿using System.Collections.Generic;
using UnityEngine;
using System;
using net.roomful.api;
using SA.Foundation.Time;


namespace net.roomful.assets
{

    [Serializable]
    public class Template 
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
            var OriginalJSON = new Dictionary<string, object>();

            OriginalJSON.Add("id", Id);
            OriginalJSON.Add("title", Title);
            OriginalJSON.Add("created", SA_Rfc3339_Time.ToRfc3339(Created));
            OriginalJSON.Add("updated", SA_Rfc3339_Time.ToRfc3339(Updated));
          
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
                var tags = assetData.GetValue<List<object>>("tags");

                if (tags != null) {
                    foreach (var tag in tags) {
                        var tagName = Convert.ToString(tag);
                        Tags.Add(tagName);
                    }
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


        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public GUIContent DisaplyContent {
            get {
                var content = new GUIContent();
                if (Icon != null && Icon.Thumbnail != null) {
                    content.image = Icon.Thumbnail;
                }

                content.text = Title;
                return content;
            }
        }

        public bool IsNew => string.IsNullOrEmpty(Id);
    }
}
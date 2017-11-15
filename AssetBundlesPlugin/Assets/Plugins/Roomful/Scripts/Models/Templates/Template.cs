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
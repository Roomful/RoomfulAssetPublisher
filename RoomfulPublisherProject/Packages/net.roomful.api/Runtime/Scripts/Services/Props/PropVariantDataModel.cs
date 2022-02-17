using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public class PropVariantDataModel : PropRelatedTemplate
    {
        /// <summary>
        /// Name of the Variant.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Used to control the order of how available prop variants are presented to the user.
        /// The bigger sort order number means the variant will have higher priority (will be displayed on top.)
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// List of gameObjects paths that participate in teh variant.
        /// </summary>
        public List<string> GameobjectsNames { get; set; }

        /// <summary>
        /// List Of game object compiled using <see cref="GameobjectsNames"/>.
        /// <see cref="FinRenderers"/> method must be called.
        /// </summary>
        public List<GameObject> GameObjects { get; set; } = new List<GameObject>();

        /// <summary>
        /// Variant thumbnail.
        /// </summary>
        public ResourceDataModel ThumbnailData { get; set; }

        /// <summary>
        /// Variant supports color.
        /// </summary>
        public bool HasColorSupport { get; set; }

        /// <summary>
        /// Variant default color.
        /// </summary>
        public Color DefaultColor { get; set; } = Color.white;

        public PropVariantDataModel() { }

        public PropVariantDataModel(JSONData metaData):base(metaData) {
            Name = metaData.GetValue<string>("name");

            GameobjectsNames = new List<string>();
            var gameObjectList = metaData.HasValue("gameobjects") ? metaData.GetValue<List<object>>("gameobjects") : new List<object>();
            gameObjectList.ForEach(t => {
                GameobjectsNames.Add(Convert.ToString(t));
            });

            if (metaData.HasValue("thumbnail")) {
                var resInfo = new JSONData(metaData.GetValue<Dictionary<string, object>>("thumbnail"));
                ThumbnailData = new ResourceDataModel(resInfo);
            }

            if (metaData.HasValue("sortOrder")) {
                SortOrder = metaData.GetValue<int>("sortOrder");
            }

            if (metaData.HasValue("hasColorSupport")) {
                HasColorSupport = metaData.GetValue<bool>("hasColorSupport");
            }

            if (metaData.HasValue("defaultColor")) {
                var defaultColor = new JSONData(metaData.GetValue<Dictionary<string, object>>("defaultColor"));
                DefaultColor = new Color {
                    r = defaultColor.GetValue<float>("r"),
                    g = defaultColor.GetValue<float>("g"),
                    b = defaultColor.GetValue<float>("b"),
                    a = defaultColor.GetValue<float>("b")
                };
            }
        }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();

            data.Add("name", Name);
            var gameobjects = new List<object>();
            foreach (var r in GameobjectsNames) {
                gameobjects.Add(r);
            }

            if (ThumbnailData != null) {
                data.Add("thumbnail", ThumbnailData.ToDictionary());
            }

            var defaultColor = new Dictionary<string, object>();
            defaultColor.Add("r", DefaultColor.r);
            defaultColor.Add("g", DefaultColor.g);
            defaultColor.Add("b", DefaultColor.b);
            defaultColor.Add("a", DefaultColor.a);
            data.Add("defaultColor", defaultColor);

            data.Add("sortOrder", SortOrder);
            data.Add("hasColorSupport", HasColorSupport);
            data.Add("gameobjects", gameobjects);
            return data;
        }

        public void FinAssignedRenderers(Transform root, List<GameObject> assignedRenderers) {

            foreach (var gameObjectName in GameobjectsNames) {
                var names = gameObjectName.Split('/');

                var rendererObject = root;
                foreach (var name in names) {
                    rendererObject = rendererObject.Find(name);
                    if(rendererObject == null)
                        break;
                }

                if (rendererObject == null) {
                    Debug.LogError("Failed to find Transform for: " + gameObjectName);
                }
                else {
                    assignedRenderers.Add(rendererObject.gameObject);
                }
            }
        }
    }
}

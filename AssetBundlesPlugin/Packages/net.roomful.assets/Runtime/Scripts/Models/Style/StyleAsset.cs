using UnityEngine;

using net.roomful.assets.serialization;
using StansAssets.Foundation.Extensions;

namespace net.roomful.assets
{

    [ExecuteInEditMode]
    public class StyleAsset : Asset<StyleTemplate>
    {

        public bool ShowWalls = false;
        public bool ShowEditUI = false;
        

        public void SetTemplate(StyleTemplate tpl) {
            _Template = tpl;
        }


        //--------------------------------------
        // Unity Editor
        //--------------------------------------


        public void Update() {
            CheckHierarchy();
        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        [ContextMenu("Prepare For Upload")]
        public override void PrepareForUpload() {

            foreach (var panel in Panels) {
                panel.PrepareForUpload();
            }


            Template.Metadata = new StyleMetadata(this);
  
            CleanUpSilhouette();
            PrepareComponentsForUpload();

            ShowEditUI = false;
            ShowWalls = false;
            UpdateDefaultElements();
        }


        //--------------------------------------
        // Get / Set
        //--------------------------------------


        public SerializedStyle Settings {
            get {

                var settings = GetComponent<SerializedStyle>();
                if (settings == null) {
                    settings = gameObject.AddComponent<SerializedStyle>();
                }

                settings.hideFlags = HideFlags.HideInInspector;

                return settings;
            }
        }

        public StylePanel[] Panels => transform.GetComponentsInChildren<StylePanel>();

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        protected override void PrepareComponentsForUpload() {
            base.PrepareComponentsForUpload();

            foreach(var panel in Panels) {
                DestroyImmediate(panel);
            }
        }

            public void UpdateDefaultElements() {
           var DefaultElements =  GameObject.Find("DefaultElements");
            if(DefaultElements !=  null) {
                DestroyImmediate(DefaultElements);
            }

            DefaultElements = new GameObject("DefaultElements");
            DefaultElements.transform.Reset();
            DefaultElements.hideFlags = HideFlags.HideInHierarchy;

            if (ShowWalls) {
                PlaceWalls(DefaultElements);
            }

            if(ShowEditUI) {
                PlaceEditUI(DefaultElements);
            }

            if(DefaultElements.transform.childCount == 0) {
                DestroyImmediate(DefaultElements);
            }

            }

            private void PlaceEditUI(GameObject defaultElements) {
                foreach (var panel in Panels) {
                    var removeUI = PrefabManager.CreatePrefab("Style/RemoveUI");
                    removeUI.transform.parent = defaultElements.transform;

                    removeUI.transform.position = panel.Bounds.GetVertex(SA_VertexX.Center, SA_VertexY.Center, SA_VertexZ.Back);

                }
            }

            private void PlaceWalls(GameObject DefaultElements) {
                var startWall = PrefabManager.CreatePrefab("Style/RoomGlassWall");
                startWall.transform.parent = DefaultElements.transform;
                startWall.transform.position = Panels[0].Bounds.GetVertex(SA_VertexX.Left, SA_VertexY.Bottom, SA_VertexZ.Front);


                var endtWall = PrefabManager.CreatePrefab("Style/RoomGlassWall");
                endtWall.transform.parent = DefaultElements.transform;

                var index = Panels.Length - 1;
                endtWall.transform.position = Panels[index].Bounds.GetVertex(SA_VertexX.Right, SA_VertexY.Bottom, SA_VertexZ.Front);
            }


    }
}
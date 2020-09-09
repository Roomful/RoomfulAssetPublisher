using System.Globalization;
using net.roomful.assets.serialization;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal static class EditorMenu
    {
        private const string MENU_ROOT = "Roomful/Assets/";
        //--------------------------------------
        //  Top Menu
        //--------------------------------------

        [MenuItem(MENU_ROOT+ "Wizzard &w", false, 100)]
        public static void ShowWizzard() {
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            WindowManager.ShowWizard();
        }

        [MenuItem(MENU_ROOT+ "Create/Text &t", false, 0)]
        public static void ShowWizzard2() {
            AddTextComponent();
        }

        [MenuItem(MENU_ROOT+ "Create/Thumbnail", false, 1)]
        public static void ShowWizzard3() {
            SetAsThumbnail();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Mesh Thumbnail", false, 2)]
        public static void ShowWizzard5() {
            MarkAsThumbnail();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Frame &#b", false, 3)]
        public static void ShowWizzard4() {
            AddStretchedFrame();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Tiled Frame", false, 3)]
        public static void ShowWizzard9() {
            AddTiledFrame();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Floor &#f", false, 4)]
        public static void ShowWizzard6() {
            MarkAsStand();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Ignore Bounds &#i", false, 5)]
        public static void ShowWizzard7() {
            IgnoreObjectBounds();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Placing Disabled &#d", false, 5)]
        public static void ShowWizzard77() {
            PlacingDisabled();
        }

        [MenuItem(MENU_ROOT+ "Add Component/No Mirror &#m", false, 5)]
        public static void ShowWizzard78() {
            NoMirror();
        }

        [MenuItem(MENU_ROOT+ "Add Component/Add Anchor &#a", false, 5)]
        public static void ShowWizzard8() {
            AddAnchor();
        }

        //--------------------------------------
        //  Context Menu
        //--------------------------------------

        [MenuItem("GameObject/Roomful Assets/Text", false, 0)]
        private static void AddTextComponent() {
            var text = new GameObject("Text").AddComponent<RoomfulText>();
            text.RectTransform.sizeDelta = new Vector2(1f, 1f / 5f);

            //text.transform.localRotation = Quaternion.Euler(0f, 180f, 0f); 

            Selection.activeObject = text.gameObject;
        }

        [MenuItem("GameObject/Roomful Assets/Thumbnail", false, 1)]
        private static void SetAsThumbnail() {
            var thumbnail = new GameObject("Thumbnail");
            thumbnail.AddComponent<PropThumbnail>();
            thumbnail.transform.localScale = Vector3.one * 1.5f;

            Selection.activeGameObject = thumbnail;
        }

        [MenuItem("GameObject/Roomful Assets/CanvasMirror", false, 1)]
        private static void CanvasMirror() {
            var canvas = GameObject.CreatePrimitive(PrimitiveType.Quad);
            canvas.name = "Canvas Mirror";
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Anchor", false, 104)]
        private static void AddAnchor() {
            Selection.activeGameObject.AddComponent<PropAnchor>();
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Prop Asset Settings", false, 104)]
        private static void AddPropAssetSettings() {
            Selection.activeGameObject.AddComponent<PropAssetSettings>();
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Frame", false, 100)]
        public static void AddStretchedFrame() {
            var frame = Selection.activeGameObject.AddComponent<PropStretchedFrame>();
            AddDefaultBorderParts(frame);
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Tiled Frame", false, 101)]
        public static void AddTiledFrame() {
            var frame = Selection.activeGameObject.AddComponent<PropTiledFrame>();
            AddDefaultBorderParts(frame);
        }

        private static void AddDefaultBorderParts(AbstractPropFrame frame) {
            var border = PrefabManager.CreatePrefab("Frame/DefaultBorder");
            var corner = PrefabManager.CreatePrefab("Frame/DefaultCorner");
            var back = PrefabManager.CreatePrefab("Frame/DefaultBacking");

            frame.Border = border;
            frame.Corner = corner;
            frame.Back = back;

            frame.SetBackOffset(-0.001f);
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Floor", false, 101)]
        private static void MarkAsStand() {
            var valid = IsValidPropGameobject();
            if (Selection.activeGameObject.GetComponent<BoxCollider>() == null) {
                valid = false;
                EditorUtility.DisplayDialog("Error", "Object should have BoxCollider component", "Ok");
            }

            if (valid) {
                Selection.activeGameObject.AddComponent<SerializedFloorMarker>();
            }
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Mesh Thumbnail", false, 102)]
        private static void MarkAsThumbnail() {
            var valid = IsValidPropGameobject();

            if (valid) {
                var r = Selection.activeGameObject.GetComponent<Renderer>();
                if (r == null) {
                    EditorUtility.DisplayDialog("Error", "Object should have Renderer component", "Ok");
                    return;
                }

                if (Selection.activeGameObject.GetComponent<PropMeshThumbnail>() != null) {
                    EditorUtility.DisplayDialog("Error", "PropThumbnailPointer", "Ok");
                    return;
                }

                Selection.activeGameObject.AddComponent<PropMeshThumbnail>().Update();
            }
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Ignore Bounds", false, 103)]
        private static void IgnoreObjectBounds() {
            Selection.activeGameObject.AddComponent<SerializedBoundsIgnoreMarker>();
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Placing Disabled", false, 104)]
        private static void PlacingDisabled() {
            var valid = IsStyleGameobject();
            if (valid) {
                if (Selection.activeGameObject.GetComponent<BoxCollider>() == null) {
                    EditorUtility.DisplayDialog("Error", "Object should have a BoxCollider component", "Ok");
                    return;
                }

                Selection.activeGameObject.AddComponent<SerializedDisabledAreaMarker>();
            }
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/No Mirror", false, 104)]
        private static void NoMirror() {
            var valid = IsStyleGameobject();
            if (valid) {
                Selection.activeGameObject.AddComponent<SerializedNoMirrorMarker>();
            }
        }

        [MenuItem("GameObject/Roomful Assets/Add Component/Focus Pointer", false, 104)]
        private static void FocusPointer() {
            var valid = IsValidPropGameobject();
            if (valid) {
                Selection.activeGameObject.AddComponent<SerializedFocusPointer>();
            }
        }

        private static bool IsStyleGameobject() {
            var style = Object.FindObjectOfType<StyleAsset>();
            var valid = true;

            if (style == null) {
                EditorUtility.DisplayDialog("Error", "No valid style found", "Ok");
                return false;
            }

            if (style.gameObject == Selection.activeGameObject) {
                EditorUtility.DisplayDialog("Error", "Select a child object", "Ok");
                valid = false;
            }

            return valid;
        }

        private static bool IsValidPropGameobject() {
            var prop = Object.FindObjectOfType<PropAsset>();

            var valid = true;
            if (prop == null) {
                EditorUtility.DisplayDialog("Error", "No valid prop found", "Ok");
                valid = false;
            }

            if (prop.gameObject == Selection.activeGameObject) {
                EditorUtility.DisplayDialog("Error", "Select a child object", "Ok");
                valid = false;
            }

            return valid;
        }
    }
}
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    [InitializeOnLoad]
    internal static class SceneViewOptionsWindow
    {
        private const int WINDOW_ID = 5162389;
        private const int WINDOW_TEX_ID = 6162389;
        private static SceneView s_sceneView;

        private static Rect s_position = Rect.zero;
        private static Rect s_windowRect = Rect.zero;

        private static UnityEditor.Editor s_propEditor = null;
        private static bool s_isMouseOverIcon = false;

        private static Rect s_ionRect;
        private static PropAsset Asset { get; set; }

        static SceneViewOptionsWindow() {
#if UNITY_2019_4_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
        }

        public static void RegisterPropAsset(PropAsset propAsset) {
            Asset = propAsset;
        }

        public static void RemoveActiveAsset() {
            Asset = null;
        }

        private static void OnSceneGUI(SceneView sceneView) {
            if (Asset == null) {
                return;
            }

            s_sceneView = sceneView;

            if (WindowX < 0) {
                WindowX = 0;
            }

            var maxX = sceneView.position.width - s_windowRect.width;
            if (WindowX > maxX) {
                WindowX = maxX;
            }

            s_position.x = WindowX;
            s_position.y = 20;
            s_windowRect = GUILayout.Window(WINDOW_ID, s_position, OnWindowGui, Asset.name, GUILayout.ExpandHeight(true));
            WindowX = s_windowRect.x;

            if (s_isMouseOverIcon) {
                s_ionRect = new Rect(
                    Event.current.mousePosition.x + 20,
                    Event.current.mousePosition.y + 40,
                    Asset.Icon.width,
                    Asset.Icon.height + 20);

                GUI.Window(WINDOW_TEX_ID, s_ionRect, OnTExtureWindowGui, "Asset Icon");
            }

            if (Event.current.type == EventType.Repaint) {
                if (s_windowRect.Contains(Event.current.mousePosition)) {
                    Repaint();
                }
            }
        }

        public static void Repaint() {
            SceneView.RepaintAll();
        }

        private static void OnTExtureWindowGui(int id) {
            var rect = new Rect(s_ionRect);
            rect.x = 0;
            rect.y = 20;
            rect.height -= 20;
            GUI.DrawTexture(rect, Asset.Icon);
        }

        private static bool s_useEditorCameraPosition = false;

        private static void OnWindowGui(int id) {
            EditorGUILayout.LabelField("Icon: ", EditorStyles.boldLabel);

            using (new IMGUIBeginHorizontal()) {
                Asset.Icon = (Texture2D) EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

                if (Event.current.type == EventType.Repaint) {
                    var lastRect = GUILayoutUtility.GetLastRect();
                    s_isMouseOverIcon = lastRect.Contains(Event.current.mousePosition);
                }

                EditorGUILayout.Space();
                using (new IMGUIBeginVertical()) {
                    EditorGUILayout.Space();
                    s_useEditorCameraPosition = EditorGUILayout.Toggle("Use Editor Camera", s_useEditorCameraPosition);
                    var createIcon = GUILayout.Button("Make Icon");
                    if (createIcon) {
                        PropAssetScreenshotTool.CreateIcon(s_useEditorCameraPosition, Asset);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Asset: ", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            using (new IMGUIBeginHorizontal()) {
                var def = Asset.Size * 100f * Asset.Scale;

                EditorGUILayout.LabelField("Size(mm): " + (int) def.x + "x" + (int) def.y + "x" + (int) def.z);
            }

            Asset.Scale = EditorGUILayout.Slider(Asset.Scale, Asset.MinScale, Asset.MaxScale);


            DrawGizmosSiwtch();
            DrawEnvironmentSiwtch();

            if (EditorGUI.EndChangeCheck()) {
                Asset.Update();
            }

            float btnWidth = 90;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions: ", EditorStyles.boldLabel);

            if (Asset.GetTemplate().IsNew) {
                var upload = GUILayout.Button("Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (upload) {
                    PropWizard.UploadProp(Asset);
                }
            }
            else {
                using (new IMGUIBeginHorizontal()) {
                    var upload = GUILayout.Button("Re-Upload", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (upload) {
                        PropWizard.UploadProp(Asset);
                    }

                    var meta = GUILayout.Button("Update Meta", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (meta) {
                        PropWizard.UpdateMeta(Asset);
                    }

                    var refresh = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                    if (refresh) {
                        PropWizard.DownloadProp(Asset.Template);
                    }
                }
            }

            using (new IMGUIBeginHorizontal()) {
                var wizzard = GUILayout.Button("Wizzard", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (wizzard) {
                    WindowManager.ShowWizard();
                    WindowManager.Wizzard.SwitchTab(WizardTabs.Wizzard);
                }

                var create = GUILayout.Button("New", EditorStyles.miniButton, GUILayout.Width(btnWidth));
                if (create) {
                    PropWizard.CreateProp();
                }

                if (GUILayout.Button("Variants", EditorStyles.miniButton, GUILayout.Width(btnWidth))) {
                    PropVariantsEditorWindow.Editor.Show();
                }
            }

            GUI.DragWindow(new Rect(0, 0, s_sceneView.position.width, s_sceneView.position.height));
        }

        public static void DrawEnvironmentSiwtch() {
            var env = Object.FindObjectOfType<Environment>();
            if (env != null) {
                EditorGUI.BeginChangeCheck();
                env.RenderEnvironment = EditorGUILayout.Toggle("Render Environment", env.RenderEnvironment);
                if (EditorGUI.EndChangeCheck()) {
                    env.Update();
                }
            }
        }

        public static void DrawGizmosSiwtch() {
            Asset.DrawGizmos = EditorGUILayout.Toggle("Draw Gizmos", Asset.DrawGizmos);
        }

        private static T FindObjectWithType<T>() {
            var allFindedObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (GameObject gameObject in allFindedObjects) {
                var target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }

            return default(T);
        }

        public static float WindowX {
            get {
                var key = "SceneViewOptionsWindow_WindowX";
                if (EditorPrefs.HasKey(key)) {
                    return EditorPrefs.GetFloat(key);
                }
                else {
                    return 0f;
                }
            }

            set {
                var key = "SceneViewOptionsWindow_WindowX";
                EditorPrefs.SetFloat(key, value);
            }
        }

        public static UnityEditor.Editor PropEditor {
            get {
                if (s_propEditor != null && s_propEditor.target != Asset) {
                    s_propEditor = null;
                }

                if (s_propEditor == null) {
                    s_propEditor = UnityEditor.Editor.CreateEditor(Asset);
                }

                return s_propEditor;
            }
        }
    }
}

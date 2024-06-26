using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using StansAssets.Foundation;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace net.roomful.assets.editor
{
    public sealed class PropVariantsEditorWindow : EditorWindow, IHasCustomMenu
    {
        private const int WINDOW_WIDTH = 800;
        private const int WINDOW_HEIGHT = 300;
        private readonly Vector2 m_minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
        private PropVariant m_selectedVariant;
        private PropSkin m_selectedSkin;
        private PropAsset m_asset;
        private PropVariantsEditorWindowUI m_windowUI;
#if UNITY_2019_4_OR_NEWER
        PropVariantsEditorWindow() { }

        private void OnEnable() {
            minSize = m_minSize;
            maxSize = minSize;
            m_windowUI = new PropVariantsEditorWindowUI();
            m_windowUI.ShowNotification += ShowNotification;
            m_windowUI.ShowCreateNewPropVariant += ShowCreateNewPropVariant;
            m_windowUI.ShowCreateNewSkinVariant += ShowCreateNewSkinVariant;
            m_windowUI.Recreate(Asset);
            rootVisualElement.Add(m_windowUI);
        }

        private void OnGUI() {
            m_windowUI.Recreate(Asset);
        }
#endif
#if !UNITY_2019_4_OR_NEWER
        private enum SearchType
        {
            FILTERMODE_ALL = 0,
            FILTERMODE_NAME = 1,
            FILTERMODE_TYPE = 2
        }
        private const string EDIT_ICON_NAME = "edit";
        private const string REFRESH_ICON_NAME = "refresh_black";

        private const float PREVIEW_WIDTH = 80.0f;
        private const float LEFT_RECT_WIDTH_WINDOW = 300.0f;
        private const float MIDDLE_RECT_WIDTH_WINDOW = 400.0f;

        private Vector2 m_variantsScrollPos = Vector2.zero;
        private Vector2 m_skinsScrollPos = Vector2.zero;


        private VariantHierarchyView m_hierarchyView;
        private GameObjectsHierarchyView m_gameObjectsHierarchyView;

        private static bool s_useVariantEditorCameraPosition;
        private static bool s_useEditorCameraPositionSkin;
        private static bool s_disableSkinUnrelatedRenderersForScreenshot;
        private PropVariant m_selectedVariantPrevious;
        private const int WINDOW_TEX_ID = 6162389;
        private static Rect s_ionRect;
        private Texture2D m_mouseOverVariantTexture;
        private Texture2D m_mouseOverSkinTexture;
        private bool m_isMouseOverVariantIcon;
        private bool m_isMouseOverSkinIcon;
        private static Rect s_windowRect = Rect.zero;

        private List<PropVariant> m_variants = new List<PropVariant>();

        PropVariantsEditorWindow() {
#if UNITY_2019_4_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;

#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif

            GlobalEvents.OnAssetDownloadStarted += OnAssetDownloadStarted;
        }


        private void OnAssetDownloadStarted() {
            m_variants.Clear();
            m_selectedSkin = null;
            m_selectedVariant = null;
        }

        private void OnSceneGUI(SceneView sceneView) {
            if (Asset == null) {
                return;
            }

            if (m_isMouseOverVariantIcon) {
                s_ionRect = new Rect(Asset.Icon.width + 25, 20, Asset.Icon.width, Asset.Icon.height + 10);
                GUI.Window(WINDOW_TEX_ID, s_ionRect, OnTextureWindowGui, "Variant Icon");
            }

            if (m_isMouseOverSkinIcon) {
                s_ionRect = new Rect(Asset.Icon.width + 25, 20, Asset.Icon.width, Asset.Icon.height + 10);
                GUI.Window(WINDOW_TEX_ID, s_ionRect, OnTextureWindowGui, "Skin Icon");
            }

            if (Event.current.type == EventType.Repaint) {
                if (s_windowRect.Contains(Event.current.mousePosition)) {
                    SceneView.RepaintAll();
                }
            }
        }

        private void OnTextureWindowGui(int id) {
            var rect = new Rect(s_ionRect) { x = 0, y = 20 };
            GUI.DrawTexture(rect, m_isMouseOverVariantIcon ? m_mouseOverVariantTexture : m_mouseOverSkinTexture);
        }

        private void OnEnable() {
            minSize = m_minSize;
            maxSize = minSize;
        }

        private void OnGUI() {
            if (Asset == null) {
                GUILayout.Label("[No Prop Assets in Edit mode]", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            using (new IMGUIBeginHorizontal()) {
                var rect = new Rect(0.0f, 0.0f, LEFT_RECT_WIDTH_WINDOW, Screen.height);
                GUILayout.BeginArea(rect);
                GUI.Box(rect, GUIContent.none, WizardWindow.Constants.settingsBox);

                ShowVariantsPanel();
                ShowVariantsScrollView();

                GUILayout.EndArea();

                rect = new Rect(rect.x + rect.width, rect.y, MIDDLE_RECT_WIDTH_WINDOW, Screen.height);
                GUILayout.BeginArea(rect);

                var localRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
                GUI.Box(localRect, GUIContent.none, WizardWindow.Constants.settingsBox);

                if (m_selectedVariant != null) {
                    ShowPropVariantInfo();
                    ShowSkinsPanel();

                    GUILayout.Space(1.0f);

                    ShowSkinsScrollView();

                    GUILayout.FlexibleSpace();

                    ShowGameObjectsHierarchy(localRect, rect);

                    GUILayout.FlexibleSpace();
                    GUILayout.FlexibleSpace();
                    GUILayout.FlexibleSpace();
                }

                GUILayout.EndArea();

                rect = new Rect(rect.x + rect.width, rect.y, position.width - (rect.x + rect.width), position.height);
                GUILayout.BeginArea(rect);

                localRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
                GUI.Box(localRect, GUIContent.none, WizardWindow.Constants.settingsBox);

                ShowPropSkinsInfo(localRect, rect);

                GUILayout.EndArea();
            }
        }

        private void ShowVariantsPanel() {
            if (!string.IsNullOrEmpty(Asset.Template.Id) && m_selectedVariant == null) {
                VariantsService.LoadVariantsList(Asset.Template.Id, () => {
                    m_variants = VariantsService.Variants;
                    m_selectedVariant = m_variants.FirstOrDefault();
                    foreach (var variant in m_variants) {
                        variant.FinRenderers(Asset.GetLayer(HierarchyLayers.Graphics));
                    }
                });
            }

            using (new IMGUIBeginHorizontal()) {
                GUILayout.Label("Variants:", WizardWindow.Constants.settingsBoxTitle);
                var edit = Resources.Load(EDIT_ICON_NAME) as Texture2D;
                if (GUILayout.Button(edit, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    if (m_selectedVariant != null) {
                        ShowEditNameWindow(m_selectedVariant.Name, (isSuccessful, variantName) => {
                            if (isSuccessful) {
                                m_selectedVariant.Name = variantName;
                            }
                            else {
                                ShowNotification(new GUIContent("Editing variant name canceled!"));
                            }
                        });
                    }
                }

                var refreshBlack = Resources.Load(REFRESH_ICON_NAME) as Texture2D;
                if (GUILayout.Button(refreshBlack, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    VariantsService.LoadVariantsList(Asset.Template.Id, () => {
                        m_variants = VariantsService.Variants;
                        foreach (var variant in m_variants) {
                            variant.FinRenderers(Asset.GetLayer(HierarchyLayers.Graphics));
                        }
                    });
                }

                if (GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    var selection = Selection.objects.Where(o => o != null && o is GameObject && !EditorUtility.IsPersistent(o))
                        .Select(o => (GameObject) o).ToList();

                    if (selection.Count == 0) {
                        ShowNotification(new GUIContent("Variant Creation Failed! Nothing selected"));
                    }
                    else {
                        ShowCreateNewPropVariant((isSuccessful, variantName) => {
                            VariantsService.CreateVariant(variantName, Asset, selection, (variant) => {
                                m_variants.Add(variant);
                                SelectVariant(variant);
                            });
                        });

                        /*
                        if (Asset.Template.ValidateVariantCreate(selection, out var message, out var renderers)) {
                            ShowCreateNewPropVariant((isSuccessful, variantName) => {
                                if (isSuccessful) {
                                    var propVariant = new PropVariant(name, renderers);
                                    foreach (var renderer in renderers) {
                                        m_variantByRenderer[renderer] = variant;
                                    }

                                    Asset.Template.TryCreateVariant(selection, out var variant, variantName);
                                    m_selectedVariant?.ApplySkin(m_selectedVariant.DefaultSkin);
                                    variant.AddSkin(new PropVariantSkin("default", variant.MaterialDictionary, true));
                                    Asset.Template.AddVariant(variant);
                                    m_selectedVariant = variant;
                                    SelectSkin(variant.DefaultSkin);
                                    var listGO = m_selectedVariant.Renderers.Select(item => item.gameObject).ToList();
                                    m_gameObjectsHierarchyView = new GameObjectsHierarchyView(new TreeViewState(), listGO);
                                }
                                else {
                                    ShowNotification(new GUIContent("Variant creation canceled by user!"));
                                }
                            });
                        }
                        else {
                            ShowNotification(new GUIContent($"Variant Creation Failed! {message}"));
                        }*/
                    }
                }
            }
        }

        private void ShowVariantsScrollView() {
            using (new IMGUIBeginHorizontal()) {
                GUILayout.Space(1.0f);
                m_variantsScrollPos = GUILayout.BeginScrollView(m_variantsScrollPos, GUILayout.Height(Screen.height - 39.0f));
                foreach (var variant in m_variants) {
                    using (new IMGUIBeginHorizontal()) {
                        var variantLabel = new GUIContent(variant.Name);
                        var r = GUILayoutUtility.GetRect(variantLabel, PropVariantsEditorWindowStyles.VariantTitle, GUILayout.ExpandWidth(true));

                        if (m_selectedVariant == variant && Event.current.type == EventType.Repaint) {
                            var color = EditorGUIUtility.isProSkin
                                ? new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f)
                                : new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);

                            GUI.DrawTexture(r, Texture2DUtility.MakePlainColorImage(color));
                        }

                        EditorGUI.BeginChangeCheck();
                        if (GUI.Toggle(r, m_selectedVariant == variant, variantLabel, PropVariantsEditorWindowStyles.VariantTitle)) {
                            SelectVariant(variant);
                        }

                        if (EditorGUI.EndChangeCheck()) {
                            GUIUtility.keyboardControl = 0;
                        }
                    }
                }

                GUILayout.EndScrollView();
            }
        }

        private void ShowPropVariantInfo() {
            using (new IMGUIBeginHorizontal()) {
                using (new IMGUIBeginVertical()) {
                    GUILayout.Label("Prop Variant Info", PropVariantsEditorWindowStyles.HeaderLabel, GUILayout.Width(200.0f));
                    GUILayout.Space(9f);
                    GUILayout.Label(string.Concat("Id: ", m_selectedVariant.Id), new GUIStyle(EditorStyles.boldLabel), GUILayout.Width(250.0f));
                    m_selectedVariant.Name = EditorGUILayout.TextField(m_selectedVariant.Name, GUILayout.Width(250.0f));
                    using (new IMGUIBeginHorizontal()) {
                        m_selectedVariant.Thumbnail = (Texture2D) EditorGUILayout.ObjectField(m_selectedVariant.Thumbnail, typeof(Texture2D),
                            false, GUILayout.Width(70), GUILayout.Height(70));
                        m_mouseOverVariantTexture = m_selectedVariant.Thumbnail;
                        if (Event.current.type == EventType.Repaint) {
                            var lastRect = GUILayoutUtility.GetLastRect();
                            m_isMouseOverVariantIcon = lastRect.Contains(Event.current.mousePosition);
                        }

                        EditorGUILayout.Space();
                        using (new IMGUIBeginVertical()) {
                            EditorGUILayout.Space();
                            s_useVariantEditorCameraPosition = EditorGUILayout.Toggle("Use Editor Camera", s_useVariantEditorCameraPosition);
                            if (GUILayout.Button("Make Icon")) {
                                CreateVariantPreviewIcon();
                            }
                        }
                    }

                    EditorGUILayout.Space();
                }

                GUILayout.FlexibleSpace();
                using (new IMGUIBeginVertical()) {
                    GUILayout.Space(4);
                    if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                        if (m_variants.Any() && m_selectedVariant != null) {
                            VariantsService.DeleteVariant(Asset.Template.Id, m_selectedVariant, SelectNextVariant);
                        }
                        else {
                            m_selectedVariant = null;
                            m_selectedSkin = null;
                        }
                    }

                    if (GUILayout.Button("Save", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                        VariantsService.SaveVariantMeta(Asset.Template.Id, m_selectedVariant);
                    }

                    GUILayout.Space(4);
                }
            }
        }

        private void ShowSkinsPanel() {
            if (m_selectedVariant != m_selectedVariantPrevious) {
                VariantsService.GetSkinsList(Asset.Template.Id, m_selectedVariant.Id, list => {
                    m_selectedVariant.SetSkins(list);
                });
                m_selectedVariantPrevious = m_selectedVariant;
            }

            using (new IMGUIBeginHorizontal()) {
                GUILayout.Label("Skins:", WizardWindow.Constants.settingsBoxTitle);
                GUI.enabled = m_selectedVariant != null;
                var edit = Resources.Load(EDIT_ICON_NAME) as Texture2D;
                if (GUILayout.Button(edit, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    if (m_selectedVariant != null) {
                        ShowEditNameWindow(m_selectedSkin.Name, (isSuccessful, skinName) => {
                            if (isSuccessful) {
                                m_selectedSkin.Name = skinName;
                            }
                            else {
                                ShowNotification(new GUIContent("Editing skin name canceled by user!"));
                            }
                        });
                    }
                }

                var refreshBlack = Resources.Load(REFRESH_ICON_NAME) as Texture2D;
                if (GUILayout.Button(refreshBlack, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    VariantsService.GetSkinsList(Asset.Template.Id, m_selectedVariant.Id, list => {
                        m_selectedVariant.SetSkins(list);
                    });
                }

                if (GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20))) {
                    if (m_selectedVariant != null) {
                        ShowCreateNewSkinVariant((isSuccessful, variantName) => {
                            if (isSuccessful)
                                VariantsService.CreateSkin(variantName, m_selectedVariant.Id, Asset.Template.Id, skin => {
                                    m_selectedVariant.AddSkin(skin);
                                    SelectSkin(skin);
                                });
                        });
                    }
                }

                GUI.enabled = true;
            }
        }

        private void ShowSkinsScrollView() {
            using (new IMGUIBeginHorizontal()) {
                GUILayout.Space(1.0f);
                m_skinsScrollPos = GUILayout.BeginScrollView(m_skinsScrollPos);
                if (m_selectedVariant != null) {
                    foreach (var skin in m_selectedVariant.Skins) {
                        using (new IMGUIBeginHorizontal()) {
                            if (GUILayout.Toggle(m_selectedSkin == skin, skin.Name, WizardWindow.Constants.keysElement)) {
                                SelectSkin(skin);
                            }
                        }
                    }
                }

                GUILayout.EndScrollView();
            }
        }

        private void ShowGameObjectsHierarchy(Rect localRect, Rect rect) {
            using (new IMGUIBeginHorizontal()) {
                if (m_selectedVariant != null) {
                    GUILayout.Label("Game objects hierarchy:", WizardWindow.Constants.settingsBoxTitle);
                }
            }

            if (Event.current.type == EventType.Repaint) {
                localRect = GUILayoutUtility.GetLastRect();
                var bot = localRect.y + localRect.height + 1.0f;
                localRect = new Rect(1.0f, bot, rect.width - 1.0f, rect.height - bot - 24.0f);
            }

            m_gameObjectsHierarchyView?.OnGUI(localRect);
        }

        private void ShowPropSkinsInfo(Rect localRect, Rect rect) {
            if (m_selectedSkin != null && m_selectedVariant != null) {
                GUILayout.Space(4);
                using (new IMGUIBeginHorizontal()) {
                    GUILayout.Label("Prop skin info", PropVariantsEditorWindowStyles.HeaderLabel, GUILayout.Width(200.0f));
                    GUILayout.FlexibleSpace();
                    using (new IMGUIBeginVertical(GUILayout.Width(PREVIEW_WIDTH))) {
                        GUILayout.Space(1);

                        if (!m_searchFilterActive) {
                            if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                RemoveSkin();
                            }

                            GUILayout.Space(1);

                            if (GUILayout.Button("Edit", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                AddHierarchyFilterType();
                            }

                            if (GUILayout.Button("Download", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                BundleService.DownloadSkin(Asset, m_selectedVariant, m_selectedSkin);
                            }

                            if (GUILayout.Button("Upload", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                BundleService.UploadSkin(m_selectedVariant, m_selectedSkin, Asset);
                            }

                            if (GUILayout.Button("Save", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                VariantsService.SaveSkinMeta(Asset.Template.Id, m_selectedSkin);
                            }
                        }
                        else {
                            if (GUILayout.Button("Done", EditorStyles.miniButton, GUILayout.Width(PREVIEW_WIDTH))) {
                                ClearHierarchyFilterType();
                            }
                        }
                    }

                    GUILayout.Space(4);
                }

                using (new IMGUIBeginHorizontal()) {
                    using (new IMGUIBeginVertical()) {
                        GUILayout.Label(string.Concat("Id: ", m_selectedSkin.Id), new GUIStyle(EditorStyles.boldLabel), GUILayout.Width(250.0f));
                        GUILayout.Label(string.Concat("Platforms: ", string.Join(",", m_selectedSkin.AvailablePlatforms)), new GUIStyle(EditorStyles.boldLabel), GUILayout.Width(250.0f));

                        m_selectedSkin.Name = EditorGUILayout.TextField(m_selectedSkin.Name, GUILayout.Width(250.0f));
                        m_selectedSkin.IsDefault = EditorGUILayout.Toggle("Is Default", m_selectedSkin.IsDefault);

                        using (new IMGUIBeginHorizontal()) {
                            m_selectedSkin.Thumbnail = (Texture2D) EditorGUILayout.ObjectField(m_selectedSkin.Thumbnail,
                                typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
                            m_mouseOverSkinTexture = m_selectedSkin.Thumbnail;
                            if (Event.current.type == EventType.Repaint) {
                                var lastRect = GUILayoutUtility.GetLastRect();
                                m_isMouseOverSkinIcon = lastRect.Contains(Event.current.mousePosition);
                            }

                            EditorGUILayout.Space();
                            using (new IMGUIBeginVertical()) {
                                EditorGUILayout.Space();
                                s_useEditorCameraPositionSkin = EditorGUILayout.Toggle("Use Editor Camera", s_useEditorCameraPositionSkin);
                                s_disableSkinUnrelatedRenderersForScreenshot = EditorGUILayout.Toggle("Disable Unrelated Objects", s_disableSkinUnrelatedRenderersForScreenshot);
                                if (GUILayout.Button("Make Icon")) {
                                    CreateSkinPreviewIcon();
                                }

                                GUILayout.Space(5);
                            }
                        }

                        EditorGUILayout.Space();
                    }

                    GUILayout.FlexibleSpace();
                }

                ShowSkinsMaterialsHierarchy(localRect, rect);
            }
        }

        private void RemoveSkin() {
            VariantsService.DeleteSkin(Asset.Template.Id, m_selectedSkin, (s) => {
                SelectNextSkin(m_selectedVariant, s);
                m_selectedVariant.RemoveSkin(s);
            });
        }

        private static bool m_searchFilterActive;

        private void AddHierarchyFilterType() {
            m_searchFilterActive = true;
            var listGO = m_selectedVariant.GameObjects.Select(item => item.gameObject).ToList();
            listGO.ForEach(item => {
                var editSkinObject = item.GetComponent<EditSkinObject>();
                if (editSkinObject == null) {
                    item.AddComponent<EditSkinObject>();
                }
            });
            SetSearchFilter("t: " + nameof(EditSkinObject), SearchType.FILTERMODE_ALL);
        }

        private void ClearHierarchyFilterType() {
            m_searchFilterActive = false;
            var listGO = m_selectedVariant.GameObjects.Select(item => item.gameObject).ToList();
            listGO.ForEach(item => {
                DestroyImmediate(item.GetComponent<EditSkinObject>());
            });
            SetSearchFilter(string.Empty, SearchType.FILTERMODE_ALL);
        }

        private static void SetSearchFilter(string filter, SearchType filterMode) {
            SearchableEditorWindow hierarchy = null;
            SearchableEditorWindow[] windows = (SearchableEditorWindow[]) Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
            foreach (SearchableEditorWindow window in windows) {
                if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow") {
                    hierarchy = window;
                    break;
                }
            }

            if (hierarchy == null) {
                return;
            }

            var setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { filter, (int) filterMode, true, false };
            setSearchType.Invoke(hierarchy, parameters);
        }

        private void ShowSkinsMaterialsHierarchy(Rect localRect, Rect rect) {
            GUILayout.Label("Hierarchy:", WizardWindow.Constants.settingsBoxTitle);
            if (Event.current.type == EventType.Repaint) {
                localRect = GUILayoutUtility.GetLastRect();
                var bot = localRect.y + localRect.height + 1.0f;
                localRect = new Rect(1.0f, bot,
                    rect.width - 1.0f, rect.height - bot - 24.0f);
            }

            m_hierarchyView?.OnGUI(localRect);
        }

        private void SelectVariant(PropVariant variant) {
            if (variant == m_selectedVariant) {
                return;
            }

            var listGO = variant.GameObjects.Select(item => item.gameObject).ToList();
            try {
                m_gameObjectsHierarchyView = new GameObjectsHierarchyView(new TreeViewState(), listGO);
            }
            catch (Exception e) {
                Debug.LogError(e);
            }

            m_selectedVariant = variant;
            SelectDefaultSkin(m_selectedVariant);
        }

        private void SelectNextVariant() {
            var fistVariant = m_variants.FirstOrDefault();
            var nextVariantExist = false;
            PropVariant previousVariant = null;
            PropVariant variant = null;
            foreach (var next in m_variants) {
                if (nextVariantExist) {
                    variant = next;
                    break;
                }

                if (next == m_selectedVariant) {
                    nextVariantExist = true;
                }
                else {
                    previousVariant = next;
                }
            }

            switch (variant) {
                case null when previousVariant != null:
                    variant = previousVariant;
                    break;
                case null:
                    variant = fistVariant;
                    break;
            }

            m_variants.Remove(m_selectedVariant);
            if (variant != null) {
                SelectVariant(variant);
            }
        }

        private void SelectDefaultSkin(PropVariant variant) {
            var skin = variant.Skins.FirstOrDefault();
            if (skin != null) {
                SelectSkin(skin);
            }
        }

        private void SelectNextSkin(PropVariant variant, PropSkin selectedSkin) {
            var fistskin = variant.Skins.FirstOrDefault();
            var nextSkinExist = false;
            PropSkin previousSkin = null;
            PropSkin skin = null;
            foreach (var next in variant.Skins) {
                if (nextSkinExist) {
                    skin = next;
                    break;
                }

                if (next == selectedSkin) {
                    nextSkinExist = true;
                }
                else {
                    previousSkin = next;
                }
            }

            switch (skin) {
                case null when previousSkin != null:
                    skin = previousSkin;
                    break;
                case null:
                    skin = fistskin;
                    break;
            }

            if (skin != null) {
                SelectSkin(skin);
            }
        }

        private void SelectSkin(PropSkin skin) {
            if (skin == m_selectedSkin) {
                return;
            }

            m_selectedSkin = skin;
            m_hierarchyView = new VariantHierarchyView(new TreeViewState(), m_selectedVariant, m_selectedSkin);
        }

        private void CreateVariantPreviewIcon() {
            PropAssetScreenshotTool.CreateIcon(s_useVariantEditorCameraPosition, Asset, out var selectedVariantIcon);
            m_selectedVariant.Thumbnail = selectedVariantIcon;
        }

        private void CreateSkinPreviewIcon() {
            if (s_disableSkinUnrelatedRenderersForScreenshot) {
                CustomizableSkinGameObject.DisableSkinUnrelatedRenderers(m_asset, m_selectedVariant);
            }

            PropAssetScreenshotTool.CreateIcon(s_useEditorCameraPositionSkin, Asset, out var selectedSkinIcon);
            m_selectedSkin.Thumbnail = selectedSkinIcon;
            foreach (var rend in Asset.Renderers) {
                rend.enabled = true;
            }
        }
#endif
        private void ShowCreateNewVariant(string windowTitle, string nameNewUnit, string headerName, Action<bool, string> callback) {
            var window = GetWindow<DialogCreateVariant>(true, windowTitle);
            window.NameNewUnit = nameNewUnit;
            window.HeaderName = headerName;
            window.OnCreateClickEvent += callback;
            window.minSize = new Vector2(200, 100);
            var width = position.x + (position.width / 3f);
            var height = position.y + (m_minSize.y / 2f) - (window.minSize.y / 2);
            window.position = new Rect(new Vector2(width, height), window.minSize);
            window.autoRepaintOnSceneChange = true;
            window.Focus();
            window.ShowAuxWindow();
        }

        private void ShowCreateNewPropVariant(Action<bool, string> callback) {
            ShowCreateNewVariant("Create Prop variant", Selection.activeObject.name, "Enter variant name", callback);
        }

        private void ShowCreateNewSkinVariant(Action<bool, string> callback) {
            ShowCreateNewVariant("Create skin", "new_skin", "Enter skin name", callback);
        }

        public void ShowEditNameWindow(string itemName, Action<bool, string> callback) {
            var window = GetWindow<EditPropVariant>(true, "Edit prop variant name");
            window.OnEditClickEvent += callback;
            window.Init(itemName);
            window.minSize = new Vector2(300, 100);
            var width = position.x + m_minSize.x / 2f - window.minSize.x / 2;
            var height = position.y + m_minSize.y / 2f - window.minSize.y / 2;
            window.position = new Rect(new Vector2(width, height), window.minSize);
            window.Focus();
            window.ShowAuxWindow();
        }

        PropAsset Asset {
            get {
                if (m_asset == null) {
                    m_asset = FindObjectWithType<PropAsset>();
                }

                return m_asset;
            }
        }

        private T FindObjectWithType<T>() {
            var allFoundObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var obj in allFoundObjects) {
                var gameObject = (GameObject) obj;
                var target = gameObject.GetComponent<T>();

                if (target != null) {
                    return target;
                }
            }

            return default;
        }

        public static PropVariantsEditorWindow Editor => GetWindow<PropVariantsEditorWindow>("Variants");

        public void AddItemsToMenu(GenericMenu menu) {
            menu.AddItem(new GUIContent("Prepare Skin for upload test"), false, () => {
                BundleService.PrepareSkinForUploadTest(m_windowUI.SelectedVariant, m_windowUI.SelectedSkin, Asset);
            });
        }
    }
}

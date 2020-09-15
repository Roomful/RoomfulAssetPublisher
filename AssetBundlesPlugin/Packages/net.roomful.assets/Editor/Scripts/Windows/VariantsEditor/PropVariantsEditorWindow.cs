using System.Linq;
using System;
using StansAssets.Foundation;
using StansAssets.Plugins.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace net.roomful.assets.Editor
{
    public sealed class PropVariantsEditorWindow : EditorWindow
    {
        private Vector2 m_variantsScrollPos = Vector2.zero;
        private Vector2 m_skinsScrollPos = Vector2.zero;

        private PropVariant m_selectedVariant;
        private Skin m_selectedSkin;

        private PropAsset m_asset;
        private VariantHierarchyView m_hierarchyView;
        private readonly Vector2 m_minSize = new Vector2(600.0f, 100.0f);
       

        void SelectVariant(PropVariant variant)
        {
            if (variant != m_selectedVariant)
            {
                m_selectedVariant.ApplySkin(m_selectedVariant.DefaultSkin);
                m_selectedVariant = variant;
                SelectSkin(m_selectedVariant.DefaultSkin);
            }
        }

        void SelectSkin(Skin skin)
        {
            if (skin != m_selectedSkin)
            {
                m_selectedSkin = skin;
                m_hierarchyView = new VariantHierarchyView(new TreeViewState(), m_selectedVariant, m_selectedSkin);
                
                m_selectedVariant.ApplySkin(m_selectedSkin);
            }
        }

        void OnEnable()
        {
            minSize = m_minSize;
        }

        void OnGUI()
        {
            if (Asset == null)
            {
                GUILayout.Label("[No Prop Assets in Edit mode]", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            using (new IMGUIBeginHorizontal())
            {
                var rect = new Rect(0.0f, 0.0f, 300.0f, Screen.height);
                GUILayout.BeginArea(rect);
                GUI.Box(rect, GUIContent.none, WizardWindow.Constants.settingsBox);

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Label("Variants:", WizardWindow.Constants.settingsBoxTitle);

                    if (GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20)))
                    {
                        var selection = Selection.objects.Where(o => o != null && o is GameObject && !EditorUtility.IsPersistent(o))
                            .Select(o => (GameObject)o).ToList();

                        if (selection.Count == 0)
                        {
                            ShowNotification(new GUIContent("Variant Creation Failed! Nothing selected"));
                        }
                        else
                        {
                            if (Asset.Template.ValidateVariantCreate(selection))
                            {
                                ShowCreateNewPropVariant((isSuccessful, variantName) =>
                                {
                                    if (isSuccessful)
                                    {
                                        Asset.Template.TryCreateVariant(selection, out var variant, variantName);
                                        m_selectedVariant?.ApplySkin(m_selectedVariant.DefaultSkin);

                                        variant.AddSkin(new Skin("default", variant.MaterialDictionary));
                                        Asset.Template.AddVariant(variant);
                                        m_selectedVariant = variant;
                                        SelectSkin(variant.DefaultSkin);
                                    }
                                    else
                                    {
                                        //TODO explain why with popup
                                        Debug.Log("Variant Creation Canceled");
                                    }
                                });
                            }
                            else
                            {
                                ShowNotification(new GUIContent("Variant Creation Failed! Check Console!"));
                            }
                        }
                    }
                }

                using (new IMGUIBeginHorizontal())
                {
                    GUILayout.Space(1.0f);
                    m_variantsScrollPos = GUILayout.BeginScrollView(m_variantsScrollPos, GUILayout.Height(Screen.height - 39.0f));
                    foreach (var variant in Asset.Template.Variants)
                    {
                        using (new IMGUIBeginHorizontal())
                        {
                            var variantLabel = new GUIContent(variant.Name);
                            var r = GUILayoutUtility.GetRect(variantLabel, PropVariantsEditorWindowStyles.VariantTitle, GUILayout.ExpandWidth(true));

                            if (m_selectedVariant == variant && Event.current.type == EventType.Repaint)
                            {
                                var color = EditorGUIUtility.isProSkin ? new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f)
                                    : new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);

                                GUI.DrawTexture(r, Texture2DUtility.MakePlainColorImage(color));
                            }

                            EditorGUI.BeginChangeCheck();
                            if (GUI.Toggle(r, m_selectedVariant == variant, variantLabel, PropVariantsEditorWindowStyles.VariantTitle))
                            {
                                SelectVariant(variant);

                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                GUIUtility.keyboardControl = 0;
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndArea();

                rect = new Rect(rect.x + rect.width, rect.y, 300.0f, Screen.height);
                GUILayout.BeginArea(rect);

                var localRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
                GUI.Box(localRect, GUIContent.none, WizardWindow.Constants.settingsBox);

                if (m_selectedVariant != null)
                {

                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Label(m_selectedVariant.Name, PropVariantsEditorWindowStyles.HeaderLabel, GUILayout.Width(200.0f));
                        GUILayout.FlexibleSpace();

                        using (new IMGUIBeginVertical())
                        {
                            GUILayout.Space(3.0f);
                            using (new IMGUIBeginHorizontal(GUILayout.Width(80.0f)))
                            {
                                if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    m_selectedVariant.ApplySkin(m_selectedVariant.DefaultSkin);
                                    Asset.Template.RemoveVariant(m_selectedVariant);
                                    if (Asset.Template.Variants.Any())
                                    {
                                        SelectVariant(Asset.Template.Variants.First());
                                    }
                                    else
                                    {
                                        m_selectedVariant = null;
                                        m_selectedSkin = null;
                                    }
                                }
                            }
                        }
                    }

                    GUILayout.Label("Prop Variant Info");

                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Label("Skins:", WizardWindow.Constants.settingsBoxTitle);

                        GUI.enabled = m_selectedVariant != null;
                        if (GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20)))
                        {
                            if (m_selectedVariant != null)
                            {
                                var newSkin = new Skin("new skin", m_selectedVariant.MaterialDictionary);
                                m_selectedVariant.AddSkin(newSkin);
                                SelectSkin(newSkin);
                            }
                        }

                        GUI.enabled = true;
                    }

                    GUILayout.Space(1.0f);
                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Space(1.0f);

                        m_skinsScrollPos = GUILayout.BeginScrollView(m_skinsScrollPos);
                        if (m_selectedVariant != null)
                            foreach (var skin in m_selectedVariant.Skins)
                            {
                                using (new IMGUIBeginHorizontal())
                                {
                                    if (GUILayout.Toggle(m_selectedSkin == skin, skin.Name, WizardWindow.Constants.keysElement))
                                    {
                                        SelectSkin(skin);
                                    }
                                }
                            }

                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndArea();

                rect = new Rect(rect.x + rect.width, rect.y, Screen.width - 2.0f * 300.0f, Screen.height);
                GUILayout.BeginArea(rect);

                localRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
                GUI.Box(localRect, GUIContent.none, WizardWindow.Constants.settingsBox);

                if (m_selectedSkin != null && m_selectedVariant != null)
                {
                    using (new IMGUIBeginHorizontal())
                    {
                        GUILayout.Label(m_selectedSkin.Name, PropVariantsEditorWindowStyles.HeaderLabel, GUILayout.Width(200.0f));
                        GUILayout.FlexibleSpace();
                        
                        using (new IMGUIBeginVertical())
                        {
                            GUILayout.Space(3.0f);
                            using (new IMGUIBeginHorizontal(GUILayout.Width(100.0f)))
                            {
                                if (GUILayout.Button("Apply", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    m_selectedVariant.ApplySkin(m_selectedSkin);
                                }

                                if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    if (m_selectedSkin != m_selectedVariant.DefaultSkin)
                                    {
                                        m_selectedVariant.RemoveSkin(m_selectedSkin);
                                        SelectSkin(m_selectedVariant.DefaultSkin);
                                        m_selectedVariant.ApplySkin(m_selectedVariant.DefaultSkin);
                                    }
                                }

                                GUILayout.Space(4.0f);
                            }
                        }
                    }

                    using (new IMGUIBeginHorizontal())
                    {
                        const float previewWidth = 100.0f;
                        var width = localRect.width - previewWidth - 15.0f;
                        using (new IMGUIBeginVertical(GUILayout.Width(width)))
                        {
                            GUILayout.Label("Prop Skin Info");
                            if (GUILayout.Button("Reload Preview Icon", EditorStyles.miniButton, GUILayout.Width(180.0f)))
                            {
                                CreateSkinPreviewIcon();
                            }
                        }

                        GUILayout.FlexibleSpace();
                        

                        using (new IMGUIBeginVertical(GUILayout.Width(previewWidth)))
                        {
                            EditorGUILayout.LabelField("Preview Icon: ", EditorStyles.boldLabel);
                            m_selectedSkin.PreviewIcon = (Texture2D)EditorGUILayout.ObjectField(m_selectedSkin.PreviewIcon,
                                typeof(Texture2D), false, GUILayout.Width(previewWidth), GUILayout.Height(previewWidth));
                        }
                    }

                    GUILayout.Label("Hierarchy:", WizardWindow.Constants.settingsBoxTitle);

                    if (Event.current.type == EventType.Repaint)
                    {
                        localRect = GUILayoutUtility.GetLastRect();
                        var bot = localRect.y + localRect.height + 1.0f;
                        localRect = new Rect(1.0f, bot,
                            rect.width - 1.0f, rect.height - bot - 24.0f);
                    }

                    if (m_hierarchyView != null)
                        m_hierarchyView.OnGUI(localRect);
                }

                GUILayout.EndArea();
            }
        }

        private void CreateSkinPreviewIcon()
        {
            foreach (var rend in m_asset.Renderers)
            {
                rend.enabled = false;
            }
            foreach (var rend in m_selectedVariant.Renderers)
            {
                if (m_asset.Renderers.Contains(rend))
                    rend.enabled = true;
            }
            PropAssetScreenshotTool.CreateIcon(false, m_asset, m_selectedSkin);
            foreach (var rend in m_asset.Renderers)
            {
                rend.enabled = true;
            }
        }

        public void ShowCreateNewPropVariant(Action<bool, string> callback)
        {
            var window = GetWindow<CreatePropVariant>(true, "Create Prop variant");
            window.OnCreateClickEvent += callback;
            window.minSize = new Vector2(300f, 100f);
            window.position = new Rect(new Vector2(Screen.width - (window.minSize.x / 2), Screen.height - (window.minSize.y / 2)), window.minSize);
            window.Focus();

            window.ShowAuxWindow();
        }

        PropAsset Asset
        {
            get
            {
                if (m_asset == null)
                {
                    m_asset = FindObjectWithType<PropAsset>();
                }
                return m_asset;
            }
        }

        T FindObjectWithType<T>()
        {
            var allFoundObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var obj in allFoundObjects)
            {
                var gameObject = (GameObject)obj;
                var target = gameObject.GetComponent<T>();

                if (target != null)
                {
                    return target;
                }
            }
            return default(T);
        }

        public static PropVariantsEditorWindow Editor => GetWindow<PropVariantsEditorWindow>("Variants");
    }
}

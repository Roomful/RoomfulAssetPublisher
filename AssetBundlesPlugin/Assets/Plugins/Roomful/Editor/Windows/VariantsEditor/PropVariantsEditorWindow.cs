using System.Linq;
using System;
using SA.Foundation.Editor;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{
    public sealed class PropVariantsEditorWindow : EditorWindow
    {
        Vector2 m_VariantsScrollPos = Vector2.zero;
        Vector2 m_SkinsScrollPos = Vector2.zero;

        PropVariant m_SelectedVariant;
        Skin m_SelectedSkin;

        PropAsset m_Asset;

        VariantHierarchyView m_HierarchyView;

        readonly Vector2 m_MinSize = new Vector2(600.0f, 100.0f);

        GUIStyle m_VariantTitle;

        GUIStyle VariantTitle
        {
            get
            {
                if (m_VariantTitle == null)
                {
                    m_VariantTitle = "PreferencesSection";
                    m_VariantTitle.alignment = TextAnchor.MiddleLeft;
                }

                return m_VariantTitle;
            }
        }

        GUIStyle m_HeaderLabel;

        GUIStyle HeaderLabel
        {
            get
            {
                if (m_HeaderLabel == null)
                {
                    m_HeaderLabel = new GUIStyle(EditorStyles.largeLabel);
                    m_HeaderLabel.fontStyle = FontStyle.Bold;
                    m_HeaderLabel.fontSize = 18;
                    m_HeaderLabel.margin.top = -1;
                    m_HeaderLabel.margin.left++;

                    m_HeaderLabel.normal.textColor = !EditorGUIUtility.isProSkin ?
                        new Color(0.4f, 0.4f, 0.4f, 1f) : new Color(0.7f, 0.7f, 0.7f, 1f);
                }
                return m_HeaderLabel;
            }
        }

        void SelectVariant(PropVariant variant)
        {
            if (variant != m_SelectedVariant)
            {
                m_SelectedVariant.ApplySkin(m_SelectedVariant.DefaultSkin);
                m_SelectedVariant = variant;
                SelectSkin(m_SelectedVariant.DefaultSkin);
            }
        }

        void SelectSkin(Skin skin)
        {
            if (skin != m_SelectedSkin)
            {
                m_SelectedSkin = skin;

                m_HierarchyView = new VariantHierarchyView(new TreeViewState(), m_SelectedVariant, m_SelectedSkin);
            }
        }

        void OnEnable()
        {
            minSize = m_MinSize;
        }

        void OnGUI()
        {
            if (Asset == null)
            {
                GUILayout.Label("[No Prop Assets in Edit mode]", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            using (new SA_GuiBeginHorizontal())
            {
                Rect rect = new Rect(0.0f, 0.0f, 300.0f, Screen.height);
                GUILayout.BeginArea(rect);
                GUI.Box(rect, GUIContent.none, WizardWindow.Constants.settingsBox);

                using (new SA_GuiBeginHorizontal())
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
                                ShowCreateNewPropVariant((name) =>
                                {
                                    PropVariant variant;
                                    Asset.Template.TryCreateVariant(selection, out variant, name);


                                    if (m_SelectedVariant != null)
                                    {
                                        m_SelectedVariant.ApplySkin(m_SelectedVariant.DefaultSkin);
                                    }

                                    variant.AddSkin(new Skin("default", variant.Materials));
                                    Asset.Template.AddVariant(variant);
                                    m_SelectedVariant = variant;
                                    SelectSkin(variant.DefaultSkin);

                                });
                            }
                            else
                            {
                                ShowNotification(new GUIContent("Variant Creation Failed! Check Console!"));
                            }
                        }
                    }
                }

                using (new SA_GuiBeginHorizontal())
                {
                    GUILayout.Space(1.0f);
                    m_VariantsScrollPos = GUILayout.BeginScrollView(m_VariantsScrollPos, GUILayout.Height(Screen.height - 39.0f));
                    foreach (var variant in Asset.Template.Variants)
                    {
                        using (new SA_GuiBeginHorizontal())
                        {
                            GUIContent variantLabel = new GUIContent(variant.Name);
                            Rect r = GUILayoutUtility.GetRect(variantLabel, VariantTitle, GUILayout.ExpandWidth(true));

                            if (m_SelectedVariant == variant && Event.current.type == EventType.Repaint)
                            {
                                var color = EditorGUIUtility.isProSkin ? new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f)
                                    : new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);

                                GUI.DrawTexture(r, IconManager.GetIcon(color));
                            }

                            EditorGUI.BeginChangeCheck();
                            if (GUI.Toggle(r, m_SelectedVariant == variant, variantLabel, VariantTitle))
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

                Rect localRect = new Rect(0.0f, 0.0f, rect.width, rect.height);
                GUI.Box(localRect, GUIContent.none, WizardWindow.Constants.settingsBox);

                if (m_SelectedVariant != null)
                {

                    using (new SA_GuiBeginHorizontal())
                    {
                        GUILayout.Label(m_SelectedVariant.Name, HeaderLabel, GUILayout.Width(200.0f));
                        GUILayout.FlexibleSpace();

                        using (new SA_GuiBeginVertical())
                        {
                            GUILayout.Space(3.0f);
                            using (new SA_GuiBeginHorizontal(GUILayout.Width(80.0f)))
                            {
                                if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    m_SelectedVariant.ApplySkin(m_SelectedVariant.DefaultSkin);
                                    Asset.Template.RemoveVariant(m_SelectedVariant);
                                    if (Asset.Template.Variants.Any())
                                    {
                                        SelectVariant(Asset.Template.Variants.First());
                                    }
                                    else
                                    {
                                        m_SelectedVariant = null;
                                        m_SelectedSkin = null;
                                    }
                                }
                            }
                        }
                    }

                    GUILayout.Label("Prop Variant Info");

                    using (new SA_GuiBeginHorizontal())
                    {
                        GUILayout.Label("Skins:", WizardWindow.Constants.settingsBoxTitle);

                        GUI.enabled = m_SelectedVariant != null;
                        if (GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20)))
                        {
                            if (m_SelectedVariant != null)
                            {
                                var newSkin = new Skin("new skin", m_SelectedVariant.Materials);
                                m_SelectedVariant.AddSkin(newSkin);
                                SelectSkin(newSkin);
                            }
                        }

                        GUI.enabled = true;
                    }

                    GUILayout.Space(1.0f);
                    using (new SA_GuiBeginHorizontal())
                    {
                        GUILayout.Space(1.0f);

                        m_SkinsScrollPos = GUILayout.BeginScrollView(m_SkinsScrollPos);
                        if (m_SelectedVariant != null)
                            foreach (var skin in m_SelectedVariant.Skins)
                            {
                                using (new SA_GuiBeginHorizontal())
                                {
                                    if (GUILayout.Toggle(m_SelectedSkin == skin, skin.Name, WizardWindow.Constants.keysElement))
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

                if (m_SelectedSkin != null && m_SelectedVariant != null)
                {
                    using (new SA_GuiBeginHorizontal())
                    {
                        GUILayout.Label(m_SelectedSkin.Name, HeaderLabel, GUILayout.Width(200.0f));
                        GUILayout.FlexibleSpace();

                        using (new SA_GuiBeginVertical())
                        {
                            GUILayout.Space(3.0f);
                            using (new SA_GuiBeginHorizontal(GUILayout.Width(100.0f)))
                            {
                                if (GUILayout.Button("Apply", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    m_SelectedVariant.ApplySkin(m_SelectedSkin);
                                }

                                if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(80.0f)))
                                {
                                    if (m_SelectedSkin != m_SelectedVariant.DefaultSkin)
                                    {
                                        m_SelectedVariant.RemoveSkin(m_SelectedSkin);
                                        SelectSkin(m_SelectedVariant.DefaultSkin);
                                        m_SelectedVariant.ApplySkin(m_SelectedVariant.DefaultSkin);
                                    }
                                }

                                GUILayout.Space(4.0f);
                            }
                        }
                    }

                    using (new SA_GuiBeginHorizontal())
                    {
                        const float previewWidth = 100.0f;
                        float width = localRect.width - previewWidth - 15.0f;
                        using (new SA_GuiBeginVertical(GUILayout.Width(width)))
                        {
                            GUILayout.Label("Prop Skin Info");
                        }

                        GUILayout.FlexibleSpace();
                        using (new SA_GuiBeginVertical(GUILayout.Width(previewWidth)))
                        {
                            EditorGUILayout.LabelField("Preview Icon: ", EditorStyles.boldLabel);
                            m_SelectedSkin.PreviewIcon = (Texture2D)EditorGUILayout.ObjectField(m_SelectedSkin.PreviewIcon,
                                typeof(Texture2D), false, GUILayout.Width(previewWidth), GUILayout.Height(previewWidth));
                        }
                    }

                    GUILayout.Label("Hierarchy:", WizardWindow.Constants.settingsBoxTitle);

                    if (Event.current.type == EventType.Repaint)
                    {
                        localRect = GUILayoutUtility.GetLastRect();
                        float bot = localRect.y + localRect.height + 1.0f;
                        localRect = new Rect(1.0f, bot,
                            rect.width - 1.0f, rect.height - bot - 24.0f);
                    }

                    if (m_HierarchyView != null)
                        m_HierarchyView.OnGUI(localRect);
                }

                GUILayout.EndArea();
            }
        }

        public void ShowCreateNewPropVariant(Action<string> callback)
        {
            CreatePropVariant window = EditorWindow.GetWindow<CreatePropVariant>(true, "Create Prop variant");
            window.OnCreateClickEvent += callback;
            window.minSize = new Vector2(300f, 100f);
            window.maxSize = new Vector2(window.minSize.x, window.maxSize.y);
            window.position = new Rect(new Vector2(Screen.width - (window.minSize.x / 2), Screen.height - (window.minSize.y / 2)), window.minSize);
            window.Focus();

            window.ShowAuxWindow();
        }

        PropAsset Asset
        {
            get
            {
                if (m_Asset == null)
                {
                    m_Asset = FindObjectWithType<PropAsset>();
                }
                return m_Asset;
            }
        }

        T FindObjectWithType<T>()
        {
            var allFoundObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (var obj in allFoundObjects)
            {
                var gameObject = (GameObject)obj;
                T target = gameObject.GetComponent<T>();

                if (target != null)
                {
                    return target;
                }
            }
            return default(T);
        }

        public static PropVariantsEditorWindow Editor
        {
            get
            {
                return GetWindow<PropVariantsEditorWindow>("Variants");
            }
        }
    }
}

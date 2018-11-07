using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;


namespace SA.Foundation.Editor
{
    public abstract class SA_ServiceLayout : SA_GUILayoutElement
    {

        [SerializeField] bool m_isSelected = false;
        [SerializeField] SA_HyperLabel m_blockTitleLabel;
        [SerializeField] SA_HyperLabel m_blockAPIStateLabel;
        [SerializeField] SA_HyperLabel m_apiEnableButton;

        [SerializeField] SA_HyperLabel m_showMoreButton;
        [SerializeField] protected List<SA_FeatureUrl> m_features;

        private AnimBool m_ShowExtraFields;

        [SerializeField] Texture2D m_expandOpenIcon;
        [SerializeField] Texture2D m_expandClosedIcon;


        [SerializeField] Texture2D m_onToggle;
        [SerializeField] Texture2D m_offToggle;



        //--------------------------------------
        // Abstract
        //--------------------------------------



        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract Texture2D Icon { get; }
        public abstract SA_iAPIResolver Resolver { get; }
        protected abstract void OnServiceUI();
        protected abstract void DrawServiceRequirements();


        //--------------------------------------
        // Virtual
        //--------------------------------------

        public virtual bool CanBeDisabled {
            get {
                return true;
            }
        }



        public virtual List<string> SupportedPlatfroms {
            get {
                return new List<string>() { "Android", "Android TV", "Android Wear" };
            }
        }


        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void AddFeatureUrl(string title, string url) {
            var feature = new SA_FeatureUrl(title, url);
            m_features.Add(feature);
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public bool IsSelected {
            get {
                return m_isSelected;
            }
        }

        //--------------------------------------
        // SA_GUILayoutElement implementation
        //--------------------------------------

        public override void OnAwake() {
            m_blockTitleLabel = new SA_HyperLabel(new GUIContent(Title), SA_PluginSettingsWindowStyles.LabelServiceBlockStyle);
            m_blockTitleLabel.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

            m_blockAPIStateLabel = new SA_HyperLabel(new GUIContent("OFF"), OffStyle);
            m_blockAPIStateLabel.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);


            m_expandOpenIcon = SA_Skin.GetGenericIcon("expand.png");
            m_expandClosedIcon = SA_Skin.GetGenericIcon("expand_close.png");
            m_showMoreButton = new SA_HyperLabel(new GUIContent(m_expandOpenIcon));
            m_showMoreButton.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);



            m_onToggle = SA_Skin.GetGenericIcon("on_toggle.png");
            m_offToggle = SA_Skin.GetGenericIcon("off_toggle.png");
            m_apiEnableButton = new SA_HyperLabel(new GUIContent(m_onToggle));
            m_apiEnableButton.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);
            m_apiEnableButton.GuiColorOverrdie(true);

            m_ShowExtraFields = new AnimBool(false);

            m_features = new List<SA_FeatureUrl>();
        }


        Rect m_labelRect;
        const float DESCRIBTION_LABEL_ONE_LIEN_HEIGHT = 16f;
        public override void OnGUI() {
            CheckServiceAvalibility();
            DrawBlockUI();
        }

        public void UnSelect() {
            m_isSelected = false;
        }


        private void DrawBlockUI() {
            GUILayout.Space(5);

            bool titleClick = false;
            bool toggleClick = false;

            using (new SA_GuiBeginHorizontal()) {
                GUILayout.Space(10);
                GUILayout.Label(Icon, SA_PluginSettingsWindowStyles.LabelServiceBlockStyle, GUILayout.Width(IconSize), GUILayout.Height(IconSize));
                GUILayout.Space(5);

                using (new SA_GuiBeginVertical()) {
                    GUILayout.Space(TitleVerticalSpace);
                    titleClick = m_blockTitleLabel.Draw(GUILayout.Height(25));
                }


                GUILayout.FlexibleSpace();
                toggleClick = DrawServiceStateInfo();
                
            }

            if (titleClick || toggleClick) {
                m_isSelected = true;
            }


            GUILayout.Space(5);
            using (new SA_GuiBeginHorizontal()) {
                GUILayout.Space(15);
                EditorGUILayout.LabelField(Description, SA_PluginSettingsWindowStyles.DescribtionLabelStyle);

                if (Event.current.type == EventType.Repaint) {

                    m_labelRect = GUILayoutUtility.GetLastRect();
                }
                GUILayout.FlexibleSpace();
                using (new SA_GuiBeginVertical()) {
                    GUILayout.Space(m_labelRect.height - DESCRIBTION_LABEL_ONE_LIEN_HEIGHT);
                    bool click = m_showMoreButton.Draw(GUILayout.Height(22), GUILayout.Width(22));
                    if (click) {
                        if (m_ShowExtraFields.faded.Equals(0f) || m_ShowExtraFields.faded.Equals(1f)) {
                            m_ShowExtraFields.target = !m_ShowExtraFields.target;
                            if (m_ShowExtraFields.target) {
                                m_showMoreButton.SetContent(new GUIContent(m_expandClosedIcon));
                            } else {
                                m_showMoreButton.SetContent(new GUIContent(m_expandOpenIcon));
                            }
                        }
                    }
                }

                GUILayout.Space(5);
            }



            if (EditorGUILayout.BeginFadeGroup(m_ShowExtraFields.faded)) {
                GUILayout.Space(5);
                DrawFeaturesList();
                GUILayout.Space(5);
            }
            EditorGUILayout.EndFadeGroup();


            GUILayout.Space(5);
            EditorGUILayout.BeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
        }

      

        public void DrawHeaderUI() {

            CheckServiceAvalibility();

            EditorGUILayout.BeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle);
            {
                GUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(SA_PluginSettingsWindowStyles.INDENT_PIXEL_SIZE);
                    EditorGUILayout.LabelField(Title, SA_PluginSettingsWindowStyles.LabelHeaderStyle);

                    GUILayout.FlexibleSpace();
                    DrawServiceStateInteractive();
                    GUILayout.Space(SA_PluginSettingsWindowStyles.INDENT_PIXEL_SIZE);

                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(8);


                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(SA_PluginSettingsWindowStyles.INDENT_PIXEL_SIZE);
                    EditorGUILayout.LabelField(Description, SA_PluginSettingsWindowStyles.DescribtionLabelStyle);


                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(25);
            }
            EditorGUILayout.EndVertical();
        }

        public void DrawServiceUI() {


            DrawGettingStartedBlock();

            OnServiceUI();


            DrawServiceRequirements();
            DrawSupportedPlatfromsBlock();


        }

        protected virtual void DrawGettingStartedBlock() {
            using (new SA_WindowBlockWithIndent(new GUIContent("Getting Started"))) {
                GettingStartedBlock();
                GUILayout.Space(-5);
                EditorGUI.indentLevel--;
                DrawFeaturesList();
                EditorGUI.indentLevel++;
            }
        }

        protected void DrawSupportedPlatfromsBlock() {
            using (new SA_WindowBlockWithSpace(new GUIContent("Supported Platfroms"))) {
                using (new SA_GuiBeginHorizontal()) {
                    foreach (var platform in SupportedPlatfroms) {
                        GUILayout.Label(platform, SA_PluginSettingsWindowStyles.AssetLabel);
                    }
                }
            }
        }


        protected virtual void GettingStartedBlock() {
           // EditorGUILayout.LabelField(Description, SA_PluginSettingsWindowStyles.DescribtionLabelStyle);
        }

        protected virtual int IconSize {
            get {
                return 25;
            }
        }

        protected virtual int TitleVerticalSpace {
            get {
                return 4;
            }
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------


        private void DrawFeaturesList() {
            EditorGUILayout.Space();

            using (new SA_GuiIndentLevel(1)) {
                for (int i = 0; i < m_features.Count; i += 2) {
                    EditorGUILayout.BeginHorizontal();

                    m_features[i].DrawLink(GUILayout.Width(150));
                    if (m_features.Count > (i + 1)) {
                        m_features[i + 1].DrawLink(GUILayout.Width(150));
                    }
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
            }

        }



        protected virtual bool DrawServiceStateInfo() {
            bool click =  m_blockAPIStateLabel.Draw(GUILayout.Height(25), GUILayout.Width(32));
            GUILayout.Space(5);

            return click;
        }
        protected virtual void DrawServiceStateInteractive() {
            if (CanBeDisabled) {
                bool click = m_apiEnableButton.Draw(GUILayout.Width(50), GUILayout.Height(25));
                if (click) {
                    GUI.changed = true;
                    Resolver.IsSettingsEnabled = !Resolver.IsSettingsEnabled;
                }
            }
        }

        protected virtual void CheckServiceAvalibility() {
            if (Resolver.IsSettingsEnabled) {
                m_blockAPIStateLabel.SetStyle(OnStyle);
                m_blockAPIStateLabel.SetContent(new GUIContent("ON"));


                m_apiEnableButton.SetContent(new GUIContent(m_onToggle));
                m_apiEnableButton.SetColor(SA_PluginSettingsWindowStyles.SelectedImageColor);
                m_apiEnableButton.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedImageColor);


            } else {

                m_blockAPIStateLabel.SetStyle(OffStyle);
                m_blockAPIStateLabel.SetContent(new GUIContent("OFF"));


                m_apiEnableButton.SetContent(new GUIContent(m_offToggle));
                m_apiEnableButton.SetColor(SA_PluginSettingsWindowStyles.DisabledImageColor);
                m_apiEnableButton.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedImageColor);

            }
        }


        private GUIStyle m_onStyle = null;
        private GUIStyle OnStyle {
            get {
                if (m_onStyle == null) {
                    m_onStyle = new GUIStyle(SA_PluginSettingsWindowStyles.DescribtionLabelStyle);
                    m_onStyle.fontSize = 14;
                    m_onStyle.fontStyle = FontStyle.Bold;
                    m_onStyle.alignment = TextAnchor.MiddleRight;
                    m_onStyle.normal.textColor = SA_PluginSettingsWindowStyles.SelectedImageColor;
                }

                return m_onStyle;
            }
        }

        private GUIStyle m_offStyle = null;
        private GUIStyle OffStyle {
            get {
                if (m_offStyle == null) {
                    m_offStyle = new GUIStyle(OnStyle);
                    m_offStyle.normal.textColor = SA_PluginSettingsWindowStyles.DisabledImageColor;
                }


                return m_offStyle;
            }
        }

    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace SA.Foundation.Editor
{

    public abstract class SA_PluginSettingsWindow<T> : EditorWindow where T : EditorWindow
    {

        private float m_headerHeight;
        private float m_scrollContentHeight;
        private Vector2 m_scrollPos;

        [SerializeField] bool m_shouldEnabled = false;
        [SerializeField] bool m_shouldAwake = false;


        [SerializeField] string m_headerTitle;
        [SerializeField] string m_headerDescribtion;
        [SerializeField] string m_headerVersion;
        [SerializeField] string m_documentationUrl;

        [SerializeField] ScriptableObject m_serializationStateInidcator;

        [SerializeField] SA_HyperLabel m_documentationLink;

        //MenuTabs
        [SerializeField] protected bool m_isToolBarWasAlreadyCreated = false;
        [SerializeField] protected SA_HyperToolbar m_menuToolbar;
        [SerializeField] protected List<SA_GUILayoutElement> m_tabsLayout = new List<SA_GUILayoutElement>();
      
     

        //--------------------------------------
        // Public Methods
        //--------------------------------------


        public void SetHeaderTitle(string headerTitle) {
            m_headerTitle = headerTitle;
        }

        public void SetHeaderDescription(string headerDescribtion) {
            m_headerDescribtion = headerDescribtion;
        }

        public void SetHeaderVersion(string headerVersion) {
            m_headerVersion = headerVersion;
        }

        public void SetDocumentationUrl(string documentationUrl) {
            m_documentationUrl = documentationUrl;
        }


        public void AddMenuItem(string itemName, SA_GUILayoutElement layout, bool forsed = false) {

            //It could be 2 cases
            //1 When the window is created and we need to create everything
            //2 When Unity called Awake and only ScriptableObjects are destoryed, so we only need to re-create ScriptableObjects
            if (!m_isToolBarWasAlreadyCreated || forsed) {
                var button = new SA_HyperLabel(new GUIContent(itemName), EditorStyles.boldLabel);
                button.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);
                m_menuToolbar.AddButtons(button);
            }

            m_tabsLayout.Add(layout);
            layout.OnAwake();
        }

		//--------------------------------------
        // Virtual Methods
        //--------------------------------------
        

        protected virtual void BeforeGUI() {

        }

        protected virtual void AfterGUI() {

        }

        //--------------------------------------
        // Unity Editor Callbacks
        //--------------------------------------

        private void Awake() {

            if(!m_isToolBarWasAlreadyCreated) {
                OnCreate();
            }
          
            m_tabsLayout = new List<SA_GUILayoutElement>();
            m_shouldAwake = true;
            m_serializationStateInidcator = CreateInstance<ScriptableObject>();

        }

        private void OnEnable() {
            EditorApplication.update += () => {
                Repaint();
            };

            m_shouldEnabled = true;           
        }


        //--------------------------------------
        // Custom Editor Callbacks
        //--------------------------------------

        protected abstract void OnAwake();
        protected virtual void OnCreate() {
            m_menuToolbar = new SA_HyperToolbar();
        }

        private void OnLayoutEnable() {
            foreach (var layout in m_tabsLayout) {
                layout.OnLayoutEnable();
            }

            m_documentationLink = new SA_HyperLabel(new GUIContent("Go To Documentation"), EditorStyles.miniLabel);
            m_documentationLink.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);

            //Update toolbar Styles
            foreach (var button in m_menuToolbar.Buttons) {
                button.SetStyle(EditorStyles.boldLabel);
                button.SetMouseOverColor(SA_PluginSettingsWindowStyles.SelectedElementColor);
            }

        }


        //--------------------------------------
        // GUI
        //--------------------------------------

        private void CheckForGUIEvents() {

            //Just a workaround, since in play-mode scriptable object could get destroyed
            if (m_serializationStateInidcator == null) {
                Awake();
            }

            if (m_shouldAwake) {
                OnAwake();
                m_shouldAwake = false;

                //When entering playmode both OnAwake & OnEnable get called
                //But when we exit playmode only OnAwake is called, so we need to add
                //one more extra OnEnable emulation
                m_shouldEnabled = true;
            }

            if (m_shouldEnabled) {
                OnLayoutEnable();
                m_shouldEnabled = false;
            }

            //first GUI call, we assume all tool bar items has beedn added already
            m_isToolBarWasAlreadyCreated = true;
        }


        private void OnGUI() {

            CheckForGUIEvents();
            BeforeGUI();
            OnLayoutGUI();
            AfterGUI();
        }

        protected virtual void OnLayoutGUI() {
            DrawTopbar();
            DrawHeader();

            int tabIndex = DrawMenu();

            DrawScrollView(() => {
                OnTabsGUI(tabIndex);
            });
          
        }

        protected virtual void OnTabsGUI(int tabIndex) {
            m_tabsLayout[tabIndex].SetPosition(position);
            m_tabsLayout[tabIndex].OnGUI();
        }


        protected void DrawScrollView(Action OnContent) {
            if (Event.current.type == EventType.Repaint) {
                m_headerHeight = GUILayoutUtility.GetLastRect().yMax + SA_PluginSettingsWindowStyles.LAYOUT_PADDING;
            }

            using (new SA_GuiBeginScrollView(ref m_scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height - m_headerHeight))) {

                OnContent.Invoke();

                GUILayout.Space(1);
                if (Event.current.type == EventType.Repaint) {
                    m_scrollContentHeight = GUILayoutUtility.GetLastRect().yMax + SA_PluginSettingsWindowStyles.LAYOUT_PADDING;
                }


                if (Event.current.type == EventType.Layout) {
                    float m_totalHeight = m_scrollContentHeight + m_headerHeight + 20;
                    if (position.height > m_totalHeight) {
                        using (new SA_GuiBeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle)) {
                            GUILayout.Space(position.height - m_totalHeight);
                        }
                    }
                }
            }

        }

        protected int DrawMenu() {
            GUILayout.Space(2);
			int index = m_menuToolbar.Draw ();
            GUILayout.Space(4);

            EditorGUILayout.BeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle);
            GUILayout.Space(5);
            EditorGUILayout.EndVertical();

			return index;
        }

        public void SetSelectedTabIndex(int index) {

            //OMG!!
            //OnAwake
            EditorApplication.delayCall += () => {
                //OnEnabled
                EditorApplication.delayCall += () => {
                    m_menuToolbar.SetSelectedIndex(index);
                };
            };

            //And yes I ams to lazy to add state.
           
        }

        protected void DrawTopbar(Action OnContent = null) {
            GUILayout.Space(2);
            using (new SA_GuiBeginHorizontal()) {
                if(OnContent != null) { OnContent.Invoke();}
                EditorGUILayout.Space();
                DrawDocumnetationLink();
            }
            GUILayout.Space(5);
        }

        protected void DrawDocumnetationLink() {
            float width = m_documentationLink.CalcSize().x + 5f;
            bool clicked = m_documentationLink.Draw(GUILayout.Width(width));
            if (clicked) {
                Application.OpenURL(m_documentationUrl);
            }
        }

        protected void DrawHeader() {

            EditorGUILayout.BeginVertical(SA_PluginSettingsWindowStyles.SeparationStyle);
            {
                GUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(SA_PluginSettingsWindowStyles.INDENT_PIXEL_SIZE);
                    EditorGUILayout.LabelField(m_headerTitle, SA_PluginSettingsWindowStyles.LabelHeaderStyle);
                }
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(8);


                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Space(SA_PluginSettingsWindowStyles.INDENT_PIXEL_SIZE);
                    EditorGUILayout.LabelField(m_headerDescribtion, SA_PluginSettingsWindowStyles.DescribtionLabelStyle);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(2);
                using (new SA_GuiBeginHorizontal()) {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("v: " + m_headerVersion, SA_PluginSettingsWindowStyles.VersionLabelStyle, GUILayout.Width(120));
                    GUILayout.Space(10);
                }

                GUILayout.Space(5);
            }
            EditorGUILayout.EndVertical();

        }

        
        //--------------------------------------
        // Static Methods
        //--------------------------------------

        public static T ShowTowardsInspector(string text, Texture image = null) {
            return ShowTowardsInspector(new GUIContent(text, image));
        }

        public static T ShowTowardsInspector(GUIContent titleContent) {
            Type inspectorType = Type.GetType("UnityEditor.InspectorWindow, UnityEditor.dll");
            var window = EditorWindow.GetWindow<T>(new Type[] { inspectorType });
            window.Show();

            window.titleContent = titleContent;
            window.minSize = new Vector2(350, 100);

            return window;
        }

    }
}
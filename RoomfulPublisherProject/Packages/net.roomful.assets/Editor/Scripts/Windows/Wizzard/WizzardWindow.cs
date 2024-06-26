using System.Collections.Generic;
using StansAssets.Foundation;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class WizardWindow : EditorWindow
    {
        Vector2 m_SectionScrollPos;
        List<WizardSection> m_Sections;
        static WizzardConstants s_Constants;

        AccountPanel m_AccountPanel;
        PublisherUIComponent m_DrawHelper;
        
        //--------------------------------------
        //  Initialisation
        //--------------------------------------

        void OnEnable() {
            m_Sections = new List<WizardSection>();
            m_Sections.Add(new WizardSection("Wizard", new WizzardPanel(this)));
            m_Sections.Add(new WizardSection("Props", new PropsList(this)));
            m_Sections.Add(new WizardSection("Styles", new StylesList(this)));
            m_Sections.Add(new WizardSection("Environments", new EnvironmentList(this)));

            m_Sections.Add(new WizardSection("--------------", new SeparatorPanel(this)));
            m_Sections.Add(new WizardSection("Settings", new SettingsPanel(this)));

            m_AccountPanel = new AccountPanel(this);
            m_Sections.Add(new WizardSection("Account", m_AccountPanel));

            m_DrawHelper = new PublisherUIComponent();
            RoomfulPublisher.Session.CheckAuth(() => { });
        }

        //--------------------------------------
        //  GUI Render
        //--------------------------------------

        void OnGUI() {
            GUI.changed = false;
            EditorGUIUtility.labelWidth = 200f;
            
            if (RoomfulPublisher.Session.IsLoggedOut) {
                SwitchTab(WizardTabs.Account);
            }

            GUILayout.BeginHorizontal();

            m_SectionScrollPos = GUILayout.BeginScrollView(m_SectionScrollPos, Constants.sectionScrollView, GUILayout.MinWidth(180f), GUILayout.ExpandWidth(false));

            GUILayout.Space(40f);
            for (var i = 0; i < m_Sections.Count; i++) {
                var section = m_Sections[i];

                var rect = GUILayoutUtility.GetRect(section.Content, Constants.sectionElement, GUILayout.ExpandWidth(true));

                if (section == SelectedSection && Event.current.type == EventType.Repaint) {
                    var color = EditorGUIUtility.isProSkin
                        ? new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f)
                        : new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);

                    GUI.DrawTexture(rect, Texture2DUtility.MakePlainColorImage(color));
                }

                EditorGUI.BeginChangeCheck();

                GUI.enabled = section.CanBeSelected;
                if (GUI.Toggle(rect, AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex == i, section.Content, Constants.sectionElement)) {
                    if (section.CanBeSelected) {
                        AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = i;
                    }
                }

                GUI.enabled = true;

                if (EditorGUI.EndChangeCheck()) {
                    GUIUtility.keyboardControl = 0;
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(s_Constants.settingsBoxTitle);
            {
                GUILayout.FlexibleSpace();
                var platformClick = GUILayout.Button(EditorUserBuildSettings.activeBuildTarget.ToString(), EditorStyles.label);
                if (platformClick) {
                    SwitchTab(WizardTabs.Platforms);
                }

                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
            GUILayout.Space(10f);

            GUILayout.BeginVertical();
            
            
            if (RoomfulPublisher.Session.AuthCheckInProgress)
            {
                m_DrawHelper.DrawPreloaderAt(new Rect(400, 150, 20, 20));
            }
            else
            {
                SelectedSection.Draw();
            }
            
            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
            
            Repaint();
        }

        //--------------------------------------
        //  Get / Set
        //--------------------------------------

        WizardSection SelectedSection => m_Sections[AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex];
        public static WizzardConstants Constants => s_Constants ?? (s_Constants = new WizzardConstants());

        //--------------------------------------
        //  Public Methods
        //--------------------------------------

        public void SwitchTab(WizardTabs tab) {
            AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = (int) tab;
        }
    }
}
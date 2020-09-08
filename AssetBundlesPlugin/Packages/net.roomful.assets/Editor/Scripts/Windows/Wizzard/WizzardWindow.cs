using System.Collections.Generic;
using StansAssets.Foundation;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.Editor
{
    internal class WizardWindow : EditorWindow
    {
        private Vector2 m_sectionScrollPos;
        private List<WizzardSection> m_sections;
        private static WizzardConstants s_constants = null;

        //--------------------------------------
        //  Initialisation
        //--------------------------------------

        private void OnEnable() {
            m_sections = new List<WizzardSection>();
            m_sections.Add(new WizzardSection("Wizard", new WizzardPanel(this)));
            m_sections.Add(new WizzardSection("Props", new PropsList(this)));
            m_sections.Add(new WizzardSection("Styles", new StylesList(this)));
            m_sections.Add(new WizzardSection("Environments", new EnvironmentList(this)));

            m_sections.Add(new WizzardSection("--------------", new SeparatorPanel(this)));

            m_sections.Add(new WizzardSection("Settings", new SettingsPanel(this)));
            m_sections.Add(new WizzardSection("Account", new AccountPanel(this)));
        }

        //--------------------------------------
        //  GUI Render
        //--------------------------------------

        private void OnGUI() {
            GUI.changed = false;
            EditorGUIUtility.labelWidth = 200f;

            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                SwitchTab(WizardTabs.Account);
            }

            GUILayout.BeginHorizontal();

            m_sectionScrollPos = GUILayout.BeginScrollView(m_sectionScrollPos, Constants.sectionScrollView, GUILayout.Width(120f));

            GUILayout.Space(40f);
            for (var i = 0; i < m_sections.Count; i++) {
                var section = m_sections[i];

                var rect = GUILayoutUtility.GetRect(section.Content, Constants.sectionElement, GUILayout.ExpandWidth(true));

                if (section == SelectedSection && Event.current.type == EventType.Repaint) {
                    var color = EditorGUIUtility.isProSkin 
                        ? new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f) 
                        : new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);

                    GUI.DrawTexture(rect, Texture2DUtility.MakePlainColorImage(color));
                }

                EditorGUI.BeginChangeCheck();
                if (GUI.Toggle(rect, AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex == i, section.Content, Constants.sectionElement)) {
                    if (section.CanBeSelected) {
                        AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = i;
                    }
                }

                if (EditorGUI.EndChangeCheck()) {
                    GUIUtility.keyboardControl = 0;
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(s_constants.settingsBoxTitle);
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
            SelectedSection.Draw();
            GUILayout.Space(5f);
            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();
        }

        //--------------------------------------
        //  Get / Set
        //--------------------------------------

        private WizzardSection SelectedSection => m_sections[AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex];
        public static WizzardConstants Constants => s_constants ?? (s_constants = new WizzardConstants());

        //--------------------------------------
        //  Public Methods
        //--------------------------------------

        public void SwitchTab(WizardTabs tab) {
            AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = (int) tab;
        }
    }
}
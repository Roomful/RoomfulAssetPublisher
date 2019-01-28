using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor {
	public class WizardWindow : EditorWindow {


        private Vector2 m_sectionScrollPos;
        private List<WizzardSection> m_sections;
        private static WizzardConstants m_constants = null;



		//--------------------------------------
		//  Initialisation
		//--------------------------------------


		private void OnEnable() {

            m_sections = new List<WizzardSection>();
            m_sections.Add(new WizzardSection("Wizard", new WizzardPanel(this)));
            m_sections.Add(new WizzardSection("Props",   new PropsList(this)));
            m_sections.Add(new WizzardSection("Styles", new StylesList(this)));
            m_sections.Add(new WizzardSection("Environments", new EnvironmentList(this)));
            m_sections.Add(new WizzardSection("Batch reupload", new BatchReuploadPanel(this)));

            m_sections.Add(new WizzardSection("--------------", new SeparatorPanel(this)));


            m_sections.Add(new WizzardSection("Settings", new SettingsPanel(this)));
            m_sections.Add(new WizzardSection("Account",  new AccountPanel(this)));
		}


		//--------------------------------------
		//  GUI Render
		//--------------------------------------

		void OnGUI() {

            titleContent = new GUIContent("Roomful Plugin -  " + AssetBundlesSettings.WEB_SERVER_URL);

			GUI.changed = false;
			EditorGUIUtility.labelWidth = 200f;
		
            if (AssetBundlesSettings.Instance.IsLoggedIn) {
                SiwtchTab(WizardTabs.Account);
            }

            GUILayout.BeginHorizontal(new GUILayoutOption[0]);


            m_sectionScrollPos = GUILayout.BeginScrollView(this.m_sectionScrollPos, Constants.sectionScrollView, new GUILayoutOption[]{ GUILayout.Width(120f)});

			GUILayout.Space(40f);
			for (int i = 0; i < m_sections.Count; i++) {
				var section = m_sections[i];

                Rect rect = GUILayoutUtility.GetRect(section.Content, Constants.sectionElement, new GUILayoutOption[]{GUILayout.ExpandWidth(true)});

				if (section == SelectedSection && Event.current.type == EventType.Repaint) {

                    Color color;
                    if (EditorGUIUtility.isProSkin) {
                        color = new Color(62f / 255f, 95f / 255f, 150f / 255f, 1f);
                    } else {
                        color = new Color(62f / 255f, 125f / 255f, 231f / 255f, 1f);
                    }

                    GUI.DrawTexture(rect, IconManager.GetIcon(color));
				}

				EditorGUI.BeginChangeCheck();
                if (GUI.Toggle(rect, AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex == i, section.Content, Constants.sectionElement)) {
                    if(section.CanBeSelected) {
                        AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = i;
                    }
                   
				} if (EditorGUI.EndChangeCheck()){
					GUIUtility.keyboardControl = 0;
				}
			}


            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal(WizardWindow.m_constants.settingsBoxTitle);
            {
                GUILayout.FlexibleSpace();
                bool platfromClick = GUILayout.Button(EditorUserBuildSettings.activeBuildTarget.ToString(), EditorStyles.label);
                if (platfromClick) {
                    SiwtchTab(WizardTabs.Platforms);
                }
                GUILayout.FlexibleSpace();
            } GUILayout.EndHorizontal();


            GUILayout.EndScrollView();
			GUILayout.Space(10f);

			GUILayout.BeginVertical(new GUILayoutOption[0]);
            SelectedSection.Draw();
			GUILayout.Space(5f);
			GUILayout.EndVertical();


			GUILayout.Space(10f);
			GUILayout.EndHorizontal();

		}



		//--------------------------------------
		//  Get / Set
		//--------------------------------------

        private WizzardSection SelectedSection {
			get {
				return m_sections[AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex];
			}
		}

	

        public static WizzardConstants Constants  {
            get {
                if(m_constants == null) {
                    m_constants = new WizzardConstants();
                }

                return m_constants;
            }
        }


        //--------------------------------------
        //  Public Methods
        //--------------------------------------

        public void SiwtchTab(WizardTabs tab) {
            AssetBundlesSettings.Instance.WizardWindowSelectedTabIndex = (int)tab;
		}


		
	}
}

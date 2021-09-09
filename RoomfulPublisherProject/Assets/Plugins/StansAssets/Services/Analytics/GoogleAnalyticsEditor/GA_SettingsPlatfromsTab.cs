using UnityEngine;
using System.Collections;
using UnityEditor;
using StansAssets.Plugins.Editor;

namespace SA.Analytics.Google
{

    public class GA_SettingsPlatfromsTab : IMGUILayoutElement
    {

        private static GUIContent prodModeLabel = new GUIContent("Production [?]:", "This account will be used if testing mode disabled.");
        private static GUIContent testingModeLabel = new GUIContent("Testing [?]:", "This account will be used if testing mode enabled");


        public override void OnGUI() {
            if (GA_Settings.Instance.accounts.Count == 0) {
                EditorGUILayout.HelpBox("Setup at least one Google Analytics Profile", MessageType.Error);
            } else {
                foreach (GA_PlatfromBound bound in GA_Settings.Instance.platfromBounds) {

                    EditorGUI.indentLevel = 1;
                    EditorGUILayout.BeginVertical(GUI.skin.box);



                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel = 0;
                    EditorGUILayout.LabelField(bound.platfrom.ToString());
                    EditorGUILayout.EndHorizontal();


                    EditorGUI.indentLevel = 1;


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(prodModeLabel);
                    EditorGUI.BeginChangeCheck();


                    int index = GA_Settings.Instance.GetProfileIndexForPlatfrom(bound.platfrom, false);
                    index = EditorGUILayout.Popup(index, GA_Settings.Instance.GetProfileNames());


                    if (EditorGUI.EndChangeCheck()) {
                        GA_Settings.Instance.SetProfileIndexForPlatfrom(bound.platfrom, index, false);
                    }
                    EditorGUILayout.EndHorizontal();



                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(testingModeLabel);
                    EditorGUI.BeginChangeCheck();


                    index = GA_Settings.Instance.GetProfileIndexForPlatfrom(bound.platfrom, true);
                    index = EditorGUILayout.Popup(index, GA_Settings.Instance.GetProfileNames());


                    if (EditorGUI.EndChangeCheck()) {
                        GA_Settings.Instance.SetProfileIndexForPlatfrom(bound.platfrom, index, true);
                    }

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.EndVertical();
                }
            }
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;
using Rotorz.ReorderableList;

namespace SA.Productivity.Console
{
    public class UCL_CurrentWindowTab : IMGUILayoutElement {

        private int x;

        private const string DESCRIBTION = "This tab settings is only related to the console window on your machine. " +
            "Changes you made here, will be applied once the Preferences window is closed.";

        private UCL_ConsoleWindowState m_state;


        public override void OnLayoutEnable() {
            m_state = UCL_Settings.ConsoleWindowState;
        }
 
        public override void OnGUI() {

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(DESCRIBTION, MessageType.Info);


            using (new IMGUIBlockWithIndent(new GUIContent("Settings"))) {
                m_state.DisplayCollapse = IMGUILayout.ToggleFiled("Collapse Button", m_state.DisplayCollapse, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                m_state.DisplayClearOnPlay = IMGUILayout.ToggleFiled("Clear On Play Button", m_state.DisplayClearOnPlay, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                m_state.DisplayPauseOnError = IMGUILayout.ToggleFiled("Error Pause Button", m_state.DisplayPauseOnError, IMGUIToggleStyle.ToggleType.EnabledDisabled);

                m_state.DisplaySeartchBar = IMGUILayout.ToggleFiled("Seartch Bar", m_state.DisplaySeartchBar, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                m_state.RichText = IMGUILayout.ToggleFiled("Rich Text", m_state.RichText, IMGUIToggleStyle.ToggleType.EnabledDisabled);
            }


            using (new IMGUIBlockWithIndent(new GUIContent("Tags"))) {
                EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true)); {
                    ReorderableListGUI.ListField(UCL_Settings.Instance.Tags,
                         (Rect position, UCL_ConsoleTag tag) => {
                            position.x -= 15;

                            int dockedToggleWidth = 40;
                            Rect labelRect = new Rect(position);
                            labelRect.width = position.width - dockedToggleWidth;

                            EditorGUI.LabelField(labelRect, tag.DisaplyContent);

                            Rect docedRect = new Rect(position);
                            docedRect.x = labelRect.x + labelRect.width;
                            docedRect.width = dockedToggleWidth;
                            
                            bool docked = m_state.IsTagDocked(tag);
                            docked = EditorGUI.Toggle(docedRect, docked);

                            m_state.SetTagDockedState(tag, docked);
                           
                            return tag;
                        }, 
                        () => {

                        },
                        ReorderableListFlags.HideAddButton | ReorderableListFlags.HideRemoveButtons
                     );
    

                } EditorGUILayout.EndVertical();
            }


            if(EditorGUI.EndChangeCheck()) {
                UCL_Settings.SaveConsoleWindowState(m_state);
            }
        }
    }
}
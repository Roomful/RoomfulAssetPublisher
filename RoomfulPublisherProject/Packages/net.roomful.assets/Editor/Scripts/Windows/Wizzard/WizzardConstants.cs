﻿using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal class WizzardConstants
    {
        public GUIStyle sectionScrollView = "PreferencesSectionBox";
        public GUIStyle settingsBoxTitle = "OL Title";
        public GUIStyle settingsBox = "OL Box";
        public GUIStyle errorLabel = "WordWrappedLabel";
        public GUIStyle sectionElement = "PreferencesSection";
        public GUIStyle evenRow = "CN EntryBackEven";
        public GUIStyle oddRow = "CN EntryBackOdd";
        public GUIStyle keysElement = "PreferencesKeysElement";
        public GUIStyle warningIcon = "CN EntryWarn";
        public GUIStyle sectionHeader = new GUIStyle(EditorStyles.largeLabel);
        public GUIStyle cacheFolderLocation = new GUIStyle(GUI.skin.label);
        public GUIStyle toolbarStyle;
        public GUIStyle toolbarSearchTextFieldStyle;
        public GUIStyle toolbarSearchCancelButtonStyle;

        public WizzardConstants() {
            sectionHeader = new GUIStyle(EditorStyles.largeLabel);

            sectionScrollView = new GUIStyle(sectionScrollView);
            sectionScrollView.overflow.bottom++;

            toolbarStyle = GUI.skin.FindStyle("Toolbar");
            toolbarSearchTextFieldStyle = GUI.skin.FindStyle("ToolbarSearchTextField");
            toolbarSearchCancelButtonStyle = GUI.skin.FindStyle("ToolbarSearchCancelButton");

            sectionHeader.fontStyle = FontStyle.Bold;
            sectionHeader.fontSize = 18;
            sectionHeader.margin.top = 10;
            sectionHeader.margin.left++;

            if (!EditorGUIUtility.isProSkin) {
                sectionHeader.normal.textColor = new Color(0.4f, 0.4f, 0.4f, 1f);
            }
            else {
                sectionHeader.normal.textColor = new Color(0.7f, 0.7f, 0.7f, 1f);
            }

            cacheFolderLocation.wordWrap = true;
        }
    }
}
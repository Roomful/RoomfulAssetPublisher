using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using StansAssets.Foundation.Editor;
using StansAssets.Plugins.Editor;


namespace SA.Productivity.Console
{
    public class UCL_AboutTab : IMGUIAboutTab
    {
        private const string DESCRIPTION = "The Ultimate Console Plugin v. {0}\n" +
           "Plugin adds and extended Unity like console window with set of additional features. " +
           "There is also an ability to use native console view on diffret platfroms " +
           "For feature requests or bug reports please use the information below.";

        public override void OnGUI() {

            using (new IMGUIBlockWithIndent(new GUIContent("About"))) {
                EditorGUILayout.LabelField(string.Format(DESCRIPTION, UCL_Settings.GetFormattedVersion()), SettingsWindowStyles.DescriptionLabelStyle);
                EditorGUILayout.Space();
            }

            base.OnGUI();
        }
    }
}
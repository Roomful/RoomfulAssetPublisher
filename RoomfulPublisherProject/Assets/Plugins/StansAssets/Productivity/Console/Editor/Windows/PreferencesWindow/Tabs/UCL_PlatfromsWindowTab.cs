using UnityEngine;
using UnityEditor;
using StansAssets.Plugins.Editor;



namespace SA.Productivity.Console
{
    public class UCL_PlatfromsWindowTab : IMGUILayoutElement {
        private const string DESCRIBTION = "The Plugin will also give you an ability to collected, show and share logs per platfroms.";
       
        public override void OnGUI() {

            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(DESCRIBTION, MessageType.Info);

            using (new IMGUIBlockWithIndent(new GUIContent("iOS"))) {
                UCL_PlatfromsLogSettings.Instance.iOS_LogsRecord = IMGUILayout.ToggleFiled("Logs Record", UCL_PlatfromsLogSettings.Instance.iOS_LogsRecord, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                UCL_PlatfromsLogSettings.Instance.iOS_OverrideLogsOutput = IMGUILayout.ToggleFiled("Override XCode Output", UCL_PlatfromsLogSettings.Instance.iOS_OverrideLogsOutput, IMGUIToggleStyle.ToggleType.EnabledDisabled);
            }

            using (new IMGUIBlockWithIndent(new GUIContent("Android"))) {
                UCL_PlatfromsLogSettings.Instance.Android_LogsRecord = IMGUILayout.ToggleFiled("Logs Record", UCL_PlatfromsLogSettings.Instance.Android_LogsRecord, IMGUIToggleStyle.ToggleType.EnabledDisabled);
                UCL_PlatfromsLogSettings.Instance.Android_OverrideLogsOutput = IMGUILayout.ToggleFiled("Override LogCat Output", UCL_PlatfromsLogSettings.Instance.Android_OverrideLogsOutput, IMGUIToggleStyle.ToggleType.EnabledDisabled);
            }

        }
    }
}
using net.roomful.assets.serialization;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{
    [CustomEditor(typeof(ButtonTrigger), true)]
    public class ButtonTriggerEditor : ButtonEditor
    {
        protected ButtonTrigger m_trigger;

        protected override void OnEnable() {
            base.OnEnable();

            m_trigger = target as ButtonTrigger;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            
            EditorGUILayout.Space();

            var guiEnabled = GUI.enabled;
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Target", m_trigger.Target, typeof(Button), true);
            GUI.enabled = guiEnabled;
        }
    }
}
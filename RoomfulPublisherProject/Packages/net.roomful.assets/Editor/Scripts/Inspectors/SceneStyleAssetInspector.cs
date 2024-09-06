using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    [CustomEditor(typeof(SceneStyleAsset))]
    class SceneStyleAssetInspector :  AssetInspector<StyleAssetTemplate, SceneStyleAsset>
    {
        SerializedProperty m_StyleId;
        SerializedProperty m_Title;
        SerializedProperty m_Thumbnail;

        void OnEnable() {
            m_StyleId = serializedObject.FindProperty("m_StyleId");
            m_Title = serializedObject.FindProperty("m_Title");
            m_Thumbnail = serializedObject.FindProperty("m_Thumbnail");
            Asset.Validate();
        }
        

        public override void OnInspectorGUI()
        {
            var env = FindObjectOfType<Environment>();
            if (env != null)
            {
                Debug.Log(env, env);
                env.hideFlags = HideFlags.None;
                GameObject.DestroyImmediate(env);
            }

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_StyleId);
                EditorGUILayout.PropertyField(m_Title);
                EditorGUILayout.PropertyField(m_Thumbnail);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Asset.Validate();
            }
            
            
            DrawActionButtons();
            
            serializedObject.ApplyModifiedProperties();
        }
        
       


        protected override SceneStyleAsset Asset => target as SceneStyleAsset;
    }
}
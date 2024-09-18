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
        
        SerializedProperty m_HomePosition;
        SerializedProperty m_Price;
        SerializedProperty m_SortingScore;
        SerializedProperty m_Tags;

        void OnEnable() {
            m_StyleId = serializedObject.FindProperty("m_StyleId");
            m_Title = serializedObject.FindProperty("m_Title");
            
            m_HomePosition = serializedObject.FindProperty("m_HomePosition");
            m_Price = serializedObject.FindProperty("m_Price");
            m_SortingScore = serializedObject.FindProperty("m_SortingScore");
            m_Tags = serializedObject.FindProperty("m_Tags");
            
            m_Thumbnail = serializedObject.FindProperty("m_Thumbnail");
            Asset.Validate();
        }
        

        public override void OnInspectorGUI()
        {
            // No environment for the scene styles.
            var env = FindObjectOfType<Environment>();
            if (env != null)
            {
                env.hideFlags = HideFlags.None;
                DestroyImmediate(env);
            }

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_StyleId);
                EditorGUILayout.PropertyField(m_Title);
                EditorGUILayout.PropertyField(m_Thumbnail);
                
                EditorGUILayout.PropertyField(m_HomePosition);
                EditorGUILayout.PropertyField(m_Price);
                EditorGUILayout.PropertyField(m_SortingScore);
                EditorGUILayout.PropertyField(m_Tags);
                
                
                serializedObject.ApplyModifiedProperties();
            }
            if (EditorGUI.EndChangeCheck())
            {
                Asset.Validate();
            }
            
            DrawActionButtons(false);
        }
        
       


        protected override SceneStyleAsset Asset => target as SceneStyleAsset;
    }
}
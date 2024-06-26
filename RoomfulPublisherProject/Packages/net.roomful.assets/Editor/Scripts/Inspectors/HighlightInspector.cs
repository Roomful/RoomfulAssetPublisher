using HighlightPlus;
using net.roomful.assets;
using net.roomful.assets.serialization;
using UnityEditor;
using UnityEngine;

namespace RF.AssetBundles.Serialization
{
    [CustomEditor(typeof(Highlight), true)]
    public class HighlightInspector : Editor
    {
        const string k_HighlightEffectPrefabPath = "Highlight/HighlightEditor";
        bool m_ShowHighlightEffect = true;
        HighlightEffect m_HighlightEffect;

        Editor m_ProfileEditor;
        bool m_ShowProfileEditor;
        
        void OnEnable()
        {
            UpdatePreview();
            
            EditorApplication.update += EditorUpdate;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;
        }

        void OnDisable()
        {
            UpdatePreview(false);
            
            EditorApplication.update -= EditorUpdate;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;
        }
        
        void EditorUpdate()
        {
            // SceneView.lastActiveSceneView.Repaint();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
        
        void OnUndoRedoPerformed()
        {
            UpdatePreview();
        }
        
        public override void OnInspectorGUI() {
            if (targets.Length > 1) {
                return;
            }
            
            EditorGUI.BeginChangeCheck();

            DrawDefaultInspectorGUI();

            DrawHighlightProfileInspectorGUI();
            
            if (EditorGUI.EndChangeCheck()) {
                UpdatePreview();
            }
        }

        void ApplyEffect(Highlight highlight)
        {
            var highlightInstance = PrefabManager.CreatePrefab(k_HighlightEffectPrefabPath);
            m_HighlightEffect = highlightInstance.GetComponent<HighlightEffect>();
            m_HighlightEffect.transform.SetParent(highlight.transform);
            m_HighlightEffect.SetTarget(highlight.transform);
            m_HighlightEffect.Apply(highlight);
            
            m_HighlightEffect.SetHighlighted(true);
        }

        void StopEffect(Highlight highlight)
        {
            if (highlight != null)
            {
                foreach (var helper in highlight.GetComponentsInChildren<HighlightEffect>()) {
                    DestroyImmediate(helper.gameObject);
                } 
            }
        }

        void UpdatePreview(bool reapply = true)
        {
            if (!reapply && m_HighlightEffect != null)
            {
                DestroyImmediate(m_HighlightEffect.gameObject);
                m_HighlightEffect = null;
            }

            if (!reapply && m_ProfileEditor != null)
            {
                DestroyImmediate(m_ProfileEditor);
                m_ProfileEditor = null;
            }

            EditorApplication.delayCall += () =>
            {
                if (target is Highlight highlight)
                {
                    StopEffect(highlight);
                    if (reapply && m_ShowHighlightEffect)
                        ApplyEffect(highlight);
                }
            };
        }

        void DrawDefaultInspectorGUI()
        {
            var prevColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.yellow;
            GUILayout.Label(new GUIContent("IMPORTANT. If HighlightProfile is set, other settings below (except Tooltip) will be ignored. Remove profile to use less detailed settings below. Profile Inspector located below the default one.", EditorGUIUtility.IconContent("Warning@2x").image), EditorStyles.helpBox);
            GUI.backgroundColor = prevColor;
            
            GUILayout.Space(15f);
            prevColor = GUI.backgroundColor;
            GUI.backgroundColor = m_ShowHighlightEffect ? Color.green : Color.white;
            var content = m_ShowHighlightEffect ? 
                new GUIContent("Preview", EditorGUIUtility.IconContent("animationvisibilitytoggleon").image, "Click to disable preview.") : 
                new GUIContent("Preview", EditorGUIUtility.IconContent("animationvisibilitytoggleoff").image, "Click to enable preview.");
            m_ShowHighlightEffect = GUILayout.Toggle(m_ShowHighlightEffect, content, EditorStyles.toolbarButton);
            GUI.backgroundColor = prevColor;
            
            DrawDefaultInspector();
        }
        
        void DrawHighlightProfileInspectorGUI()
        {
            if (target is Highlight highlight && highlight.Profile != null)
            {
                if (m_ProfileEditor == null || (m_ProfileEditor.target == null || m_ProfileEditor.target != highlight.Profile))
                {
                    m_ProfileEditor = CreateEditor(highlight.Profile);
                }
            }
            else
            {
                m_ProfileEditor = null;
            }

            if (m_ProfileEditor != null)
            {
                GUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(15f);
                        m_ShowProfileEditor = EditorGUILayout.Foldout(m_ShowProfileEditor, "PROFILE INSPECTOR");
                    }
                    GUILayout.EndHorizontal();
                    
                    if(m_ShowProfileEditor)
                        m_ProfileEditor.OnInspectorGUI();
                }
                GUILayout.EndVertical();
            }
        }
    }
}

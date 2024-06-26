using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
    [InitializeOnLoad]
    public static class SceneViewEffectsWindow
    {
        private static SceneView s_sceneView;
        private static bool s_tonemappingToggleState = true;
        private static bool s_prevTonemappingToggleState = true;
        private static bool s_bloomToggleState = true;
        private static bool s_prevBloomToggleState = true;
        
        static SceneViewEffectsWindow() {
            SceneView.duringSceneGui += DrawWindow;
        }
        
        private static void DrawWindow(SceneView sceneView) {
            if (Object.FindObjectOfType<EditorScenePlanarReflections>() != null) {
                s_sceneView = sceneView;
                GUI.Window(
                    0,
                    new Rect(s_sceneView.position.width - 210, s_sceneView.position.height - 80, 210, 80),
                    MyWindow,
                    "");
            }
        }
        private static void MyWindow(int windowID) {
            s_tonemappingToggleState = GUI.Toggle(new Rect(10, 10, 190, 20), GetPostEffectsVolumeComponent(typeof(Tonemapping)), "Tonemapping");
            s_bloomToggleState = GUI.Toggle(new Rect(10, 30, 190, 20), GetPostEffectsVolumeComponent(typeof(Bloom)), "Bloom");
            if (s_prevTonemappingToggleState != s_tonemappingToggleState) {
                s_prevTonemappingToggleState = s_tonemappingToggleState;
                SetPostEffectsVolumeComponent(typeof(Tonemapping), s_tonemappingToggleState);
            }
            if (s_prevBloomToggleState != s_bloomToggleState) {
                s_prevBloomToggleState = s_bloomToggleState;
                SetPostEffectsVolumeComponent(typeof(Bloom), s_bloomToggleState);
            }

            if (GUI.Button(new Rect(10, 50, 190, 20), "Update Planar Reflections")) {
                EditorScenePlanarReflections.UpdatePlanarReflections(null);
            }
            GUI.DragWindow(new Rect(0, 0, s_sceneView.position.width, s_sceneView.position.height));
        }
        
        static bool GetPostEffectsVolumeComponent(Type type) {
            var volume = Object.FindObjectOfType<Volume>();
            var state = true;
            if (volume != null) {
                volume.sharedProfile.TryGet(type, out VolumeComponent component);
                state = component.active;
            }
            return state;
        }
        static void SetPostEffectsVolumeComponent(Type type, bool state) {
            var volume = Object.FindObjectOfType<Volume>();
            if (volume != null) {
                volume.sharedProfile.TryGet(type, out VolumeComponent component);
                component.active = state;
            }
        }
    }
}
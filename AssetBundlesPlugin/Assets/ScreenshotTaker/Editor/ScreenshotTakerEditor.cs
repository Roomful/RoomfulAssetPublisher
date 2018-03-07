using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

[CanEditMultipleObjects]
[CustomEditor(typeof(ScreenshotTaker))]
public class ScreenshotTakerEditor : Editor {

    SerializedProperty cameraResMultiplier;
    SerializedProperty fileName;
    SerializedProperty captureBackground;
    SerializedProperty useSceneName;


    void OnEnable()
    {
        cameraResMultiplier = serializedObject.FindProperty("cameraResMultiplier");
        fileName            = serializedObject.FindProperty("fileName");
        captureBackground   = serializedObject.FindProperty("captureBackground");
        useSceneName        = serializedObject.FindProperty("useSceneNameAsFileName");
    }




    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(
            cameraResMultiplier,
            new GUIContent("Resolution Multiplier")
            );


        EditorGUILayout.PropertyField(
                captureBackground,
                new GUIContent("Capture background")
            );


        EditorGUILayout.PropertyField(
                useSceneName,
                new GUIContent("Use Scene Name As File Name")
            );


        if (!useSceneName.boolValue)
            EditorGUILayout.PropertyField(fileName, new GUIContent("File name"));





        var screenshotTakers = target as ScreenshotTaker;

        if (GUILayout.Button("Take Screenshot"))
        {
            if (useSceneName.boolValue)
                screenshotTakers.fileName = SceneManager.GetActiveScene().name;

            screenshotTakers.TakeScreenshot();
        }


        serializedObject.ApplyModifiedProperties();
    }

}

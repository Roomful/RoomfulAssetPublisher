using UnityEngine;
using System.Collections;
using System.IO;
using System;

[RequireComponent(typeof(Camera))]
public class ScreenshotTaker : MonoBehaviour {

    private static int imageIndex = 0;

    public bool useSceneNameAsFileName;
    public string fileName = "Screenshot";

    public bool captureBackground = false;

    public float cameraResMultiplier = 1.0f;

    private Camera _camera;
    public Camera SSCamera
    {
        get
        {
            if (!_camera)
                _camera = GetComponent<Camera>();

            return _camera;
        }
    }



    public void TakeScreenshot()
    {        
        RenderTexture rt = new RenderTexture(
                                        (int) (SSCamera.pixelWidth * cameraResMultiplier),
                                        (int) (SSCamera.pixelHeight * cameraResMultiplier),
                                        32
                           );

        Texture2D screenShot = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);




        var pClearFlag = SSCamera.clearFlags;

        if (captureBackground)
            SSCamera.clearFlags = CameraClearFlags.Skybox;
        else
            SSCamera.clearFlags = CameraClearFlags.Depth;


        var pRenderTexture = SSCamera.targetTexture;

        SSCamera.targetTexture = rt;
        SSCamera.Render();

        SSCamera.targetTexture = pRenderTexture;



        SSCamera.clearFlags = pClearFlag;




        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;



        if (Application.isPlaying)
            Destroy(rt);
        else
            DestroyImmediate(rt);




        byte[] pngShotBytes = screenShot.EncodeToPNG();

        var time = DateTime.Now;
        var fn = string.Format("{0}{1}{2}{3}{4}_{5}.png",
                fileName,
                time.Month, time.Day, time.Hour, time.Minute,
                imageIndex
            );

        // https://msdn.microsoft.com/en-us/library/54a0at6s.aspx
        // Any and all directories specified in path are created, unless they already exist
        // or unless some part of path is invalid. If the directory already exists,
        // this method does not create a new directory, but it returns a DirectoryInfo object
        // for the existing directory.
        Directory.CreateDirectory(Application.dataPath + "/Screenshots");

        File.WriteAllBytes(Application.dataPath + "/Screenshots/" + fn, pngShotBytes);




        Debug.LogFormat("Screenshot saved:\n{0}/{1}", Application.dataPath, fn);
        imageIndex++;


        // I keep forgetting to delete the screenshot camera, so I rather add this
        if (Application.isPlaying)
            Destroy(gameObject);
        else
            DestroyImmediate(gameObject);

    }


}

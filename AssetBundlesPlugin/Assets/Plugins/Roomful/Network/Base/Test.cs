using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


using RF.AssetWizzard.Network;

[ExecuteInEditMode]
public class Test : MonoBehaviour {

	void Start() {
        StartCoroutine(GetText());
	}



    IEnumerator GetText() {
        using (UnityWebRequest request = UnityWebRequest.Get("http://unity3d.com/")) {
            yield return request.Send();

            if (request.isNetworkError) // Error
            {
                Debug.Log(request.error);
            } else // Success
              {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }

    private void SendRequest() {
       
		var r = new AssetsList (0, 1, new List<string>());
		r.Send ((result) => {

            Debug.Log("Result");
			Debug.Log(result.Responce.IsSucceeded);

          new GameObject("Hello");

		});

    }


    [ContextMenu("Test")]
    public void TestMe() {
        SendRequest();
    }


   // [ContextMenu("Test2")]
    public void Test2() {
        var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets(0, 1, "");
        allAssetsRequest.Send();
    }
}

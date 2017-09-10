using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using RF.AssetWizzard.Network;

[ExecuteInEditMode]
public class Test : MonoBehaviour {


	void Awake() {
		
	}


    private void SendRequest() {
       
		var r = new AssetsList (0, 1, new List<string>());
		r.Send ((result) => {

			Debug.Log("Result");
			Debug.Log(result.Responce.IsSucceeded);

		});

    }


    [ContextMenu("Test")]
    public void TestMe() {
        SendRequest();
    }


    [ContextMenu("Test2")]
    public void Test2() {
        var allAssetsRequest = new RF.AssetWizzard.Network.Request.GetAllAssets(0, 1, "");
        allAssetsRequest.Send();
    }
}

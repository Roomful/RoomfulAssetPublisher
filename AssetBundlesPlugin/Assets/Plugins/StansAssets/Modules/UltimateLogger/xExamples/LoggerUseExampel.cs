using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoggerUseExampel : MonoBehaviour {

	void Awake() {
	
		SA.UltimateLogger.Logger.Init ();
	
		Debug.LogWarning ("Test LogWarning");
		Debug.LogError ("Test LogError");


		U.LogWarning ("warning with color", Color.blue);
		U.LogWarning ("warning with color", Color.white);
		U.LogWarning ("warning with color", Color.green);

	
		U.Log ("dfdfdd", "Some tag");
		U.Log ("Testing network tag", "network");
		U.Log ("Data income", "in");



		U.Log ("tag test", SA.UltimateLogger.DefaultTags.NETWORK);
		U.Log ("tag test", SA.UltimateLogger.DefaultTags.GAMEPLAY);
		U.Log ("tag test", SA.UltimateLogger.DefaultTags.CLOUD);
		U.Log ("tag test", SA.UltimateLogger.DefaultTags.SERVICE);
		U.Log ("tag test", SA.UltimateLogger.DefaultTags.IN);
		U.Log ("tag test", SA.UltimateLogger.DefaultTags.OUT);

	}

	void FixedUpdate() {
//		Debug.Log ("On FixedUpdate");
	}



	//--------------------------------------
	// Button Handlers
	//--------------------------------------

	public void PrintLog() {
		Debug.Log ("Hello, I am test log print");
		Debug.LogError ("Hello, I am test log print");

		GameObject o = null;
		o.transform.position = Vector3.zero;
	}

	public void GetSessionLog() {
		string log = SA.UltimateLogger.Logger.GetSessionLog ();	
		Debug.Log (log);
	}

	public void ShowSessionLog() {
		SA.UltimateLogger.Logger.ShowSessionLog ();
	}

	public void ShowSharingUI() {
		SA.UltimateLogger.Logger.ShowSharingUI ();
	}


	private void TestException() {
		GameObject go = null;
		go.transform.position = Vector3.zero;
	}
		


}

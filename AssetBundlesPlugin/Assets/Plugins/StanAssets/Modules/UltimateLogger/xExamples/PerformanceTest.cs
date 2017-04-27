using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PerformanceTest : MonoBehaviour {

	void Update() {

		transform.Rotate (Vector3.left * Time.deltaTime * 100f);
	}

	void FixedUpdate() {
	//	Debug.Log ("On FixedUpdate");
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


		


}

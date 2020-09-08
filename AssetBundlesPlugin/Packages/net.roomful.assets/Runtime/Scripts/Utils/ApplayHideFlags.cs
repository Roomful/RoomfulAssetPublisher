using UnityEngine;


[ExecuteInEditMode]
public class ApplayHideFlags : MonoBehaviour {

	public HideFlags flag;

	// Use this for initialization
	void Awake () {
		gameObject.hideFlags = flag;
	}
	

}

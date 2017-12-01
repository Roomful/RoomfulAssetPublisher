using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{

	[ExecuteInEditMode]
	public class Environment : MonoBehaviour {


	    public GameObject Light;
	    public GameObject Walls;
	    public GameObject SizeRef;


	    public bool RenderEnvironment = true;


	    public void Update() {

	        Walls.SetActive(RenderEnvironment);
	        SizeRef.SetActive(RenderEnvironment);


	    }

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{

	[ExecuteInEditMode]
	public class Enviroment : MonoBehaviour {


	    public GameObject Light;
	    public GameObject Walls;
	    public GameObject SizeRef;


	    public bool RenderEnviroment = true;


	    public void Update() {

	        Walls.SetActive(RenderEnviroment);
	        SizeRef.SetActive(RenderEnviroment);


	    }

	}
}
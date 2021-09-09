////////////////////////////////////////////////////////////////////////////////
//
// CRYSTAL CLEAR SOFTWARE
// Copyright 2012 Crystal Clear Software. http://ccsoft.ru
// All Rights Reserved. CCsoft Bear Shooter
// @author Osipov Stanislav lacost.20@gmail.com
//
//
// NOTICE: Crystal Soft does not allow to use, modify, or distribute this file
// for any purpose
//
////////////////////////////////////////////////////////////////////////////////

using System; 
using UnityEngine;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.Networking;

public delegate void OnDocLoaded(string data);


public class GD_WWWDocLoader : MonoBehaviour
{
	
	public OnDocLoaded LoadEvent;
	
	
	//--------------------------------------
	// PUBLIC METHODS
	//-------------------------------------
	
	public static GD_WWWDocLoader createRequest() {
		GameObject inst =  new GameObject("WWWDocLoader");
		return inst.AddComponent<GD_WWWDocLoader> ();
	}
	
	public void send(string url) {
        UnityWebRequest request = new UnityWebRequest(url);
        request.downloadHandler = new DownloadHandlerBuffer();
        StartCoroutine(WaitForRequest(request));
    }
	
	
    public void send(string url, WWWForm form)  {
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        request.downloadHandler = new DownloadHandlerBuffer();
        StartCoroutine(WaitForRequest(request));
    }
	
	
	//--------------------------------------
	// PRIVATE METHODS
	//-------------------------------------
	
	
	private IEnumerator WaitForRequest(UnityWebRequest request) {
        yield return request.SendWebRequest();

        // check for errors
        if (request.error == null) {
			SendEvent(request.downloadHandler.text);
			Destroy(gameObject);
        } else {
			SendEvent(string.Empty);
			Destroy(gameObject);
        }
	}

	private void SendEvent(string data) {
		if (LoadEvent != null) {
			LoadEvent(data);
		}
	}

   
}


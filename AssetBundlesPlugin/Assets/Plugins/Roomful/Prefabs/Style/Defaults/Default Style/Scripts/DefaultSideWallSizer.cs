using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DefaultSideWallSizer : MonoBehaviour {

    private DefaultPanelSizer panelSizer;

    public bool isRightWall = false;

    public Transform doorTop;
    public Transform sideWall;

    public float doorHeight = 3.5f;
    public float doorLength = 4.5f;

    private bool FoundAllComponents
    {
        get
        {
            return panelSizer && doorTop && sideWall;
        }
    }

	
	void Update () {

        if (!FoundAllComponents)
            UpdateReferences();

        if (!FoundAllComponents)
        {
            Debug.Log("Not all pieces were found. Won't resize");
            return;
        }


        transform.localPosition = Vector3.zero;
        doorTop.gameObject.SetActive( panelSizer.panelSize.y >= doorHeight );
        sideWall.gameObject.SetActive(panelSizer.panelSize.z >= doorLength);


        var xPos   = (isRightWall) ? -panelSizer.panelSize.x : 0.0f;
        var scaleX = (isRightWall) ? -1.0f : 1.0f;

        doorTop.localPosition = new Vector3(xPos, doorHeight, 0.0f);
        doorTop.localScale = new Vector3(scaleX, panelSizer.panelSize.y - doorHeight, 4.5f);

        sideWall.localPosition = new Vector3(xPos, 0.0f, -doorLength);
        sideWall.localScale = new Vector3(scaleX, panelSizer.panelSize.y, panelSizer.panelSize.z - doorLength);

    }



    private bool UpdateReferences()
    {
        panelSizer = GetComponentInParent<DefaultPanelSizer>();

        return FoundAllComponents;
    }



}

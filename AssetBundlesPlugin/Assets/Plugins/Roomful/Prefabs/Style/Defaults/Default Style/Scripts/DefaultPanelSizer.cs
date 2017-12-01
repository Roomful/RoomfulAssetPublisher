using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DefaultPanelSizer : MonoBehaviour {

    public Transform FloorParent;
    private Transform floorMesh;
    private Transform floorStripe_L;
    private Transform floorStripe_R;


    public Transform WallParent;
    private Transform wallMesh;



    public Transform CeilingParent;
    private Transform ceilingMesh;
    private Transform ceilingStripe_L;
    private Transform ceilingStripe_R;


    private bool FoundAllComponents
    {
        get
        {
            return floorMesh && floorStripe_L && floorStripe_R &&
                   wallMesh &&
                   ceilingMesh && ceilingStripe_L && ceilingStripe_R;
        }
    }


    [Space(16.0f)]

    public Vector3 panelSize = new Vector3(3.0f, 3.5f, 4.5f);

    public float stripeThickness = 1.0f;
    private float stripeOffset = 0.001f;


	void Update () {


        if (!FoundAllComponents)
            UpdateReferences();

        if (!FoundAllComponents)
        {
            Debug.Log("Not all pieces were found. Won't resize");
            return;
        }
        


        FloorParent.localPosition = floorMesh.localPosition = Vector3.zero;
        floorMesh.localScale = new Vector3(panelSize.x, 1.0f, panelSize.z);

        floorStripe_L.localScale = floorStripe_R.localScale = new Vector3(stripeThickness, 1.0f, panelSize.z);
        floorStripe_L.localPosition = new Vector3(0.0f, stripeOffset, 0.0f);
        floorStripe_R.localPosition = new Vector3(-panelSize.x, stripeOffset, 0.0f);



        WallParent.localPosition = Vector3.back * panelSize.z;
        wallMesh.localPosition = Vector3.zero;
        wallMesh.localScale = new Vector3(panelSize.x, panelSize.y, 1.0f);



        CeilingParent.localPosition = Vector3.up * panelSize.y;
        ceilingMesh.localPosition = Vector3.zero;
        ceilingMesh.localScale = new Vector3(panelSize.x, 1.0f, panelSize.z);

        ceilingStripe_L.localScale = ceilingStripe_R.localScale = new Vector3(stripeThickness, 1.0f, panelSize.z);
        ceilingStripe_L.localPosition = new Vector3(0.0f, -stripeOffset, 0.0f);
        ceilingStripe_R.localPosition = new Vector3(-panelSize.x, -stripeOffset, 0.0f);
    }



    private bool UpdateReferences()
    {
        if (!FloorParent || !WallParent || !CeilingParent)
            return false;

        floorMesh = FloorParent.Find("Floor");
        floorStripe_L = FloorParent.Find("Stripe_L");
        floorStripe_R = FloorParent.Find("Stripe_R");


        wallMesh = WallParent.Find("Wall");


        ceilingMesh = CeilingParent.Find("Ceiling");
        ceilingStripe_L = CeilingParent.Find("Stripe_L");
        ceilingStripe_R = CeilingParent.Find("Stripe_R");


        return FoundAllComponents;
    }


}

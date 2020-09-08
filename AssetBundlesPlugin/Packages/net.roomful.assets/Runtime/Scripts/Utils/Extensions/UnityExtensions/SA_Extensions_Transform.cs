using UnityEngine;

public static class SA_Extensions_Transform
{
    public static void Reset(this Transform t) {
        t.localScale = Vector3.one;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
    }

    //--------------------------------------
    // Bounds
    //--------------------------------------

    public static Vector3 GetVertex(this Transform t, SA_VertexX x, SA_VertexY y, SA_VertexZ z) {
        return t.gameObject.GetVertex(x, y, z);
    }
}
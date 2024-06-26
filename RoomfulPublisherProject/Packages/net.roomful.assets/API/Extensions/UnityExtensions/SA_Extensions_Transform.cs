using UnityEngine;

public static class SA_Extensions_Transform
{
    //--------------------------------------
    // Bounds
    //--------------------------------------

    public static Vector3 GetVertex(this Transform t, SA_VertexX x, SA_VertexY y, SA_VertexZ z) {
        return t.gameObject.GetVertex(x, y, z);
    }
}
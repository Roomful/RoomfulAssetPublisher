using UnityEngine;

public static class SA_Extensions_GameObject
{
    //--------------------------------------
    // Bounds
    //--------------------------------------

    public static Bounds GetRendererBounds(this GameObject go) {
        return SA_Extensions_Bounds.CalculateBounds(go);
    }

    public static Vector3 GetVertex(this GameObject go, SA_VertexX x, SA_VertexY y, SA_VertexZ z) {
        var bounds = go.GetRendererBounds();
        return bounds.GetVertex(x, y, z);
    }
}
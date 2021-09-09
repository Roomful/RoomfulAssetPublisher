using UnityEngine;

namespace net.roomful
{
    public static class Vector3Extension
    {
        public static Vector3 TerminateYAxis(this Vector3 source) {
            return (new Vector3(source.x, 0f, source.z)).normalized;
        }
    }
}
using UnityEngine;

namespace net.roomful.api
{
    public static class Vector3Extension
    {
        /// <summary>
        /// Drops Vector3.y value to 0;
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Vector3 TerminateYAxis(this Vector3 source) {
            return (new Vector3(source.x, 0f, source.z)).normalized;
        }
    }
}

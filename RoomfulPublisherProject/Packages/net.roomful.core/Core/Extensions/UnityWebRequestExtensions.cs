using UnityEngine.Networking;

namespace net.roomful.api
{
    public static class UnityWebRequestExtensions
    {
        public static bool IsSuccess(this UnityWebRequest request) {
#if UNITY_2021_3_OR_NEWER
            return request.result == UnityWebRequest.Result.Success;
#else
            return !request.isNetworkError && !request.isHttpError;
#endif
        }

        public static bool IsFailed(this UnityWebRequest request) {
            return !request.IsSuccess();
        }
    }
}
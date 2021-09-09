using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;

namespace net.roomful.api
{
    public class AsyncRequestWrapper<TRequestId, TRequestResult>
    {
        private readonly Action<TRequestId, Action<TRequestResult>> m_requestImplementation = null;
        private readonly Dictionary<TRequestId, TRequestResult> m_resultsCache = new Dictionary<TRequestId, TRequestResult>();

        private bool m_shouldCacheResults = true;
        private readonly Dictionary<TRequestId, List<Action<TRequestResult>>> m_pendingRequests = new Dictionary<TRequestId, List<Action<TRequestResult>>>();

        public AsyncRequestWrapper(Action<TRequestId, Action<TRequestResult>> requestImplementation) {
            m_requestImplementation = requestImplementation;
        }

        public void DefineResultForRequest(TRequestId id, TRequestResult result) {
            m_resultsCache[id] = result;
        }

        public bool TryGetValueResult(TRequestId id, out TRequestResult result) {
            return m_resultsCache.TryGetValue(id, out result);
        }

        public void Request(TRequestId id, Action<TRequestResult> callback) {
            if (m_shouldCacheResults && m_resultsCache.ContainsKey(id)) {
                callback.Invoke(m_resultsCache[id]);
                return;
            }

            if (m_pendingRequests.ContainsKey(id)) {
                m_pendingRequests[id].Add(callback);
                return;
            }

            var callbacksList = ListPool<Action<TRequestResult>>.Get();
            callbacksList.Add(callback);
            m_pendingRequests.Add(id, callbacksList);

            m_requestImplementation.Invoke(id, result => {
                if (m_shouldCacheResults) {
                    m_resultsCache[id] = result;
                }

                var pendingCallbacks = m_pendingRequests[id];
                foreach (var pendingCallback in pendingCallbacks) {
                    pendingCallback.Invoke(result);
                }

                ListPool<Action<TRequestResult>>.Release(pendingCallbacks);
                m_pendingRequests.Remove(id);
            });
        }

        public void ClearCache() {
            m_resultsCache.Clear();
        }

        public void DisableCache() {
            m_shouldCacheResults = false;
        }
    }
}

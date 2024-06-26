using System;
using System.Collections.Generic;
using StansAssets.Foundation.Patterns;

namespace net.roomful.api
{
    public class AsyncRequestWrapper<TRequestId, TRequestResult>
    {
        readonly Action<TRequestId, Action<TRequestResult>> m_RequestImplementation = null;
        readonly Dictionary<TRequestId, TRequestResult> m_ResultsCache = new Dictionary<TRequestId, TRequestResult>();

        bool m_ShouldCacheResults = true;
        readonly Dictionary<TRequestId, List<Action<TRequestResult>>> m_PendingRequests = new Dictionary<TRequestId, List<Action<TRequestResult>>>();

        public AsyncRequestWrapper(Action<TRequestId, Action<TRequestResult>> requestImplementation) {
            m_RequestImplementation = requestImplementation;
        }

        public void DefineResultForRequest(TRequestId id, TRequestResult result) {
            m_ResultsCache[id] = result;
        }
        
        public bool TryGetValueResult(TRequestId id, out TRequestResult result) {
            return m_ResultsCache.TryGetValue(id, out result);
        }

        public void Request(TRequestId id, Action<TRequestResult> callback) {
            if (m_ShouldCacheResults && m_ResultsCache.ContainsKey(id)) {
                callback.Invoke(m_ResultsCache[id]);
                return;
            }

            if (m_PendingRequests.ContainsKey(id)) {
                m_PendingRequests[id].Add(callback);
                return;
            }

            var callbacksList = ListPool<Action<TRequestResult>>.Get();
            callbacksList.Add(callback);
            m_PendingRequests.Add(id, callbacksList);

            m_RequestImplementation.Invoke(id, result => {
                if (m_ShouldCacheResults) {
                    m_ResultsCache[id] = result;
                }

                var pendingCallbacks = m_PendingRequests[id];
                foreach (var pendingCallback in pendingCallbacks) {
                    pendingCallback.Invoke(result);
                }

                ListPool<Action<TRequestResult>>.Release(pendingCallbacks);
                m_PendingRequests.Remove(id);
            });
        }

        public void ClearCache() {
            m_ResultsCache.Clear();
        }
        
        public void ClearCache(TRequestId id)
        {
            m_ResultsCache.Remove(id);
        }

        public void SetData(TRequestId id, TRequestResult data)
        {
            m_ResultsCache[id] = data;
        }

        public void DisableCache() {
            m_ShouldCacheResults = false;
        }
    }
}

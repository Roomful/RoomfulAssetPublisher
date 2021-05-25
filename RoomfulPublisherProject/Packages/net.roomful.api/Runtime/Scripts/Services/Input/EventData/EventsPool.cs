using System;
using System.Collections.Generic;

namespace net.roomful.api
{
    public class EventsPool<T> where T : new()
    {
        public readonly struct PooledObject : IDisposable
        {
            readonly EventsPool<T> m_pool;

            /// <summary>
            /// Creates `IDisposable` wrapper around poolable object.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="pool"></param>
            public PooledObject(T value, EventsPool<T> pool)
            {
                EventData = value;
                m_pool = pool;
            }

            public T EventData { get; }

            void IDisposable.Dispose() => m_pool.Release(EventData);
        }


        private readonly Stack<T> m_stack = new Stack<T>();

        protected T Get() {
            return m_stack.Count > 0 ? m_stack.Pop() : new T();
        }

        private void Release(T item) {
            m_stack.Push(item);
        }
    }
}

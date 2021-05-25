using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api.sa
{

    public interface SA_iEvent<T> : SA_iSafeEvent<T>
    {
        /// <summary>
        /// Add listener to the SA_Event.
        /// </summary>
        /// <param name="listner"> Callback function. </param> 
        void AddListener(Action<T> listner);
    }

    public interface SA_iEvent<T, T1> : SA_iSafeEvent<T, T1>
    {
        /// <summary>
        /// Add listener to the SA_Event.
        /// </summary>
        /// <param name="listner"> Callback function. </param> 
        void AddListener(Action<T, T1> listner);
    }

    public interface SA_iEvent : SA_iSafeEvent
    {
        /// <summary>
        /// Add listener to the SA_Event.
        /// </summary>
        /// <param name="listner"> Callback function. </param> 
        void AddListener(Action listner);
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moon.Network.Web {

	public interface IRequest  {

        string Path { get;  }
        Method Method { get; }
        IDataReader DataReader { get; }
        Dictionary<string, string> Headers { get; }


        IRequestCallback CreateRequestCallbackObject();
        void Finish(IRequestCallback callback);
    }
}

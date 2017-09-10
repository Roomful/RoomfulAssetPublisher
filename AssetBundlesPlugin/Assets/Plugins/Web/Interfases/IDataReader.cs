using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Moon.Network.Web
{

    public interface IDataReader
    {

        void SetRequest(IRequest request);
        byte[] Data { get; }
        Dictionary<string, object> Body { get;  }
       
    }

}
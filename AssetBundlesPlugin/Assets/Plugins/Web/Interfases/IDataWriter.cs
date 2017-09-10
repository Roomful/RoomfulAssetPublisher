using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moon.Network.Web
{

    public interface IDataWriter {

        void SetResult(IRequestCallback result);
        void Parse(byte[] data);
    }
}
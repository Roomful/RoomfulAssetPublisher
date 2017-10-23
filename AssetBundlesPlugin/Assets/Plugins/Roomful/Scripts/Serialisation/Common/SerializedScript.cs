using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetBundles.Serialization
{

    public interface IRecreatableOnLoad 
    {
        GameObject gameObject { get; }
    }

}
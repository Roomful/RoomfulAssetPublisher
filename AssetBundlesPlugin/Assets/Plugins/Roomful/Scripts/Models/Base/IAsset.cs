using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{
    public interface IAsset
    {
        Template GetTemplate();


        GameObject gameObject { get; }
        Component Component { get; }
    }
}


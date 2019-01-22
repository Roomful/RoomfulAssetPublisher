using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetWizzard {

    void OnGUI(bool GUIState);

    bool HasAsset { get; }

}

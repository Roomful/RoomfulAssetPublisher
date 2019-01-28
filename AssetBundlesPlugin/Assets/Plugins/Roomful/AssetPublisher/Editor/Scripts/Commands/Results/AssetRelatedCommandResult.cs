using UnityEngine;
using System.Collections;
using RF.AssetWizzard;
using System;

public class AssetRelatedCommandResult<T> : BaseCommandResult where T: Template {

    public T Asset {get; private set;}
    public AssetRelatedCommandResult() : base(false) {
    }

    public AssetRelatedCommandResult(string assetData) : base(true) {
        try {
            Asset = (T)Activator.CreateInstance(typeof(T), assetData);
        } catch {
            Success = false;
        }
        
    }
}

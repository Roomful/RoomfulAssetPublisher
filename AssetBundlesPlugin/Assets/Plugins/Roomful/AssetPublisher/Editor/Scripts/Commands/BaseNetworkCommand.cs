using UnityEngine;
using System.Collections;
using RF.AssetWizzard.Commands;
using System;
using RF.AssetWizzard.Network.Request;

public abstract class BaseNetworkCommand<T> : AbstractCommand<T> where T : ICommandResult {
    protected override void ExecuteImpl() {
        var request = GetRequest();
        request.PackageCallbackError = ErrorHandler;
        request.PackageCallbackText = SuccessHandler;
        request.Send();
    }

    protected abstract void SuccessHandler(string response) ;

    protected abstract void ErrorHandler(long errorId);

    protected abstract BaseWebPackage GetRequest();
}

using UnityEngine;
using System.Collections;

public class BaseCommandResult : ICommandResult {
    public bool Success {get; protected set;}

    public BaseCommandResult(bool success) {
        Success = success;
    }
}

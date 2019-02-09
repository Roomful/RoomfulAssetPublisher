using UnityEngine;
using System.Collections;

namespace RF.AssetWizzard.Results {

    public class BaseCommandResult : ICommandResult {
        public bool Success { get; protected set; }

        public BaseCommandResult(bool success) {
            Success = success;
        }
    }
}

using UnityEngine;
using System.Collections;

public class SendForReviewResult : BaseCommandResult {
    public ReleaseStatus NewReleaseStatus {get; private set;}
    public string AssetId {get; private set;}

    public SendForReviewResult(): base(false) {

    }

    public SendForReviewResult(ReleaseStatus releaseStatus, string assetId) : base(true) {
        NewReleaseStatus = releaseStatus;
        AssetId = assetId;
    }
}

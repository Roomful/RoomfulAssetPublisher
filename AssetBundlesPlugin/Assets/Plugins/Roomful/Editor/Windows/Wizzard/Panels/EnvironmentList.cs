using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class EnvironmentList : AssetsList<EnvironmentTemplate>
    {

        public EnvironmentList(EditorWindow window) : base(window) { }


        protected override void DrawAssetInfo() {

        }


        protected override Network.Request.GetAssetsList CreateAssetsListRequests() {

            Network.Request.GetEnvironmentsList listRequest = null;


            switch(SeartchType) {
                case SeartchRequestType.ByName:
                    listRequest = new Network.Request.GetEnvironmentsList(ListSize, 5, SeartchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    List<string> separatedTags = new List<string>(SeartchPattern.Split(','));
                    listRequest = new RF.AssetWizzard.Network.Request.GetEnvironmentsList(ListSize, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new RF.AssetWizzard.Network.Request.GetEnvironmentsList(ListSize, 5, string.Empty);
                    listRequest.SetId(SeartchPattern);
                    break;

            }

            return listRequest;
        }

    }
}
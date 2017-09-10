using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Moon.Network.Web;

namespace RF.AssetWizzard.Network 
{

  

	public class AssetsList : BaseRoomfulRequest<AssetsListsCallback> {

        [Param]
        private string Id;
        [Param]
        private string Title;
        [Param]
        private List<string> Tags;
        [Param]
        private int Offset = 0;
        [Param]
        public int Size = 10;

        public const string PATH = "/api/v0/asset/list";


        public AssetsList(int offset, int size, List<string> tags):base(PATH) {
            Offset = offset;
            Size = size;

            Tags = new List<string>();
            Tags.AddRange(tags);
        }

        public AssetsList(int offset, int size, string title):base(PATH) {
            Offset = offset;
            Size = size;
            Title = title;
        }

        public void SetId(string id) {
            Id = id;
        }

	}
}

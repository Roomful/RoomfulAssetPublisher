using System.Collections.Generic;

namespace net.roomful.assets
{
    public class PropSkinComparer : IComparer<IPropSkin>
    {
        public int Compare(IPropSkin a, IPropSkin b) {
            var p1 = a.SortOrder;
            var p2 = b.SortOrder;

            if (p2 > p1) {
                return 1;
            }

            if (p2 < p1) {
                return -1;
            }

            return 0;
        }
    }
}

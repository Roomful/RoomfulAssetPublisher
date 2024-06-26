using System.Collections.Generic;

namespace net.roomful.assets
{
    public class PropVariantComparer : IComparer<IPropVariant>
    {
        public int Compare(IPropVariant a, IPropVariant b) {
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

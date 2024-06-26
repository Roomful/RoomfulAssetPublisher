using System.Collections.Generic;

namespace net.roomful.assets
{
    class PriorityComparer : IComparer<IPropPublihserComponent>
    {
        public int Compare(IPropPublihserComponent a, IPropPublihserComponent b) {
            var p1 = (int) a.UpdatePriority;
            var p2 = (int) b.UpdatePriority;

            if (p1 > p2) {
                return 1;
            }
            else {
                if (p1 < p2) {
                    return -1;
                }
                else {
                    return 0;
                }
            }
        }
    }
}
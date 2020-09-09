using System.Collections.Generic;

namespace net.roomful.assets
{
    internal class PriorityComparer : IComparer<IPropComponent>
    {
        public int Compare(IPropComponent a, IPropComponent b) {
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RF.AssetWizzard
{

    public class PriorityComparer : IComparer<IPropComponent>
    {

        public int Compare(IPropComponent a, IPropComponent b) {

            int p1 = (int) a.UpdatePriority;
            int p2 = (int) b.UpdatePriority;

           if(p1 > p2) {
                return 1;
           } else {
                if(p1 < p2) {
                    return -1;
                } else {
                    return 0;
                }
            }
        }
    }
}
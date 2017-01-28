using UnityEngine;
using System.Collections;
using TrueSync;


public abstract class LineWarStatusEffect : StatusEffect {

    public void SetLockOnLineIntersection()
    {
        TSCollider tsCollider = GetComponent<TSCollider>();
        bool beforeUnlockedFlag = tsCollider.enabled;
        bool isUnlockedFlag = true;
        FP radius = GetComponent<Renderer>().bounds.extents.magnitude;
        FP distanceToWorldCenter = tsTransform.position.magnitude;

        foreach (TSRigidBody rb in Line.LineList)
        {
            // Where is the nearest point on line?
            TSVector nearestPointOnLine = rb.tsTransform.forward * distanceToWorldCenter;
            if (TSVector.Distance(nearestPointOnLine, tsTransform.position) < radius)
            {
                isUnlockedFlag = false;
                break;
            }
        }

        if (beforeUnlockedFlag != isUnlockedFlag) // Check for state change
        {
            if (isUnlockedFlag) // State has been unlocked (now accessible for activation)
            {
                tsCollider.enabled = true;
                OnUnlockForActivation();
            }
            else // State has been locked (now accessible for activation)
            {
                tsCollider.enabled = false;
                OnLockedForActivation();
            }
        }
    }

}

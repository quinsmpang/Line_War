using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;


public class Line : StatusEffectActor {

    [SerializeField]
    private FP _currentVelocity;

    private TSRigidBody rbody;

    public override void OnSyncedStart()
    {
        rbody = GetComponent<TSRigidBody>();
    }

    public override void OnSyncedUpdate()
    {
        _currentVelocity = rbody.angularVelocity.magnitude;
    }

    // FIXME: Check to see if this method is used, remove if not
    public void OnSyncedCollisionEnter(TSCollision other)
    {
        Debug.LogWarning("Collided with: "+other.gameObject.name);
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;


public class Line : StatusEffectActor {

    public static List<TSRigidBody> LineList = new List<TSRigidBody>();


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


}

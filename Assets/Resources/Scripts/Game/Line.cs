using UnityEngine;
using System.Collections;
using TrueSync;


public class Line : TrueSyncBehaviour {

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

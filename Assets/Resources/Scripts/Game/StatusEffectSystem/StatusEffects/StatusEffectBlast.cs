using UnityEngine;
using System.Collections;
using System;
using TrueSync;


public class StatusEffectBlast : LineWarStatusEffect {

    public FP blastEffectForce; // amount of force to apply upon activating this effect
    public FP blastPenaltyForce; // amount of force to apply if activated too close to a line
    public bool usesPenalty;

    private TSPlayerInfo activationPlayer;  // player who triggered the status effect


    public void Awake()
    {
        OnUpdate = SetLockOnLineIntersection;
    }


    public override void OnSpawn()
    {
        Debug.LogWarning("Status Effect Blast Spawned");
    }

    public override void OnDespawn()
    {
        Debug.LogWarning("Status Effect Blast Despawned");
        // FIXME: Rewrite this with a pooling system, to recycle gameObjects.
        // Write a system for generic objects for use with other types.
        StatusEffectSystem.spawnedStatusEffects.Remove(this);
        TrueSyncManager.SyncedDestroy(gameObject);
    }

    public override void OnActivation()
    {
        // FIXME: code this with respect to the initiating actor i.e. whoever activated this effect
        //Debug.LogWarning("Status Effect Blast, OnActivation");
        Debug.LogWarning("OnActivation/owner: " + activationPlayer.Name);
        TSTransform tst = GetComponent<TSTransform>();
        TSRigidBody lineRB = Player.GetClosestLine(tst.position);

        if (lineRB.angularVelocity.magnitude > 0) // exit this method if line is moving, can't hit lines while they move
            return;
        FP distanceToLine = Player.GetDistanceToLineFromPosition(lineRB, tst.position);
        FP direction = TSVector.Cross(tst.position.normalized, lineRB.tsTransform.forward).normalized.y;
        FP tapEffectRadius = PlayerConfig.Instance.TapEffectRadius;
        FP minTapDistance = PlayerConfig.Instance.MinTapDistance;
        FP effect = FP.One;
        
        if (usesPenalty && distanceToLine < minTapDistance) // apply penalty, tap was too close to the line
        {
            Player.ApplyExplosiveForceOnLine(lineRB, -1 * direction * PlayerConfig.Instance.PenaltyEffectForce);
        }
        else if (!usesPenalty || tapEffectRadius >= distanceToLine) // within range, but not on line.  Apply normal force
        {
            effect = 1 - (distanceToLine - minTapDistance) / tapEffectRadius;
            Player.ApplyExplosiveForceOnLine(lineRB, direction * PlayerConfig.Instance.TapEffectForce * effect);
        }
           
    }

    public override void OnDeactivation()
    {
        // FIXME: code this with respect to the initiating actor
        Debug.LogWarning("Status Effect Blast, OnDeactivation");
    }

    public override void OnUnlockForActivation()
    {
        // This Status Effect has been enabled for activation, switch graphics now
        Debug.LogWarning("OnUnlockForActivation, time to switch graphics now");
    }

    public override void OnLockedForActivation()
    {
        // This Status Effect has been disabled for activation, switch graphics now
        Debug.LogWarning("OnLockedForActivation, time to switch graphics now");
    }


    public override void OnSyncedTriggerEnter(TSCollision other)
    {
        TapLocationSphereCheck sphereCheck = other.gameObject.GetComponent<TapLocationSphereCheck>();

        if (sphereCheck != null)
        {
            activationPlayer = sphereCheck.Owner;
            OnActivation(); // Start all status effects here
            activatedEndtime = activatedLifetime + TrueSyncManager.Time;

            // Conclude the instant status effects now
            if (activationLifeSpan == ActivatedTimeFrame.Instant)
            {
                OnDeactivation();
                OnDespawn();
            }
        }   
    }

}

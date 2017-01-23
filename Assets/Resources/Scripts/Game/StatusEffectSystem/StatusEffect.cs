using UnityEngine;
using System.Collections;
using TrueSync;
using System;


public abstract class StatusEffect : TrueSyncBehaviour {

    /// <summary>
    /// Name of this Status Effect
    /// </summary>
    public string Name;

    /// <summary>
    /// Time frame this status effect functions with, determines when OnBegin and OnEnd actions are called
    /// Instant - OnBegin and OnEnd methods are called instantly upon first update 
    /// Duration - OnBegin is called instantly upon first update, while OnEnd is called at end of lifetime
    /// Continuous - OnBegin is called instantly upon first update.  OnEnd is called when this StatusEffect
    /// is canceled/terminated.  Continuous will resume this status effect forever until it is canceled.
    /// </summary>
    public enum ActivatedTimeFrame { Instant, Duration, Continuous }
    public ActivatedTimeFrame activationLifeSpan;


    /// <summary>
    /// Selector to determine if this status effect will be available for pickup forever (Eternal) or
    /// for a timed duration (Timed)
    /// </summary>
    public enum ExistanceTimeFrame { Timed, Eternal }
    public ExistanceTimeFrame existanceTimeFrame;

    /// <summary>
    /// Target this status effect applies to when activated
    /// Self - Local player (player who activates/touches this status effect)
    /// Everyone - All players in game
    /// Others - All players except the local player
    /// </summary>
    public enum Target { Self, Everyone, Others } // FIXME: add ability to target a specific player
    public Target target;

    /// <summary>
    /// Time in seconds of which this effect remains activated once activated
    /// </summary>
    [Tooltip("Number of seconds in which this status effect will remain activated once triggered")]
    public FP activatedLifetime;

    /// <summary>
    /// Time at which this effect deactivates/ends (calculated upon application of this effect upon its target(s)
    /// </summary>
    [HideInInspector]
    public FP activatedEndtime; // FIXME: this needs to be calculated upon activation of this effect

    /// <summary>
    /// Time in seconds of which this effect exists in the game world upon initially spawning
    /// </summary>
    [Tooltip("Number of seconds in which this status effect will remain within the game world before disappearing")]
    public FP spawnLifetime;

    /// <summary>
    /// Time at which this effect despawns/disappears from the game world
    /// </summary>
    [HideInInspector]
    public FP spawnEndTime; // FIXME: this needs to be calculated right after instantiation of this status effect/pickup

    [TextArea(3,10)]
    public string Description;


    /// THE FOLLOWING EVENT HANDLERS MUST BE ASSIGNED WHEN THE PLAYER ACTIVATES THE STATUS EFFECT
    /// <summary>
    /// Method that is called when this status effect is activated (applied status effect to target)
    /// </summary>
    public abstract void OnActivation();

    /// <summary>
    /// Method that is called when this status effect is terminated (removed status effect from target)
    /// </summary>
    public abstract void OnDeactivation();

    /// <summary>
    /// Method that is called when this status effect is brought into existance (created/appears in game world)
    /// </summary>
    public abstract void OnSpawn();

    /// <summary>
    /// Method that is called when this status effect is removed from existance (canceled/removed from game world)
    /// </summary>
    public abstract void OnDespawn();

    /// <summary>
    /// Method that is called when this status effect is touched by another trigger/collider
    /// </summary>
    /// <param name="other"></param>
    public abstract void OnSyncedTriggerEnter(TSCollision other);


    /// <summary>
    /// Method that is called upon instantiation of this status effect
    /// </summary>
    public override void OnSyncedStart()
    {
        spawnEndTime = TrueSyncManager.Time + spawnLifetime;
    }


    


    

}

using UnityEngine;
using System;
using System.Collections;
using TrueSync;
using System.Collections.Generic;


public class StatusEffectSystem : TrueSyncBehaviour {

    [Tooltip("Status Effect Prefabs that will be made available within the Status Effect System")]
    public List<GameObject> statusEffectPrefabs;

    public static StatusEffectSystem instance;

    public static List<StatusEffectActor> allStatusEffectActors = new List<StatusEffectActor>();
    public static List<StatusEffect> spawnedStatusEffects = new List<StatusEffect>();

    public FP spawnTimeInterval = 5;

    public int maxDistributionRadius;
    public int minDistributionRadius;


    public void Awake()
    {
        instance = this;
    }


    /// <summary>
    /// Plan
    /// 
    /// Get distribution working
    /// Get response from taps working
    /// 
    /// </summary>
    public override void OnSyncedStart()
    {
        TrueSyncManager.SyncedStartCoroutine(DispensePowerup());
    }


    IEnumerator DispensePowerup()
    {
        while (true)
        {
            yield return spawnTimeInterval;

            if (statusEffectPrefabs.Count < 1)
                Debug.LogError("Status Effect Prefabs List is empty, there must be at least 1 prefab reference in the list.");

            GameObject go = TrueSyncManager.SyncedInstantiate(statusEffectPrefabs[0], getRandomPosition(), TSQuaternion.identity);
            StatusEffect se = go.GetComponent<StatusEffect>();
            spawnedStatusEffects.Add(se);
            se.OnSpawn();
        }
    }


    /// <summary>
    /// Generates a random position using min and max distribution radius
    /// </summary>
    /// <returns>new random position (TSVector)</returns>
    /// FIXME:  This method will occasionally generate positions which existing powerups may be already occupying
    private TSVector getRandomPosition()
    {
        // Place the powerup in a random location
        int direction = TSRandom.Range(0, 359);

        // Generate general position TSVector
        TSVector position = TSQuaternion.AngleAxis(direction, TSVector.up).Rotate(TSVector.forward);

        // Apply distance between min and max distribution radius
        return position.normalized * TSRandom.Range(minDistributionRadius, maxDistributionRadius);
    }


    public override void OnSyncedUpdate()
    {
        // Loop through all running status effects and update their states (check TimeFrame and lifetime)
        foreach (StatusEffectActor actor in allStatusEffectActors)
        {
            actor.UpdateEffects();
        }

        // Loop through all unactivated spawned status effects
        // FIXME: need to incorporate some plan for this
        /* PLAN:
            1. -add enabled and disabled states to status effect
            2. check each status effect against the list of lines to determine if status effect should be enabled or disabled
            3. used the enabled/disabled state to determine how input interaction is handled (to determine if the status effect is responsive)
         */
        foreach (StatusEffect se in spawnedStatusEffects)
        {
            se.OnUpdate();
        }
        
    }


    /// <summary>
    /// Enabled the status effect with the given name on the specified list of actors,
    /// this assigns the status effect to the actors and begins the status effect
    /// </summary>
    /// <param name="actors"></param>
    /// <param name="statusEffectName"></param>
    public void EnableStatusEffect(List<StatusEffectActor> actors, string statusEffectName)
    {
        foreach (StatusEffectActor actor in actors)
        {
            if (!allStatusEffectActors.Contains(actor))
            {
                allStatusEffectActors.Add(actor);
            }

            actor.ActivateStatusEffect(statusEffectName);
        }
    }


    /// <summary>
    /// Disables the status effect with the given name on the specified list of actors
    /// NOTE: This must be called for Continuous TimeFrame Effects
    /// </summary>
    /// <param name="actors"></param>
    /// <param name="statusEffectName"></param>
    public void DisableStatusEffect(List<StatusEffectActor> actors, string statusEffectName)
    {
        foreach (StatusEffectActor actor in actors)
        {
            actor.DeactivateStatusEffect(statusEffectName);
        }
    }


}


public static class LineWarExtensionMethods
{
    /// <summary>
    /// Rotates the given TSVector by the TSQuaternion
    /// </summary>
    /// <param name="quat"></param>
    /// <param name="vec"></param>
    /// <returns></returns>
    public static TSVector Rotate(this TSQuaternion quat, TSVector vec)
    {
        FP num = quat.x * 2f;
        FP num2 = quat.y * 2f;
        FP num3 = quat.z * 2f;
        FP num4 = quat.x * num;
        FP num5 = quat.y * num2;
        FP num6 = quat.z * num3;
        FP num7 = quat.x * num2;
        FP num8 = quat.x * num3;
        FP num9 = quat.y * num3;
        FP num10 = quat.w * num;
        FP num11 = quat.w * num2;
        FP num12 = quat.w * num3;
        TSVector result;
        result.x = (1f - (num5 + num6)) * vec.x + (num7 - num12) * vec.y + (num8 + num11) * vec.z;
        result.y = (num7 + num12) * vec.x + (1f - (num4 + num6)) * vec.y + (num9 - num10) * vec.z;
        result.z = (num8 - num11) * vec.x + (num9 + num10) * vec.y + (1f - (num4 + num5)) * vec.z;
        return result;
    }

}

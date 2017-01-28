using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TrueSync;


/// <summary>
/// Extend this class for all actors to have effects upon (effects are like powerups/debuffs)
/// </summary>
public abstract class StatusEffectActor : TrueSyncBehaviour {

    public Dictionary<string, StatusEffect> runningStatusEffectsDict;


    /// <summary>
    /// Checks for expired running status effects and removes them
    /// </summary>
    public void UpdateEffects()
    {
        // FYI: This is only for Duration TimeFrame Status Effects, Instant and Continuous effects are handled separately.
        // Instant is processed instantly while Continuous is processed on demand though other means.
        // Instant will not be in the runningStatusEffectsDict, but Continuous will.
        List<string> terminatedStatusEffects = null;

        // Check for terminated status effects, their lifetimes have expired
        foreach (KeyValuePair<string, StatusEffect> pair in runningStatusEffectsDict)
        {
            StatusEffect se = pair.Value;
            if (se.activationLifeSpan == StatusEffect.ActivatedTimeFrame.Duration) // handle only Duration timeFrame effects
            {
                if (TrueSyncManager.Time >= se.activatedEndtime)
                {
                    // Call OnEnd and remove this effect from this actor
                    se.OnDeactivation();
                    if (terminatedStatusEffects == null)
                        terminatedStatusEffects = new List<string>();

                    terminatedStatusEffects.Add(se.Name);
                }
            }
        }

        // Remove terminated status effects
        foreach (string statusEffectName in terminatedStatusEffects)
        {
            Debug.LogWarning("Removing status effect: "+statusEffectName);
            runningStatusEffectsDict.Remove(statusEffectName);
        }
    }


    /// <summary>
    /// Begins the status effect assigned to this actor with the specified status effect name
    /// NOTE: Status effect must be assigned to this actor before calling this method
    /// </summary>
    /// <param name="statusEffectName"></param>
    public void ActivateStatusEffect(string statusEffectName)
    {
        runningStatusEffectsDict[statusEffectName].OnActivation();
    }

    /// <summary>
    /// Terminates the running status effect with the specified name
    /// </summary>
    /// <param name="statusEffectName"></param>
    public void DeactivateStatusEffect(string statusEffectName)
    {
        runningStatusEffectsDict[statusEffectName].OnDeactivation();
        runningStatusEffectsDict.Remove(statusEffectName);
    }

    /// <summary>
    /// Assigns the given status effect with the specified status effect name to this actor.
    /// </summary>
    /// <param name="statusEffectName"></param>
    /// <param name="statusEffect"></param>
    public void AssignStatusEffect(string statusEffectName, StatusEffect statusEffect)
    {
        runningStatusEffectsDict.Add(statusEffectName, statusEffect);
    }

}

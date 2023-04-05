using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect
{
    public float TimeSinceEffectApply { get; private set; }
    public float TimeCooldownEffect { get; private set; }
    public float EffectForce { get; private set; }
    /// <summary>
    /// Checks if enough time has passed for some effect with a cooldown time
    /// </summary>
    public static Func<EntityEffect, bool> HasEnoughTimeHasPassed { get; } = entityEffect => Time.time >= entityEffect.TimeSinceEffectApply + entityEffect.TimeCooldownEffect;

    public EntityEffect(float timeCooldownEffect, float effectForce)
    {
        TimeSinceEffectApply = 0f;
        TimeCooldownEffect = timeCooldownEffect;
        EffectForce = effectForce;
    }
    public void SetNewTimeEffectApply()
    {
        TimeSinceEffectApply = Time.time;
    }
}

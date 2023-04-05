using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEffect
{
    public float TimeSinceEffectApply;
    public float TimeCooldownEffect;
    public float EffectForce;
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
}

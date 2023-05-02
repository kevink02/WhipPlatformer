using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    public override void TakeDamage()
    {
        // No need to have variable stored to handle invincibility frames
        // Enemies flipping movement direction and player knockback should account for time between hits
        Health--;
        if (Health <= 0)
        {
            OnDeath();
        }
        else
        {
            StartCoroutine(PlayDamageAnimation());
        }
    }
    public override void OnDeath()
    {
        ComponentMovement.SetPositionToSpawnPoint();
        Health = MaxHealth;
    }
}

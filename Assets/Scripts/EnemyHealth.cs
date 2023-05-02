using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    private float _timeSinceLastHit;

    public override void TakeDamage()
    {
        // If attacked recently, should skip taking damage until player's attack collider disables
        // Prevents enemy from taking damage more than once during a player's attack (especially if player turns while attacking)
        if (Time.time <= _timeSinceLastHit + PlayerAttack.AttackColliderDuration)
        {
            return;
        }
        _timeSinceLastHit = Time.time;
        StartCoroutine(PlayDamageAnimation());
        Health--;
        if (Health <= 0)
        {
            OnDeath();
        }
    }
    public override void OnDeath()
    {
        Destroy(HealthText.gameObject);
        Destroy(gameObject);
    }
}

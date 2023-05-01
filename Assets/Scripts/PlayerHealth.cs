using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : EntityHealth
{
    private PlayerMovement _playerMovement;

    private new void Awake()
    {
        base.Awake();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    public override void TakeDamage()
    {
        // No need to have variable stored to handle invincibility frames
        // Enemies flipping movement direction and player knockback should account for time between hits
        StartCoroutine(PlayDamageAnimation());
        Health--;
        if (Health <= 0)
        {
            Debug.Log($"{name}: I is dead");
            _playerMovement.SetPositionToSpawnPoint();
            Health = MaxHealth;
        }
    }
}

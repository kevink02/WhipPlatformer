using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : EntityHealth
{
    public override void TakeDamage()
    {
        Health--;
        if (Health <= 0)
            Destroy(gameObject);
    }
}

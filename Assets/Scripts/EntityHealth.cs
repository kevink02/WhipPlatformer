using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IVerification
{
    [SerializeField]
    protected int MaxHealth;
    protected int Health;

    private void Awake()
    {
        VerifyVariables();
    }
    public void VerifyVariables()
    {
        if (MaxHealth <= 0)
        {
            MaxHealth = 3;
        }
        Health = MaxHealth;
    }
    public abstract void TakeDamage();
}

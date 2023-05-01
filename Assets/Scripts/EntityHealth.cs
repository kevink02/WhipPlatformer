using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityHealth : MonoBehaviour, IVerification
{
    [SerializeField]
    protected int Health;

    private void Awake()
    {
        VerifyVariables();
    }
    public void VerifyVariables()
    {
        if (Health <= 0)
        {
            Health = 3;
        }
    }
    protected abstract void TakeDamage();
}

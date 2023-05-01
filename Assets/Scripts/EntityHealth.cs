﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityHealth : MonoBehaviour, IVerification
{
    [SerializeField]
    protected int MaxHealth;
    protected int Health;
    protected SpriteRenderer ComponentSprite;
    protected Text HealthText;

    private void Awake()
    {
        ComponentSprite = GetComponent<SpriteRenderer>();

        VerifyVariables();
    }
    protected void FixedUpdate()
    {
        if (!HealthText)
            return;

        HealthText.text = $"{Health}/{MaxHealth}";
        HealthText.transform.position = transform.position + 2 * Vector3.up;
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
    // "Animation" of enemy taking damage
    protected IEnumerator PlayDamageAnimation()
    {
        float time = PlayerAttack.AttackColliderDuration / 5;
        for (int i = 0; i < 3; i++)
        {
            // Flash red, then revert colors back to normal
            ComponentSprite.color = new Color(1, 0, 0, 1);
            yield return new WaitForSeconds(time);
            ComponentSprite.color = new Color(1, 1, 1, 1);
            if (i != 2)
                yield return new WaitForSeconds(time);
        }
    }
}

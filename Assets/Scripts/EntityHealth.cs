using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityHealth : MonoBehaviour, IVerification
{
    public int MaxHealth { get; private set; }
    [HideInInspector]
    public int Health { get; protected set; }
    protected SpriteRenderer ComponentSprite;
    protected Text HealthText;

    protected void Awake()
    {
        ComponentSprite = GetComponent<SpriteRenderer>();

        VerifyVariables();
    }
    protected void FixedUpdate()
    {
        HealthText.text = $"{Health}/{MaxHealth}";
        HealthText.transform.position = transform.position + 5 * Vector3.up;
    }
    public void VerifyVariables()
    {
        if (MaxHealth <= 0)
        {
            MaxHealth = 3;
        }
        Health = MaxHealth;

        if (!HealthText)
        {
            HealthText = Game_Manager.GetSingleton().CreateAndGetHealthText();
        }
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

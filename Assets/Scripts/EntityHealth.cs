using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EntityHealth : MonoBehaviour, IVerification
{
    protected EntityMovement ComponentMovement;
    public int MaxHealth;
    [HideInInspector]
    public int Health;
    protected SpriteRenderer ComponentSprite;
    protected Text HealthText;

    protected void Awake()
    {
        ComponentMovement = GetComponent<EntityMovement>();
        ComponentSprite = GetComponent<SpriteRenderer>();

        VerifyVariables();
    }
    protected void FixedUpdate()
    {
        HealthText.text = $"{Health}/{MaxHealth}";
        // Allow larger enemies to have their health text be higher above them compared to smaller enemies
        HealthText.transform.position = transform.position + 7 * transform.localScale.y * Vector3.up;
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
    public abstract void OnDeath();
    // "Animation" of enemy taking damage
    protected IEnumerator PlayDamageAnimation()
    {
        HealthText.gameObject.SetActive(true);
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
        HealthText.gameObject.SetActive(false);
    }
}

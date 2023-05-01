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
        Health--;
        if (Health <= 0)
            Destroy(gameObject);
        else
            ShowEnemyHealthText();
    }
    private void ShowEnemyHealthText()
    {
        Game_Manager.EnemyHealthText.enabled = true;
        Game_Manager.EnemyHealthText.transform.position = transform.position + 2 * Vector3.up;
        Game_Manager.EnemyHealthText.text = $"{Health}/{MaxHealth}";
        StartCoroutine(HideEnemyHealthText());
    }
    private IEnumerator HideEnemyHealthText()
    {
        yield return new WaitForSeconds(1.5f);
        Game_Manager.EnemyHealthText.enabled = false;
    }
}

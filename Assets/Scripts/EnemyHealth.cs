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
        else
            ShowEnemyHealthText();
    }
    private void ShowEnemyHealthText()
    {
        Game_Manager.EnemyHealthText.enabled = true;
        Game_Manager.EnemyHealthText.transform.position = transform.position + 2 * Vector3.up;
        Game_Manager.EnemyHealthText.text = $"{Health}/{3}";
        StartCoroutine(HideEnemyHealthText());
    }
    private IEnumerator HideEnemyHealthText()
    {
        yield return new WaitForSeconds(1.5f);
        Game_Manager.EnemyHealthText.enabled = false;
    }
}

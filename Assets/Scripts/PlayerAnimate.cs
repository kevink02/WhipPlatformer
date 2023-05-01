using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimate : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void AbilityAttack()
    {
        Debug.Log($"{name}: Attacking");
        // Prevent animation glitches by spamming attack triggers
        if (_animator.GetBool("IsAttacking"))
        {
            return;
        }
        // Play the attack animation only once after triggering an attack
        _animator.SetBool("IsAttacking", true);
        StartCoroutine(EndAnimationAttack());
    }
    private IEnumerator EndAnimationAttack()
    {
        // This value should equal the speed of the attack animation in the animator window (not the length of the clip because it is NOT accurate)
        yield return new WaitForSeconds(0.67f);
        _animator.SetBool("IsAttacking", false);
    }
}

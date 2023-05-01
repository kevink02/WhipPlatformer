using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("This value should equal the speed of the attack animation in the animator window (not the length of the clip because it is NOT accurate)")]
    public const float AnimationAttackDuration = 0.67f;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    public void AbilityAttack()
    {
        _animator.enabled = true;
        StartCoroutine(EndAnimationAttack());
    }
    private IEnumerator EndAnimationAttack()
    {
        yield return new WaitForSeconds(AnimationAttackDuration);
        _animator.enabled = false;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"{name}: Collided with {collision.name}");
    }
}

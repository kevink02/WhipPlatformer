using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Tooltip("This value should equal the speed of the attack animation in the animator window (not the length of the clip because it is NOT accurate)")]
    public const float AnimationAttackDuration = 0.67f;
    private Animator _animator;
    private Collider2D _collider;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }
    public void AbilityAttack()
    {
        _animator.enabled = true;
        StartCoroutine(EnableWhipCollider());
        StartCoroutine(EndAnimationAttack());
    }
    // Only enable whip hitbox corresponding to certain time period/frames within the attack animation
    // Currently, only have the collider enabled when the whip is out in front of the player
    private IEnumerator EnableWhipCollider()
    {
        yield return new WaitForSeconds(0.2f);
        _collider.enabled = true;
        yield return new WaitForSeconds(0.2f);
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

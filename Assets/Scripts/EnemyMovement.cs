using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    private void FixedUpdate()
    {
        Vector2 direction = Vector2.zero;
        Debug.DrawRay(transform.position, direction, Color.white, 0.02f);
    }
}

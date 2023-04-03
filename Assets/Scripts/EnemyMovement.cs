using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Range(0, 360f)]
    [SerializeField]
    private float _detectAngle; // default = 315
    [SerializeField]
    private float _moveSpeed;

    private void FixedUpdate()
    {
        Vector2 direction = Game_Manager.GetVector2FromAngle(_detectAngle);
        Debug.DrawRay(transform.position, direction, Color.white, Game_Manager.DebugRayLifeTime);
    }
}

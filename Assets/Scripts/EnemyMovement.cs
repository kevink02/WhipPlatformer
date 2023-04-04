using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Range(0, 360f)]
    [SerializeField]
    private float _detectAngle; // default = 315
    [Range(0.01f, 1)]
    [SerializeField]
    private float _moveAccel; // default = 0.2f
    [Range(0.01f, 1)]
    [SerializeField]
    private float _moveDecel; // default = 0.1f
    [SerializeField]
    private float _moveForce; // default = 5

    private Rigidbody2D _rigidBody2D;
    private Vector2 _detectVector; // the vector corresponding to the detect angle
    private Vector2 _moveDirection;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _detectVector = Game_Manager.GetVector2FromAngle(_detectAngle);
        _moveDirection = Vector2.right;
    }
    private void FixedUpdate()
    {
        _rigidBody2D.velocity = Vector2.Lerp(_rigidBody2D.velocity, _moveForce * _moveDirection, _moveAccel);


        Debug.DrawRay(transform.position, _detectVector, Color.white, Game_Manager.DebugRayLifeTime);

        // Move in current direction
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        Ray ray = new Ray(transform.position, _detectVector);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, Game_Manager.PlatformMask);
        // Did not detect a platform in front of it
        if (!(hit && hit.collider.CompareTag("Platform")))
        {
            FlipMoveDirection();
        }
    }
    private void FlipMoveDirection()
    {
        _detectVector = Vector2.Reflect(_detectVector, Vector2.down);
        _detectAngle = Game_Manager.GetAngleFromVector2(_detectVector);
        _moveDirection *= -1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [Range(0, 360f)]
    [SerializeField]
    private float _detectAngle; // default = 315

    private Vector2 _detectVector; // the vector corresponding to the detect angle
    private Vector2 _moveDirection;

    private new void Awake()
    {
        base.Awake();
        _detectVector = Game_Manager.GetVector2FromAngle(_detectAngle);
        _moveDirection = Vector2.right;
    }
    private void FixedUpdate()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * _moveDirection, MoveAccel);


        Debug.DrawRay(transform.position, _detectVector, Color.white, Game_Manager.DebugRayLifeTime);

        // Move in current direction
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        Ray ray = new Ray(transform.position, _detectVector);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, Game_Manager.PlatformMask);
        // Did not detect a platform in front of it
        if (!hit || !Game_Manager.IsObjectAPlatform(hit.collider.gameObject))
        {
            FlipMoveDirection();
        }
    }
    private void FlipMoveDirection()
    {
        _detectVector = Vector2.Reflect(_detectVector, Vector2.right);
        _detectAngle = Game_Manager.GetAngleFromVector2(_detectVector);
        _moveDirection *= -1;
    }
}

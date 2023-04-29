using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : EnemyMovement
{
    private new void Awake()
    {
        base.Awake();
        MoveDirection = Vector2.right;
        RigidBody.gravityScale = 1;
    }
    protected override void DoMovementPatrol()
    {
        throw new System.NotImplementedException();
    }
    protected override void DoMovementTimed()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        // Did not detect a platform in front of it
        if (!HasCollidedWithPlatformAtDetectAngle())
            FlipMoveDirection(Vector2.right);
    }
}

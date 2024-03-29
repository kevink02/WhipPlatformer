﻿using System.Collections;
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
        if (!PatrolPointTarget)
        {
            VerifyVariables();
        }
        //float distanceToTargetTransform = Mathf.Abs(PatrolPointTarget.position.x - transform.position.x);
        RigidBody.velocity = MoveForce * MoveDirection;
        if (IsCloseToPatrolPointTarget())
        {
            MoveDirection *= -1;
            SwapPatrolPointTarget();
        }
    }
    protected override void DoMovementTimed()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        if (EntityEffect.HasEnoughTimePassed(EffectMoveFlip))
        {
            EffectMoveFlip.SetNewTimeEffectApply();
            FlipMoveDirection(Vector2.right);
        }

        // Did not detect a platform in front of it
        //if (!HasCollidedWithPlatformAtDetectAngle())
        //    FlipMoveDirection(Vector2.right);
    }
    protected override bool IsCloseToPatrolPointTarget()
    {
        return Mathf.Abs(transform.position.x - PatrolPointTarget.position.x) <= PatrolPointDistance &&
            Mathf.Abs(transform.position.x - PatrolPointCurrent.position.x) > PatrolPointDistance;
    }
    protected override void FlipMoveDirectionOnCollision()
    {
        FlipMoveDirection(Vector2.right);
    }
}

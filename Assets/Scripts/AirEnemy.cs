﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirEnemy : EnemyMovement
{
    [SerializeField]
    private EnemyMoveDirection _enemyMoveDirection;
    private enum EnemyMoveDirection : int
    {
        Vertical, Horizontal
    }

    private new void Awake()
    {
        base.Awake();
        switch (_enemyMoveDirection)
        {
            // Set physics values based on the enemy type
            case EnemyMoveDirection.Vertical:
                MoveDirection = Vector2.up;
                RigidBody.gravityScale = 0;
                // Only allow changes to y position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
                RigidBody.freezeRotation = true;
                break;
            case EnemyMoveDirection.Horizontal:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 0;
                // Only allow changes to x position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
                RigidBody.freezeRotation = true;
                break;
        }
    }
    protected override void DoMovementPatrol()
    {
        if (!PatrolPointTarget)
        {
            VerifyVariables();
        }
        Vector2 distanceToTargetTransform = PatrolPointTarget.position - transform.position;
        RigidBody.velocity = MoveForce * distanceToTargetTransform.normalized;
        // Set move direction to the matching direction which it is moving in
        if (_enemyMoveDirection == EnemyMoveDirection.Horizontal)
        {
            MoveDirection = (RigidBody.velocity.x < 0) ? Vector2.left : Vector2.right;
        }
        else
        {
            // Face the direction which the player is coming from
            MoveDirection = Vector2.left;
        }
        if (IsCloseToPatrolPointTarget())
        {
            SwapPatrolPointTarget();
        }
    }
    protected override void DoMovementTimed()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        if (EntityEffect.HasEnoughTimePassed(EffectMoveFlip))
        {
            EffectMoveFlip.SetNewTimeEffectApply();
            if (_enemyMoveDirection == EnemyMoveDirection.Horizontal)
                FlipMoveDirection(Vector2.right);
            else if (_enemyMoveDirection == EnemyMoveDirection.Vertical)
                FlipMoveDirection(Vector2.up);
        }
    }
    protected override bool IsCloseToPatrolPointTarget()
    {
        return Vector2.Distance(transform.position, PatrolPointTarget.position) <= PatrolPointDistance &&
            Vector2.Distance(transform.position, PatrolPointCurrent.position) > PatrolPointDistance;
    }
    protected override void FlipMoveDirectionOnCollision()
    {
        if (_enemyMoveDirection == EnemyMoveDirection.Horizontal)
            FlipMoveDirection(Vector2.right);
        else if (_enemyMoveDirection == EnemyMoveDirection.Vertical)
            FlipMoveDirection(Vector2.up);
    }
}

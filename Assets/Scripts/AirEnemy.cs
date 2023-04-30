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
    protected override void DoMovementTimed()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
        {
            EffectMoveFlip.SetNewTimeEffectApply();
            if (_enemyMoveDirection == EnemyMoveDirection.Horizontal)
                FlipMoveDirection(Vector2.right);
            else if (_enemyMoveDirection == EnemyMoveDirection.Vertical)
                FlipMoveDirection(Vector2.up);
        }
    }
}
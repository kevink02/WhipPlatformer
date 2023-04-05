﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : EntityMovement
{
    [SerializeField]
    private EnemyTypes _enemyMoveType;
    [SerializeField]
    [Tooltip("For airborne enemies, a point to travel to when moving")]
    private Transform _patrolPointStart, _patrolPointEnd;
    private Transform _patrolPointTarget;

    private enum EnemyTypes : int
    {
        Ground, AirVertical, AirHorizontal
    }

    private new void Awake()
    {
        base.Awake();
        _patrolPointTarget = _patrolPointStart;

        // Set physics values based on the enemy type
        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
            case EnemyTypes.AirVertical:
                MoveDirection = Vector2.up;
                RigidBody.gravityScale = 0;
                // Only allow changes to y position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
                RigidBody.freezeRotation = true;
                break;
            case EnemyTypes.AirHorizontal:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 0;
                // Only allow changes to x position;
                RigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
                RigidBody.freezeRotation = true;
                break;
            default:
                MoveDirection = Vector2.right;
                RigidBody.gravityScale = 1;
                break;
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    protected override void MoveHorizontally()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);

        // Check conditions to flip move direction, based on the enemy type
        switch (_enemyMoveType)
        {
            case EnemyTypes.Ground:
                // Did not detect a platform in front of it
                if (!HasCollidedWithPlatformAtDetectAngle())
                {
                    FlipMoveDirectionHorizontal();
                }
                break;
            case EnemyTypes.AirVertical:
                if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
                {
                    EffectMoveFlip.SetNewTimeEffectApply();
                    FlipMoveDirectionVertical();
                }
                break;
            case EnemyTypes.AirHorizontal:
                if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
                {
                    EffectMoveFlip.SetNewTimeEffectApply();
                    FlipMoveDirectionHorizontal();
                }
                break;
            default:
                break;
        }
    }
    protected override void MoveVertically()
    {
        throw new System.NotImplementedException();
    }
    private void FlipMoveDirectionHorizontal()
    {
        DetectVector = Vector2.Reflect(DetectVector, Vector2.right);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
        Debug.Log($"{name}: Endpoint at {transform.position}");
    }
    private void FlipMoveDirectionVertical()
    {
        DetectVector = Vector2.Reflect(DetectVector, Vector2.up);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
        Debug.Log($"{name}: Endpoint at {transform.position}");
    }
}

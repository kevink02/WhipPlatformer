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
    protected override void DoMovementTimed()
    {
        RigidBody.velocity = Vector2.Lerp(RigidBody.velocity, MoveForce * MoveDirection, MoveAccel);
        if (EntityEffect.HasEnoughTimeHasPassed(EffectMoveFlip))
        {
            EffectMoveFlip.SetNewTimeEffectApply();
            FlipMoveDirection(Vector2.right);
        }

        // Did not detect a platform in front of it
        //if (!HasCollidedWithPlatformAtDetectAngle())
        //    FlipMoveDirection(Vector2.right);
    }
}
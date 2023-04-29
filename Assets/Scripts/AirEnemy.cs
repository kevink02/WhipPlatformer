using System.Collections;
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
    /// <summary>
    /// Air enemies move via "patrol points" in the scene
    /// </summary>
    protected override void DoMovementPatrol()
    {
        Vector2 distanceToTargetTransform = _patrolPointTarget.position - transform.position;
        RigidBody.velocity = MoveForce * distanceToTargetTransform.normalized;
        if (Vector2.Distance(transform.position, _patrolPointTarget.position) <= 0.1f && Vector2.Distance(transform.position, _patrolPointCurrent.position) > 0.1f)
        {
            if (_patrolPointTarget == _patrolPointEnd)
            {
                _patrolPointCurrent = _patrolPointEnd;
                _patrolPointTarget = _patrolPointStart;
            }
            else
            {
                _patrolPointCurrent = _patrolPointStart;
                _patrolPointTarget = _patrolPointEnd;
            }
        }
    }
    /// <summary>
    /// Air enemies move via their set move force and time until switching directions
    /// </summary>
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

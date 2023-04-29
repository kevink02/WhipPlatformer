using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : EntityMovement
{
    [SerializeField]
    protected EnemyMoveType MoveType;
    [SerializeField]
    [Tooltip("Points to travel to when moving")]
    protected Transform _patrolPointStart, _patrolPointEnd;
    protected Transform _patrolPointTarget, _patrolPointCurrent;

    protected enum EnemyMoveType : int
    {
        Timed, Patrol
    }

    private new void Awake()
    {
        // Spawn point is set in base class here
        base.Awake();
        // Reset it if needed to its start patrol point
        if (MoveType == EnemyMoveType.Patrol)
        {
            SetInitialPositionToPatrolPoint();
        }
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    private void SetInitialPositionToPatrolPoint()
    {
        if (!_patrolPointStart)
            throw new Exception("The start patrol point is not set");
        if (_patrolPointStart == _patrolPointEnd)
            throw new Exception("The patrol points are the same, enemy can't move");

        _patrolPointCurrent = _patrolPointStart;
        transform.position = _patrolPointCurrent.position;
        _patrolPointTarget = _patrolPointEnd;
    }
    protected override void DoMovement()
    {
        // Check conditions to flip move direction, based on the enemy type
        if (MoveType == EnemyMoveType.Patrol)
            DoMovementPatrol();
        else
            DoMovementTimed();
    }
    protected abstract void DoMovementPatrol();
    protected abstract void DoMovementTimed();
    protected void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
}

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
    protected Transform PatrolPointStart, PatrolPointEnd;
    protected Transform PatrolPointTarget, PatrolPointCurrent;

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
        if (!PatrolPointStart)
            throw new Exception("The start patrol point is not set");
        if (PatrolPointStart == PatrolPointEnd)
            throw new Exception("The patrol points are the same, enemy can't move");

        PatrolPointCurrent = PatrolPointStart;
        transform.position = PatrolPointCurrent.position;
        PatrolPointTarget = PatrolPointEnd;
    }
    protected override void DoMovement()
    {
        // Check conditions to flip move direction, based on the enemy type
        if (MoveType == EnemyMoveType.Patrol)
            DoMovementPatrol();
        else
            DoMovementTimed();
    }
    /// <summary>
    /// Enemies move via "patrol points" in the scene
    /// </summary>
    protected abstract void DoMovementPatrol();
    /// <summary>
    /// Enemies move via their set move force and time until switching directions
    /// </summary>
    protected abstract void DoMovementTimed();
    protected void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
}

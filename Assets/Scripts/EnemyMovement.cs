using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : EntityMovement, IVerification
{
    [SerializeField]
    protected EnemyMoveType MoveType;
    [SerializeField]
    [Tooltip("Points to travel to when moving")]
    protected Transform PatrolPointEnd;
    protected Transform PatrolPointStart, PatrolPointTarget, PatrolPointCurrent;

    protected enum EnemyMoveType : int
    {
        Timed, Patrol
    }

    private new void Awake()
    {
        // Spawn point is set in base class here
        base.Awake();
        VerifyVariables();

        // Reset it if needed to its start patrol point
        if (MoveType == EnemyMoveType.Patrol)
        {
            SetInitialPositionToPatrolPoint();
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        // Switch move direction upon player collision in case the enemy would pin the player to a wall
        if (Game_Manager.IsObjectAPlayer(collision.gameObject))
        {
            // Check conditions to flip move direction, based on the enemy type
            FlipMoveDirectionOnCollision();
        }
        else if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    public new void VerifyVariables()
    {
        if (!SpawnPoint)
            throw new Exception("The spawn point is not set");
        if (!PatrolPointEnd)
            throw new Exception("The end patrol point is not set");

        PatrolPointStart = SpawnPoint;
        PatrolPointCurrent = PatrolPointStart;
        PatrolPointTarget = PatrolPointEnd;
    }
    private void SetInitialPositionToPatrolPoint()
    {
        transform.position = PatrolPointCurrent.position;
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
    protected abstract bool IsCloseToPatrolPointTarget();
    protected void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
    protected abstract void FlipMoveDirectionOnCollision();
}

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
        VerifyVariables();

        // Reset it if needed to its start patrol point
        if (MoveType == EnemyMoveType.Patrol)
        {
            SetInitialPositionToPatrolPoint();
        }
    }
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    public void VerifyVariables()
    {
        if (!PatrolPointStart)
            throw new Exception("The start patrol point is not set");
        if (PatrolPointStart == PatrolPointEnd)
            throw new Exception("The patrol points are the same, enemy can't move");

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
    protected void DoMovementPatrol()
    {
        if (!PatrolPointTarget)
        {

        }
        Vector2 distanceToTargetTransform = PatrolPointTarget.position - transform.position;
        RigidBody.velocity = MoveForce * distanceToTargetTransform.normalized;
        if (IsCloseToPatrolPointTarget())
        {
            if (PatrolPointTarget == PatrolPointEnd)
            {
                PatrolPointCurrent = PatrolPointEnd;
                PatrolPointTarget = PatrolPointStart;
            }
            else
            {
                PatrolPointCurrent = PatrolPointStart;
                PatrolPointTarget = PatrolPointEnd;
            }
        }
    }
    /// <summary>
    /// Enemies move via their set move force and time until switching directions
    /// </summary>
    protected abstract void DoMovementTimed();
    protected bool IsCloseToPatrolPointTarget()
    {
        return Vector2.Distance(transform.position, PatrolPointTarget.position) <= 0.1f &&
            Vector2.Distance(transform.position, PatrolPointCurrent.position) > 0.1f;
    }
    protected void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
}

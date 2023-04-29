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
    /// <summary>
    /// Air enemies move via "patrol points" in the scene
    /// </summary>
    protected void DoPatrolPointMovement()
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
    protected void FlipMoveDirection(Vector2 normal)
    {
        DetectVector = Vector2.Reflect(DetectVector, normal);
        DetectAngle = Game_Manager.GetAngleFromVector2(DetectVector);
        MoveDirection *= -1;
    }
}

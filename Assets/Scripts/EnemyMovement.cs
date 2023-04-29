using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : EntityMovement
{
    [SerializeField]
    [Tooltip("For airborne enemies, a point to travel to when moving")]
    private Transform _patrolPointStart, _patrolPointEnd;
    private Transform _patrolPointTarget, _patrolPointCurrent;

    private enum EnemyMoveTypes : int
    {
        Timed, Patrol
    }

    private new void Awake()
    {
        base.Awake();
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (Game_Manager.IsObjectAnInvisiblePlatform(collision.gameObject))
        {
            Destroy(gameObject);
        }
    }
    protected void SetInitialPositionToPatrolPoint()
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

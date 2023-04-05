using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Range(-360, 360f)]
    [SerializeField]
    [Tooltip("The angle to shoot the raycast")]
    protected float DetectAngle; // default = 315
    [Range(0.01f, 100)]
    [SerializeField]
    [Tooltip("The distance to have the raycast ray be")]
    protected float DetectDistance; // default = ?
    [Range(0.01f, 1)]
    [SerializeField]
    protected float MoveAccel; // default = 0.2f
    [Range(0.01f, 1)]
    [SerializeField]
    protected float MoveDecel; // default = 0.1f
    [Range(0, 1000)]
    [SerializeField]
    protected float JumpForce; // default = 500
    [SerializeField]
    protected float MoveForce; // default = 5
    protected Ray RayCastRay;
    protected RaycastHit2D RayCastHit;
    protected Rigidbody2D RigidBody;
    [Tooltip("The vector with the same angle as the raycast angle")]
    protected Vector2 DetectVector;

    protected void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }
    protected void FixedUpdate()
    {
        MoveHorizontally();
        CastRay();
    }
    protected void CastRay()
    {
        Debug.DrawRay(transform.position, DetectVector, Color.white, Game_Manager.DebugRayLifeTime);
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        RayCastRay = new Ray(transform.position, DetectVector);
        RayCastHit = Physics2D.Raycast(RayCastRay.origin, RayCastRay.direction, 100f, Game_Manager.PlatformMask);
    }
    protected abstract void MoveHorizontally();
    protected abstract void MoveVertically();
}

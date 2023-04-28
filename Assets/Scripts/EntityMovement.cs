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
    [SerializeField]
    protected float MoveForce; // default = 5
    [SerializeField]
    protected Transform SpawnPoint;

    protected bool IsGrounded;
    protected BoxCollider2D Collider;
    protected Ray RayCastRay;
    protected RaycastHit2D RayCastHit;
    protected Rigidbody2D RigidBody;
    protected SpriteRenderer ComponentSprite;
    [Tooltip("The vector with the same angle as the raycast angle")]
    protected Vector2 DetectVector;
    protected Vector2 MoveDirection;

    protected EntityEffect EffectKnockback;
    [SerializeField]
    private float _cooldownKnockback; // default = 0.5f
    [SerializeField]
    private Vector2 _forceKnockback; // default = new Vector(750f, 100f)
    protected EntityEffect EffectJump;
    [Range(0.01f, 1)]
    [SerializeField]
    private float _cooldownJump; // default = 0.5f
    [SerializeField]
    private Vector2 _forceJump; // default = 500
    protected EntityEffect EffectMoveFlip;
    [Range(0.01f, 10)]
    [SerializeField]
    private float _cooldownMoveFlip;
    private readonly Vector2 _forceMoveFlip = Vector2.zero;

    protected void Awake()
    {
        Collider = GetComponent<BoxCollider2D>();
        RigidBody = GetComponent<Rigidbody2D>();
        ComponentSprite = GetComponent<SpriteRenderer>();
        DetectVector = Game_Manager.GetVector2FromAngle(DetectAngle);
        DetectDistance = GetDetectDistance(DetectVector);

        // Collider shouldn't be too big since it will interfere with the platforms in tilemaps
        //Collider.size = ComponentSprite.sprite.rect.size;

        SetPositionToSpawnPoint();
        // Verify values of the knockback force
        SetEntityEffectForceValues();
        SetEntityEffects();
    }
    protected void FixedUpdate()
    {
        DoMovement();
        CastRay();
        IsGrounded = HasCollidedWithPlatformUnderneath();
    }
    protected abstract void OnCollisionEnter2D(Collision2D collision);
    protected void CastRay()
    {
        Debug.DrawRay(transform.position, DetectVector, Color.white, Game_Manager.DebugRayLifeTime);
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        RayCastRay = new Ray(transform.position, DetectVector);
        RayCastHit = Physics2D.Raycast(RayCastRay.origin, RayCastRay.direction, DetectDistance, Game_Manager.PlatformMask);
    }
    protected abstract void DoMovement();
    private void SetEntityEffectForceValues()
    {
        if (_forceKnockback.x < 1)
            _forceKnockback.x = 750;
        if (_forceKnockback.y <= 0)
            _forceKnockback.y = 100;

        if (_forceJump.x != 0)
            _forceJump.x = 0;
        if (_forceJump.y <= 0)
            _forceJump.y = 500;
    }
    private void SetEntityEffects()
    {
        EffectKnockback = new EntityEffect(_cooldownKnockback, _forceKnockback);
        EffectJump = new EntityEffect(_cooldownJump, _forceJump);
        EffectMoveFlip = new EntityEffect(_cooldownMoveFlip, _forceMoveFlip);
    }
    protected void SetPositionToSpawnPoint()
    {
        transform.position = SpawnPoint.position;
    }
    protected bool HasCollidedWithPlatformAtDetectAngle()
    {
        return RayCastHit && Game_Manager.IsObjectAPlatform(RayCastHit.collider.gameObject);
    }
    protected bool HasCollidedWithPlatformUnderneath()
    {
        Ray ray = new Ray(transform.position, Vector2.down);
        float detectDistance = GetDetectDistance(Vector2.down);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, detectDistance, Game_Manager.PlatformMask);

        float halfColliderWidth = Collider.size.x / 2;
        // Ray starting at leftmost position of collider
        Ray rayLeft = new Ray(transform.position + Vector3.left * halfColliderWidth, Vector2.down);
        // Ray starting at rightmost position of collider
        Ray rayRight = new Ray(transform.position + Vector3.right * halfColliderWidth, Vector2.down);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, detectDistance, Game_Manager.PlatformMask);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, detectDistance, Game_Manager.PlatformMask);

        // Check if some part of the player is touching the ground (leftmost side, middle, or rightmost side)
        return (hit && Game_Manager.IsObjectAPlatform(hit.collider.gameObject)) ||
            (hitLeft && Game_Manager.IsObjectAPlatform(hitLeft.collider.gameObject)) ||
            (hitRight && Game_Manager.IsObjectAPlatform(hitRight.collider.gameObject));
    }
    // This is assuming the transform.position is in the center of the collider, at least in the center in terms of y-axis
    private float GetDetectDistance(Vector2 detectVector)
    {
        float angleFrom270 = Game_Manager.GetAngleBetweenVector2s(Vector2.down, detectVector);
        if (angleFrom270 < 0)
            angleFrom270 *= -1;
        float a = Collider.size.y / 2;
        float h = (a + Game_Manager.RayCastRayOffset) / Mathf.Cos(angleFrom270 * Mathf.Deg2Rad);
        // Take into account the tile size in the tilemap
        return h * 10;
    }
}

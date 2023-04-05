using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private LayerMask _platformMask;
    public static LayerMask PlatformMask;

    public static readonly float DebugRayLifeTime = 0.02f;
    [Tooltip("When calculating distance of ray for raycast, use this to account for small gaps between colliders for better physics performance")]
    public static readonly float RayCastRayOffset = 0.2f;
    /// <summary>
    /// Returns an angle in degrees
    /// </summary>
    public static Func<Vector2, float> GetAngleFromVector2 { get; } = vector2 => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Func<float, Vector2> GetVector2FromAngle { get; } = angleInDegrees => new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    public static Func<GameObject, bool> IsObjectAPlatform { get; } = objectToCheck => objectToCheck.CompareTag("Platform");
    public static Func<GameObject, bool> IsObjectAnInvisiblePlatform { get; } = objectToCheck => objectToCheck.CompareTag("VoidCheck");
    public static Func<Vector2, Vector2, float> GetAngleBetweenVector2s { get; } =
     (vector1, vector2) => Mathf.Acos(GetDotProductFromVector2s(vector1, vector2) / GetMagnitudeProductFromVector2s(vector1, vector2)) * Mathf.Rad2Deg;
    private static Func<Vector2, Vector2, float> GetDotProductFromVector2s { get; } = (vector1, vector2) => Vector2.Dot(vector1, vector2);
    private static Func<Vector2, Vector2, float> GetMagnitudeProductFromVector2s { get; } = (vector1, vector2) => vector1.magnitude * vector2.magnitude;
    /// <summary>
    /// Checks if enough time has passed for some effect with a cooldown time
    /// </summary>
    public static Func<float, float, bool> CheckIfEnoughTimeHasPassed { get; } = (timeOfLastEvent, effectCooldownTime) => Time.time >= timeOfLastEvent + effectCooldownTime;

    private void Awake()
    {
        PlatformMask = _platformMask;
    }
}

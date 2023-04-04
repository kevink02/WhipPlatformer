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
    /// <summary>
     /// Returns an angle in degrees
     /// </summary>
    public static Func<Vector2, float> GetAngleFromVector2 { get; } = vector2 => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Func<float, Vector2> GetVector2FromAngle { get; } = angleInDegrees => new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    public static Func<GameObject, bool> IsObjectAPlatform { get; } = objectToCheck => objectToCheck.CompareTag("Platform");

    private void Awake()
    {
        PlatformMask = _platformMask;
    }
}

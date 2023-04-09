using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    [SerializeField]
    private bool _shouldAirborneEnemiesPatrol;
    public static bool ShouldAirborneEnemiesPatrol;
    [SerializeField]
    private LayerMask _platformMask;
    public static LayerMask PlatformMask;
    [SerializeField]
    private GameObject _levelEntrance;
    public static GameObject LevelEntrance;
    [SerializeField]
    private GameObject _levelExit;
    public static GameObject LevelExit;

    public static readonly float DebugRayLifeTime = 0.02f;
    [Tooltip("When calculating distance of ray for raycast, use this to account for small gaps between colliders for better physics performance")]
    public static readonly float RayCastRayOffset = 0.2f;
    /// <summary>
    /// Returns an angle in degrees
    /// </summary>
    public static Func<Vector2, float> GetAngleFromVector2 { get; } = vector2 => Mathf.Atan2(vector2.y, vector2.x) * Mathf.Rad2Deg;
    public static Func<float, Vector2> GetVector2FromAngle { get; } = angleInDegrees => new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    public static Func<Vector2, Vector2, float> GetAngleBetweenVector2s { get; } =
     (vector1, vector2) => Mathf.Acos(GetDotProductFromVector2s(vector1, vector2) / GetMagnitudeProductFromVector2s(vector1, vector2)) * Mathf.Rad2Deg;
    private static Func<Vector2, Vector2, float> GetDotProductFromVector2s { get; } = (vector1, vector2) => Vector2.Dot(vector1, vector2);
    private static Func<Vector2, Vector2, float> GetMagnitudeProductFromVector2s { get; } = (vector1, vector2) => vector1.magnitude * vector2.magnitude;
    public static Func<GameObject, bool> IsObjectAPlatform { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Platform");
    public static Func<GameObject, bool> IsObjectAnInvisiblePlatform { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "VoidCheck");
    public static Func<GameObject, bool> IsObjectAnEnemy { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Enemy");
    public static Func<GameObject, bool> IsObjectAPlayer { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "Player");
    public static Func<GameObject, bool> IsObjectALevelEnd { get; } = objectToCheck => IsObjectAGeneric(objectToCheck, "LevelEnd");
    private static Func<GameObject, string, bool> IsObjectAGeneric { get; } = (objectToCheck, tagToCheck) => objectToCheck.CompareTag(tagToCheck);

    private void Awake()
    {
        SetStatics();
    }
    private void SetStatics()
    {
        ShouldAirborneEnemiesPatrol = _shouldAirborneEnemiesPatrol;
        PlatformMask = _platformMask;
        LevelEntrance = _levelEntrance;
        LevelExit = _levelExit;
    }
}

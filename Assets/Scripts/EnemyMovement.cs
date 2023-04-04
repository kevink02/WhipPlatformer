using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Range(0, 360f)]
    [SerializeField]
    private float _detectAngle; // default = 315
    [SerializeField]
    private float _moveSpeed;
    private Vector2 _detectVector; // the vector corresponding to the detect angle

    private void Awake()
    {
        _detectVector = Game_Manager.GetVector2FromAngle(_detectAngle);
    }
    private void FixedUpdate()
    {
        
        Debug.DrawRay(transform.position, _detectVector, Color.white, Game_Manager.DebugRayLifeTime);

        // Move in current direction
        // If its raycast detects the end of its current platform, switch directions (raycast detection angle will flip to match its direction as well)
        Ray ray = new Ray(transform.position, _detectVector);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, Game_Manager.PlatformMask);
        // Did not detect a platform in front of it
        if (!(hit && hit.collider.CompareTag("Platform")))
        {

        }
    }
}

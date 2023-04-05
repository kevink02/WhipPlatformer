using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField]
    private float _lerpSpeed;
    private PlayerMovement _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>();
    }
    private void FixedUpdate()
    {
        // Prevent camera's z position from changing (makes camera zoom in too close)
        transform.position = Vector3.Lerp(transform.position, _player.transform.position + Vector3.forward * transform.position.z, _lerpSpeed);
    }
}

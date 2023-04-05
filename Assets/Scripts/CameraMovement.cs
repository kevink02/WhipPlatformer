using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField]
    private float _lerpSpeed; // default = 0.2f
    [Range(1, 50f)]
    [SerializeField]
    private float _cameraSizeZoomIn; // default = 5f
    [Range(1, 50f)]
    [SerializeField]
    private float _cameraSizeZoomOut; // default = 20f
    private Camera _camera;
    private PlayerMovement _player;
    [Tooltip("Holds a constant reference to the camera's z position")]
    private Vector3 _positionZ;

    private static bool _isZoomedOut;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _player = FindObjectOfType<PlayerMovement>();
        _positionZ = Vector3.forward * transform.position.z;
    }
    private void FixedUpdate()
    {
        if (_isZoomedOut)
        {
            transform.position = Vector3.zero + _positionZ;
            _camera.orthographicSize = 20f;
        }
        else
        {
            // Prevent camera's z position from changing (makes camera zoom in too close)
            transform.position = Vector3.Lerp(transform.position, _player.transform.position + _positionZ, _lerpSpeed);
            _camera.orthographicSize = 5f;
        }
    }
    public static void SwitchCameraZoom()
    {
        _isZoomedOut = !_isZoomedOut;
    }
}

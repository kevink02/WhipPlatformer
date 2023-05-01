using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour, IVerification
{
    [Range(0.01f, 1)]
    [SerializeField]
    private float _lerpSpeed; // default = 0.2f
    [Range(10, 75f)]
    [SerializeField]
    private float _cameraSizeZoomIn; // default = 25f
    [Range(10, 75f)]
    [SerializeField]
    private float _cameraSizeZoomOut; // default = 50f
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

        VerifyVariables();
        transform.position = _player.transform.position + _positionZ;
    }
    private void FixedUpdate()
    {
        // Prevent camera's z position from changing (makes camera zoom in too close)
        transform.position = Vector3.Lerp(transform.position, _player.transform.position + _positionZ, _lerpSpeed);
        _camera.orthographicSize = (_isZoomedOut) ? _cameraSizeZoomOut : _cameraSizeZoomIn;
    }
    public void VerifyVariables()
    {
        // If invalid zoom out size (zooming out would actually zoom in the camera)
        if (_cameraSizeZoomOut < _cameraSizeZoomIn)
        {
            _cameraSizeZoomIn = 25f;
            _cameraSizeZoomOut = 50f;
        }
    }
    public static void SwitchCameraZoom()
    {
        if (PauseMenu.IsPaused())
            return;

        _isZoomedOut = !_isZoomedOut;
    }
}

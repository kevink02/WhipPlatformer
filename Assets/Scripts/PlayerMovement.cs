using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.performed += _ => MoveHorizontally(_playerControls.Movement.HorizontalMove.ReadValue<Vector2>());
        _playerControls.Movement.VerticalMove.performed += _ => MoveVertically(_playerControls.Movement.VerticalMove.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        _playerControls.Movement.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Movement.Disable();
    }
    private void MoveHorizontally(Vector2 movement)
    {
        Debug.Log($"{name}: Moving {movement}");
    }
    private void MoveVertically(Vector2 movement)
    {
        Debug.Log($"{name}: Moving {movement}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.performed += cxt => MoveHorizontally(cxt);
        _playerControls.Movement.VerticalMove.performed += _ => MoveVertically();
    }
    private void OnEnable()
    {
        _playerControls.Movement.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Movement.Disable();
    }
    private void MoveHorizontally(InputAction.CallbackContext cxt)
    {
        Debug.Log($"{name}: Moving {cxt.ReadValue<float>()}");
    }
    private void MoveVertically()
    {
        Debug.Log($"{name}: Jumping up");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 1000)]
    private float _jumpForce;

    private PlayerControls _playerControls;
    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.performed += cxt => MoveHorizontally(cxt);
        _playerControls.Movement.VerticalMove.performed += _ => MoveVertically();
        _playerControls.Abilities.Attack.performed += _ => AbilityAttack();
        _playerControls.Abilities.Interact.performed += _ => AbilityInteract();
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    private void MoveHorizontally(InputAction.CallbackContext cxt)
    {
        Debug.Log($"{name}: Moving {cxt.ReadValue<float>()}");
    }
    private void MoveVertically()
    {
        Debug.Log($"{name}: Jumping");
        _rigidBody2D.AddForce(_jumpForce * Vector2.up);
    }
    private void AbilityAttack()
    {
        Debug.Log($"{name}: Attacking");
    }
    private void AbilityInteract()
    {
        Debug.Log($"{name}: Interacting");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(0, 1000)]
    private float _jumpForce;
    [SerializeField]
    private float _moveForce;

    private PlayerControls _playerControls;
    private Rigidbody2D _rigidBody2D;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _playerControls.Movement.HorizontalMove.performed += cxt => MoveHorizontally(cxt.ReadValue<float>());
        _playerControls.Movement.HorizontalMove.canceled += cxt => MoveHorizontally(cxt.ReadValue<float>());
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
    private void MoveHorizontally(float moveDirection)
    {
        _rigidBody2D.velocity = _moveForce * moveDirection * Vector2.right;
    }
    private void MoveVertically()
    {
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

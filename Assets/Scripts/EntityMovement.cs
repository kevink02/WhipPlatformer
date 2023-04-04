using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityMovement : MonoBehaviour
{
    [Range(0.01f, 1)]
    [SerializeField]
    protected float MoveAccel; // default = 0.2f
    [Range(0.01f, 1)]
    [SerializeField]
    protected float MoveDecel; // default = 0.1f
    [Range(0, 1000)]
    [SerializeField]
    protected float JumpForce; // default = 500
    [SerializeField]
    protected float MoveForce; // default = 5
    protected Rigidbody2D RigidBody;

    protected void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }
    protected void FixedUpdate()
    {
        MoveHorizontally();
    }
    protected abstract void MoveHorizontally();
    protected abstract void MoveVertically();
}

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float sprintSpeed = 8.0f;

    private bool _isMoving;
    public bool IsMoving
    {
        get { return _isMoving; }
        set 
        {
            _isMoving = value; 
            _animator.SetBool(AnimationStrings.IsMove, _isMoving);
        }
    }

    private bool _isSprint;
    public bool IsSprint
    {
        get { return _isSprint; }
        set
        {
            _isSprint = value;
            _animator.SetBool(AnimationStrings.IsSprint, _isSprint);
        }
    }

    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private Vector2 _moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float currentSpeed = IsSprint ? sprintSpeed : moveSpeed;
        _rigidbody.linearVelocity = new Vector2(_moveInput.x * currentSpeed, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        IsMoving = _moveInput.x != 0;
        SetFacingDirection(_moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        // 오른쪽 방향
        if(moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        // 왼쪽 방향
        else if(moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Left Shift 키를 누른 순간에 호출
        if(context.started)
        {
            IsSprint = true;
        }
        // Left Shift 키를 뗀 순간에 호출
        else if (context.canceled)
        {
            IsSprint = false;
        }
    }
}

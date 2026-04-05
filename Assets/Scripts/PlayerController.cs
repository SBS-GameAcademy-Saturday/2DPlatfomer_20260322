using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float sprintSpeed = 8.0f;
    public float jumpPower = 8.0f;
    public int maxJumpCount = 2;

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
    private TouchingDirections _touchingDirections;
    private Vector2 _moveInput;
    private int _currentJumpCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
    }

    void FixedUpdate()
    {
        float currentSpeed = IsSprint ? sprintSpeed : moveSpeed;
        if(_touchingDirections.IsOnWall || !_animator.GetBool(AnimationStrings.CanMove))
        {
            currentSpeed = 0;
        }
        _rigidbody.linearVelocity = new Vector2(_moveInput.x * currentSpeed, _rigidbody.linearVelocityY);
        _animator.SetFloat(AnimationStrings.VelocityY, _rigidbody.linearVelocityY);
    }

    private void Update()
    {
        if (_rigidbody.linearVelocityY <= 0 && _touchingDirections.IsGrounded)
        {
            _currentJumpCount = 0;
        }
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

    public void OnJump(InputAction.CallbackContext context)
    {
        // 공격중이면 점프 방지
        if (!_animator.GetBool(AnimationStrings.CanMove))
            return;

        // 땅에 닿아 있을 때만 점프가 가능하도록 작업
        // 2단 점프(확장성 -> 3단 점프 이상으로도 가능하겠끔) 
        if(context.started && _currentJumpCount < maxJumpCount)
        {
            // _rigidbody.linearVelocityX => X축 기반 속도
            // Y값은 JumpPower 값으로 초기화 하지만 X축 값은 지금 현재 상태를 유지한다.
            _animator.SetTrigger(AnimationStrings.Jump);
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX,jumpPower);
            _currentJumpCount++;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _animator.SetTrigger(AnimationStrings.Attack);
        }
    }

}

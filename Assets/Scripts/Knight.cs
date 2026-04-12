using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TouchingDirections))]
[RequireComponent(typeof(Animator))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public DetectionZone cliffDetectionZone;
    public DetectionZone attackTriggerZone;
    public float attackCoolTime = 5;

    private Rigidbody2D _rigidbody;
    private TouchingDirections _touchingDirections;
    private Animator _animator;
    private Damageable _damageable;

    private Vector2 walkDirectionVector = Vector2.right;
    private float _elapsedAttackCoolTime = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
        _damageable = GetComponent<Damageable>();
        walkDirectionVector = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    private void Update()
    {
        if (_elapsedAttackCoolTime > 0)
        {
            float elapsedTime = _elapsedAttackCoolTime - Time.deltaTime;
            _elapsedAttackCoolTime = Mathf.Max(0, elapsedTime);
        }

        bool hasTarget = attackTriggerZone.DetectedColidersCount > 0;
        bool canAttack = hasTarget && _elapsedAttackCoolTime <= 0;
        _animator.SetBool(AnimationStrings.HasTarget, canAttack);
        if (canAttack)
        {
            _elapsedAttackCoolTime = attackCoolTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_damageable.IsAlive)
            return;

        if (_touchingDirections.IsGrounded && _touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if(cliffDetectionZone.DetectedColidersCount <= 0)
        {
            FlipDirection();
        }

        if(_damageable.LockVelocity)
        {
            // Knocknack => 넉백으로 인해서 뒤로 물러나가면서 -> 다시 제자리에 서 있도록
            // 예) linearVelocity.x = 10;
            // 1 Frame => 10
            // 2 Frame => 9
            // 3 Frame => 8
            // ... 점차 속도가 느려지면서 
            // 0으로 가서 멈춰야 한다.
            _rigidbody.linearVelocity = new Vector2(Mathf.Lerp(_rigidbody.linearVelocityX, 0, Time.deltaTime), 
                _rigidbody.linearVelocityY);
        }
        else
        {
            if(_animator.GetBool(AnimationStrings.CanMove))
                _rigidbody.linearVelocity = new Vector2(walkDirectionVector.x * walkSpeed, _rigidbody.linearVelocityY);
            else
                _rigidbody.linearVelocity = new Vector2(0 * walkSpeed, _rigidbody.linearVelocityY);
        }
    }

    private void FlipDirection()
    {
        if (transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            walkDirectionVector = Vector2.left;
        }
        else if(transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            walkDirectionVector = Vector2.right;
        }
    }

    public void OnKnockback(Vector2 knockback)
    {
        // 넉백을 받으면 넉백을 받은 방향으로 이동
        _rigidbody.linearVelocity = new Vector2(knockback.x, knockback.y + _rigidbody.linearVelocityY);
        if(knockback.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            walkDirectionVector = Vector2.left;
        }
        else if(knockback.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            walkDirectionVector = Vector2.right;
        }
    }

}

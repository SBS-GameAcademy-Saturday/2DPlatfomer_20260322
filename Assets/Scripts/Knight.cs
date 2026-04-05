using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TouchingDirections))]
[RequireComponent(typeof(Animator))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public DetectionZone cliffDetectionZone;


    private Rigidbody2D _rigidbody;
    private TouchingDirections _touchingDirections;
    private Animator _animator;

    private Vector2 walkDirectionVector = Vector2.right;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
        walkDirectionVector = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(_touchingDirections.IsGrounded && _touchingDirections.IsOnWall)
        {
            FlipDirection();
        }
        if(cliffDetectionZone.DetectedColidersCount <= 0)
        {
            FlipDirection();
        }

        _rigidbody.linearVelocity = new Vector2(walkDirectionVector.x * walkSpeed, _rigidbody.linearVelocityY);
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

}

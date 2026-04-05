using UnityEngine;

public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D contactFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;

    private bool _isGrounded = false;
    public bool IsGrounded
    {
        get { return _isGrounded; }
        set 
        {
            _isGrounded = value;
            _animator.SetBool(AnimationStrings.IsGrounded, _isGrounded);
        }
    }

    private bool _isOnWall;
    public bool IsOnWall
    {
        get { return _isOnWall; }
        set
        {
            _isOnWall = value;     
        }
    }

    private Vector2 wallCheckDirection
    {
        get
        {
            return gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }
    }

    private Collider2D _touchingCol;
    private Animator _animator;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits = new RaycastHit2D[5];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _touchingCol = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 매 프레임마다 내가 땅이랑 닿아 있는지를 판단
        // Cast를 통해서 받아오는 값 => int 데이터(현재 맞닿아 있는 GameObject들의 총 수)
        // Vector2.down 방향에 groundDistance내에 contactFilter로 필터링한 GameObject들의 수가 0 이상
        // => Ground와 맞닿아 있다
        IsGrounded = _touchingCol.Cast(Vector2.down, contactFilter, groundHits, groundDistance) > 0;
        IsOnWall = _touchingCol.Cast(wallCheckDirection, contactFilter, wallHits, wallDistance) > 0;
    }
}

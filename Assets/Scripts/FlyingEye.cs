using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Damageable))]
public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 3.0f;
    public DetectionZone attackTriggerZone;
    public float attackCoolTime = 5.0f;
    public float waypointReachedDistance = 0.1f;
    

    public List<Transform> wayPoints;

    public GameObject deathCollider;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private Damageable _damageable;

    private float _elapsedAttackCoolTime = 0;
    private int currentWaypointIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
    }

    private void OnEnable()
    {
        _damageable.OnChangeHealth.AddListener(OnHealthChange);
    }

    private void OnDisable()
    {
        _damageable.OnChangeHealth.RemoveListener(OnHealthChange);
    }

    // Update is called once per frame
    void Update()
    {
        if(_elapsedAttackCoolTime > 0)
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

    private void FixedUpdate()
    {
        if (!_damageable.IsAlive)
            return;

        if(_animator.GetBool(AnimationStrings.CanMove))
        {
            Flight();
        }
        else
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
        }
    }

    private void Flight()
    {
        // 내가 목표로 하고 있는 WayPoint의 위치와 내 위치를 계산해서 

        Transform currentWayPoint = wayPoints[currentWaypointIndex];

        // 내 현 목적지 위치로 이동
        Vector2 directionToWaypoint = (currentWayPoint.position - transform.position).normalized;
        _rigidbody2D.linearVelocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        // 내 현 목적지와 내 위치 사이의 거리
        float distance = Vector2.Distance(currentWayPoint.position, transform.position);
        if(distance < waypointReachedDistance)
        {
            currentWaypointIndex++;
            if(currentWaypointIndex >= wayPoints.Count)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if(transform.localScale.x > 0)
        {
            if(_rigidbody2D.linearVelocityX < 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }
        }
        else
        {
            if (_rigidbody2D.linearVelocityX > 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x, localScale.y, localScale.z);
            }
        }
    }

    private void OnHealthChange(float currentHealth, float maxHealth)
    {
        if(currentHealth <= 0)
        {
            _rigidbody2D.gravityScale = 2f;
            _rigidbody2D.linearVelocity = new Vector2(0, _rigidbody2D.linearVelocityY);
            deathCollider.SetActive(true);
        }
    }
}

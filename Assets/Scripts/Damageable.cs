using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<Vector2> OnKnockback = new UnityEvent<Vector2>();
    public UnityEvent<float, float> OnChangeHealth = new UnityEvent<float, float>();

    public bool LockVelocity
    {
        get { return _animator.GetBool(AnimationStrings.LockVelocity); }
        set { _animator.SetBool(AnimationStrings.LockVelocity, value); }
    }

    [SerializeField] private int _maxHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
    }

    [SerializeField] private int _health = 100;

    public int Health
    {
        get { return _health; }
    }

    public bool IsAlive => _health > 0;

    // 무적 상태
    [SerializeField] private bool _isInvincible = false;
    // 무적 시간
    [SerializeField] private float _invinciblilityTime = 0.5f;

    // 데미지를 받은 시간
    private float timeSinceHit = 0;
    private Animator _animator;

    private void Awake()
    {
        _health = _maxHealth;
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_isInvincible)
        {
            if(timeSinceHit > _invinciblilityTime)
            {
                _isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool GetHit(int damage)
    {
        if (IsAlive && !_isInvincible)
        {
            int newHealth = _health - damage;
            _health = Math.Max(newHealth, 0);

            if(_health <= 0)
            {
                _animator.SetBool(AnimationStrings.Death, true);
            }
            UIManager.Instance.CharacterTakeDamage(gameObject, damage);
            _isInvincible = true;
            OnChangeHealth.Invoke(Health, MaxHealth);
            _animator.SetTrigger(AnimationStrings.Hit);
            return true;
        }
        return false;
    }

    public bool GetHit(int damage, Vector2 knockback)
    {
        if (IsAlive && !_isInvincible)
        {
            int newHealth = _health - damage;
            _health = Math.Max(newHealth, 0);

            if (_health <= 0)
            {
                _animator.SetBool(AnimationStrings.Death, true);
            }
            UIManager.Instance.CharacterTakeDamage(gameObject, damage);
            _isInvincible = true;
            _animator.SetTrigger(AnimationStrings.Hit);
            OnChangeHealth.Invoke(Health, MaxHealth);

            // 넉백 이벤트 호출
            LockVelocity = true;
            OnKnockback.Invoke(knockback);
            return true;
        }
        return false;
    }


    public bool Heal(int healthRestore)
    {
        if(IsAlive && Health < MaxHealth)
        {
            // 내가 채울있는 체력만큼만 체력 회복을 시켜줌
            // Health : 90
            // healthRestore : 20
            // Health ; 110(X),
            // Health : 100(O)

            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            _health += actualHeal;
            UIManager.Instance.CharacterTakeHeal(gameObject, actualHeal);
            OnChangeHealth.Invoke(Health, MaxHealth);
            return true;
        }
        return false;
    }
}

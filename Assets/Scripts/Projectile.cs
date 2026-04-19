using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int _attackDamage = 10;
    public Vector2 knockback = Vector2.zero;

    public Vector2 moveSpeed = new Vector2(3f, 0);

    private Rigidbody2D _rigidbody2D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity()
    {
        _rigidbody2D.linearVelocity = new Vector2(moveSpeed.x * transform.localScale.x, moveSpeed.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Damageable damageable))
        {
            Vector2 deliveredKnockback = transform.localScale.x > 0 ? knockback :
                new Vector2(-knockback.x, knockback.y);
            bool gotHit = damageable.GetHit(_attackDamage, deliveredKnockback);
            if (gotHit)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

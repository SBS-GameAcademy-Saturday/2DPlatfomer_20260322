using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] private int _attackDamage = 10;
    [SerializeField] private Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Damageable damageable))
        {
            Vector2 deliveredKnockback = transform.parent.localScale.x > 0 ? knockback : new Vector2(-knockback.x, knockback.y);
            damageable.GetHit(_attackDamage, deliveredKnockback);
        }
    }
}

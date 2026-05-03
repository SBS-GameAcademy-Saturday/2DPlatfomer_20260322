using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HealthPickup : MonoBehaviour
{
    public int healthRestore = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 180, 0);

    AudioSource pickupSource;

    private void Awake()
    {
        pickupSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinRotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Damageable>(out Damageable damageable))
        {
            bool wasHealed = damageable.Heal(healthRestore);
            if (wasHealed)
            {
                AudioSource.PlayClipAtPoint(pickupSource.clip, transform.position, pickupSource.volume);
                Destroy(gameObject);
            }
        }
    }
}

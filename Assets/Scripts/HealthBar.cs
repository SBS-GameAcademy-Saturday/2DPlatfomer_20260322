using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Damageable damageable;
    public Slider healthSldierBar;
    public TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnHealthChanged(damageable.Health, damageable.MaxHealth);
    }

    private void OnEnable()
    {
        damageable.OnChangeHealth.AddListener(OnHealthChanged);
    }

    private void OnDisable()
    {
        damageable.OnChangeHealth.RemoveListener(OnHealthChanged);
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        healthSldierBar.value = currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}

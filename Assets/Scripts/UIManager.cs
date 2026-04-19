using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton ∆–≈œ
    public static UIManager Instance;

    public GameObject damageTextPrefab;
    public GameObject healTextPrefab;

    private Canvas gameCanvas;


    private void Awake()
    {
        Instance = this;
        gameCanvas = GetComponent<Canvas>();
    }

    public void CharacterTakeDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        GameObject text = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, this.transform);
        if(text.TryGetComponent(out TextMeshProUGUI textMeshPro))
        {
            textMeshPro.text = damageReceived.ToString();
        }
    }


    public void CharacterTakeHeal(GameObject character, int damageRestored)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);
        GameObject text = Instantiate(healTextPrefab, spawnPosition, Quaternion.identity, this.transform);
        if (text.TryGetComponent(out TextMeshProUGUI textMeshPro))
        {
            textMeshPro.text = damageRestored.ToString();
        }
    }
}

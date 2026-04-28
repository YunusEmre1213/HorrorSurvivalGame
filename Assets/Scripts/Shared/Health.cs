using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;

    // GÜNCELLEME: EnemyAI okuyabilsin diye public yapýldý, editörde gizlendi
    [HideInInspector]
    public float currentHealth;

    private bool isDead = false;

    [Header("Görsel Efektler")]
    public GameObject damagePopupPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Vector3 hitPosition)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (gameObject.CompareTag("Player"))
        {
            VitalsManager vitals = FindAnyObjectByType<VitalsManager>();
            if (vitals != null) vitals.UpdateHealthBar(currentHealth, maxHealth);

            PsychologyManager psycho = GetComponent<PsychologyManager>();
            if (psycho != null)
            {
                psycho.DecreasePsycho(amount * 0.5f);
            }
        }

        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, hitPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(amount);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " öldü.");

        if (gameObject.CompareTag("Player"))
        {
            Invoke("RestartLevel", 2f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
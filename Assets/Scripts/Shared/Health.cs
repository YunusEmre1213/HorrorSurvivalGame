using UnityEngine;
using UnityEngine.SceneManagement; // BU SATIRI EKLE (Sahneleri yönetmek için)

public class Health : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false; // Karakterin bir kez ölmesini saðlamak için

    [Header("Görsel Efektler")]
    public GameObject damagePopupPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Vector3 hitPosition)
    {
        if (isDead) return; // Eðer zaten ölüysek hasar alma

        currentHealth -= amount;

        // UI Güncelleme (Player ise)
        if (gameObject.CompareTag("Player"))
        {
            VitalsManager vitals = FindAnyObjectByType<VitalsManager>();
            if (vitals != null)
            {
                vitals.UpdateHealthBar(currentHealth, maxHealth);
            }
        }

        // Hasar Yazýsý
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, hitPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(amount);
        }

        // Öldü mü kontrolü
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
            // OYUNCU ÖLDÜÐÜNDE NE OLSUN?
            // Þimdilik sahneyi 2 saniye sonra yeniden baþlatalým:
            Invoke("RestartLevel", 2f);

            // Alternatif: Karakterin hareketini burada durdurabilirsin
            // GetComponent<PlayerController>().enabled = false;
        }
        else
        {
            // Düþman öldüðünde onu yok et
            Destroy(gameObject);
        }
    }

    void RestartLevel()
    {
        // Mevcut sahneyi yeniden yükler
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
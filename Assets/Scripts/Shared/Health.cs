using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Can Ayarlarý")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Görsel Efektler")]
    public GameObject damagePopupPrefab;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, Vector3 hitPosition)
    {
        // Ýki kere yazýlan hasar satýrýný teke düþürdük
        currentHealth -= amount;

        // AJAN 1: Vurulma bu scripte ulaþýyor mu?
        Debug.Log(gameObject.name + " hasar aldý! Kalan Can: " + currentHealth);

        // EÐER HASAR ALAN KÝÞÝ OYUNCU (PLAYER) ÝSE ARAYÜZÜ GÜNCELLE:
        if (gameObject.CompareTag("Player"))
        {
            // AJAN 2: Tag (Etiket) doðru ayarlanmýþ mý?
            Debug.Log("Player tag'i algýlandý, VitalsManager aranýyor...");

            VitalsManager vitals = FindAnyObjectByType<VitalsManager>();

            if (vitals != null)
            {
                // AJAN 3: Manager bulundu, bar verisi gönderiliyor!
                Debug.Log("VitalsManager bulundu! Bar güncelleniyor: " + currentHealth + " / " + maxHealth);
                vitals.UpdateHealthBar(currentHealth, maxHealth);
            }
            else
            {
                Debug.LogError("DÝKKAT: VitalsManager sahnede bulunamadý!");
            }
        }

        // Hasar yazýsý (Popup) çýkarma sistemi
        if (damagePopupPrefab != null)
        {
            GameObject popup = Instantiate(damagePopupPrefab, hitPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(amount);
        }

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " öldü.");

        // Eðer ölen karakter oyuncuysa oyunu bitir, düþmansa yok et
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("OYUN BÝTTÝ - YENÝDEN BAÞLATILIYOR...");
            // Ýleride buraya Death Screen (Ölüm Ekraný) ekleyeceðiz.
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
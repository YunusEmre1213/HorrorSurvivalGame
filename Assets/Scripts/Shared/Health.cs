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
        currentHealth -= amount;

        
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
       
        Destroy(gameObject);

        
    }
}
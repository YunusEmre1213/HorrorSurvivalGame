using UnityEngine;

public class KeyPickup : MonoBehaviour, IInteractable
{
    [Header("Anahtar Ayarlarý")]
    public string keyName = "BekciAnahtari"; // Kapýnýn aradýðý isimle AYNI olmalý

    [Header("Ses ve Efekt")]
    public AudioClip pickupSound; // Alýnma sesi (Þýngýrtý)

    // IInteractable arayüzünden gelen etkileþim fonksiyonu
    public void Interact()
    {
        // 1. ADIM: Anahtarý "Envantere" ekle.
        // Gerçek bir envanter sistemi kurana kadar, Player'ýn üzerinde bu anahtarý tuttuðuna dair basit bir sistem kullanacaðýz.
        // Þimdilik bunu Player'ýn tag'ini kullanarak "Player" objesini bulup ona haber vererek yapalým.

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Player'ýn üzerindeki basit bir "Anahtarlýk" sistemine haber ver
            PlayerKeyring keyring = player.GetComponent<PlayerKeyring>();
            if (keyring != null)
            {
                keyring.AddKey(keyName);
                Debug.Log("Anahtar alýndý: " + keyName);
            }
            else
            {
                Debug.LogWarning("Player üzerinde PlayerKeyring scripti bulunamadý!");
            }
        }

        // 2. ADIM: Ses Çal (Opsiyonel)
        if (pickupSound != null)
        {
            // Obje yok olmadan önce sesi sahnenin merkezinde çal
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // 3. ADIM: Obseyi Sahneden Sil
        Destroy(gameObject);
    }
}
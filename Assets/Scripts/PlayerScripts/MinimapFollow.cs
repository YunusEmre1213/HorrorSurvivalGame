using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    [Header("Referanslar")]
    public Transform playerTarget;

    [Header("Ayarlar")]
    public float cameraHeight = 20f; // Haritanýn yerden yüksekliði

    // Neden Update deðil de LateUpdate?
    // Staj Notu: Karakter hareket kodlarý Update içinde çalýþýr. Eðer kamerayý da Update içinde 
    // hareket ettirirsek, bazen kamera karakterden önce hareket edip titreme (stutter) yaratabilir.
    // LateUpdate her zaman tüm hareketler bittikten SONRA, karenin en sonunda çalýþýr. 
    // Kamera takibi için sektör standardý LateUpdate kullanmaktýr.
    void LateUpdate()
    {
        if (playerTarget != null)
        {
            // Kameranýn X ve Z pozisyonu oyuncuyu takip etsin, Y yüksekliði (tepeden bakýþ) sabit kalsýn.
            Vector3 newPosition = playerTarget.position;
            newPosition.y = playerTarget.position.y + cameraHeight;

            transform.position = newPosition;

            // Eðer oyuncu döndüðünde haritanýn da onunla dönmesini istersen aþaðýdaki yorum satýrýný açabilirsin. 
            // Ancak FPS oyunlarýnda haritanýn kuzeyi sabit tutmasý daha yaygýndýr.
            // transform.rotation = Quaternion.Euler(90f, playerTarget.eulerAngles.y, 0f);
        }
    }
}
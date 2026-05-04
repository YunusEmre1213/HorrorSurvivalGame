using UnityEngine;
using System.Collections;

public class TuzakKapi : MonoBehaviour
{
    [Header("Kapý Ayarlarý")]
    public Transform hareketliKapi; // Kapanacak asýl kapý
    public Transform kapaliPozisyonNoktasi; // Kapýnýn çarpacaðý yer
    public float kapanmaHizi = 30f; // Hýz (Güm diye inmesi için yüksek tuttum)

    [Header("Ses Ayarlarý")]
    public AudioSource kapiSesi;
    public AudioClip kapiCarpmaSesi; // Yüksek sesli metal çarpma sesi

    // Görünmez tetikleyicimiz bu fonksiyonu çaðýracak
    public void KapiyiGumlet()
    {
        StartCoroutine(KapanisRutini());
    }

    IEnumerator KapanisRutini()
    {
        // Kapý, hedefine çok yaklaþana kadar hýzla hareket etsin
        while (Vector3.Distance(hareketliKapi.position, kapaliPozisyonNoktasi.position) > 0.01f)
        {
            hareketliKapi.position = Vector3.MoveTowards(hareketliKapi.position, kapaliPozisyonNoktasi.position, kapanmaHizi * Time.deltaTime);
            yield return null;
        }

        // Kapý tam hedefe oturduðunda sesi patlat!
        hareketliKapi.position = kapaliPozisyonNoktasi.position;
        if (kapiSesi != null && kapiCarpmaSesi != null)
        {
            kapiSesi.PlayOneShot(kapiCarpmaSesi);
        }
    }
}
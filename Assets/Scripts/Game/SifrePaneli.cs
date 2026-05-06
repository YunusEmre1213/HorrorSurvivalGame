using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

public class SifrePaneli : MonoBehaviour
{
    [Header("Oyuncu Kontrolü Dondurma")]
    public MonoBehaviour oyuncuKontrolKodu; // Karakterin yürüme/bakma kodunu buraya baðlayacaðýz

    [Header("Þifre Ayarlarý")]
    public string dogruSifre = "0047";
    private string girilenSifre = "";
    public int maksimumHane = 4;

    [Header("Arayüz")]
    public TextMeshProUGUI ekranYazisi;
    public Color normalRenk = Color.red;

    [Header("Olaylar (Eventler)")]
    public UnityEvent sifreDogruUyarisi;
    public UnityEvent sifreYanlisUyarisi;

    private bool cezaAktif = false;

    void OnEnable()
    {
        // Fareyi görünür yap
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // PANEL AÇILINCA KARAKTERÝ DONDUR
        if (oyuncuKontrolKodu != null)
        {
            oyuncuKontrolKodu.enabled = false;
        }
    }

    void OnDisable()
    {
        // Fareyi gizle ve oyuna dön
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        // PANEL KAPANINCA KARAKTERÝ SERBEST BIRAK
        if (oyuncuKontrolKodu != null)
        {
            oyuncuKontrolKodu.enabled = true;
        }

        // BUG ÇÖZÜMÜ: Panel aniden kapanýrsa þifreyi arka planda sýfýrla ki kilitlenmesin!
        girilenSifre = "";
        if (ekranYazisi != null)
        {
            ekranYazisi.text = "----";
            ekranYazisi.color = normalRenk;
        }
    }

    public void RakamEkle(string rakam)
    {
        if (girilenSifre.Length < maksimumHane)
        {
            girilenSifre += rakam;
            EkraniGuncelle();
        }
    }

    public void Temizle()
    {
        girilenSifre = "";
        EkraniGuncelle();
    }

    public void SifreyiKontrolEt()
    {
        if (girilenSifre == dogruSifre)
        {
            SifreBasarili();
        }
        else
        {
            if (!cezaAktif) StartCoroutine(HataSistemi());
        }
    }

    void SifreBasarili()
    {
        cezaAktif = false;
        StopAllCoroutines();

        ekranYazisi.text = "ONAYLANDI";
        ekranYazisi.color = Color.green;

        sifreDogruUyarisi.Invoke();
    }

    void EkraniGuncelle()
    {
        if (girilenSifre == "") ekranYazisi.text = "----";
        else ekranYazisi.text = girilenSifre;
    }

    IEnumerator HataSistemi()
    {
        cezaAktif = true;
        ekranYazisi.text = "HATA!";
        ekranYazisi.color = Color.red;

        sifreYanlisUyarisi.Invoke();

        yield return new WaitForSeconds(2f);

        girilenSifre = "";
        ekranYazisi.color = normalRenk;
        EkraniGuncelle();
    }
}
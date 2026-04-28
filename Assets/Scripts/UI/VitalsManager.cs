using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class VitalsManager : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public PsychologyManager psychoManager;
    public TextMeshProUGUI subjectText;
    public Image healthBarFill;
    public Image sanityBarFill;
    public Image batteryBarFill;
    public TextMeshProUGUI batteryCountText;
    public TextMeshProUGUI ammoText;

    [Header("Glitch (Bozulma) Mesajlarý")]
    public string normalName = "SUBJECT #047";
    public string[] scaryMessages = { "UYAAAAN", "YALAN SOYLUYORLAR", "KACAMAZSIN", "DENEY: BASARISIZ", "ONLARI DUYUYORUM" };

    [Header("Renk Ayarlarý")]
    public Color normalSanityColor = Color.cyan; // Psikoloji için Mavi
    public Color dangerColor = Color.red; // Tehlike (Can ve Düþük Psikoloji) için Kýrmýzý
    public Color normalTextColor = Color.white; // Yazý rengi

    private bool isGlitching = false;

    void Start()
    {
        subjectText.text = normalName;
        subjectText.color = normalTextColor;

        // Oyun baþladýðýnda can barýný otomatik olarak Kýrmýzý yapalým
        if (healthBarFill != null)
        {
            healthBarFill.color = dangerColor;
        }
    }

    void Update()
    {
        UpdateBars();

        // Glitch (Bozulma) Sistemini Kontrol Et
        if (psychoManager != null)
        {
            float psychoPercent = psychoManager.currentPsycho / psychoManager.maxPsycho;

            // Psikoloji %50'nin altýna indiyse ve o an glitch olmuyorsa, rastgele glitch baþlat!
            if (psychoPercent <= 0.5f)
            {
                if (!isGlitching)
                {
                    if (Random.Range(0, 100) < 2)
                    {
                        StartCoroutine(GlitchRoutine());
                    }
                }
            }
        }
    }

    private void UpdateBars()
    {
        // Sadece Psikoloji Barýný ve Ekran Renklerini Güncelle (Can barýný Health scripti güncelliyor)
        if (psychoManager != null)
        {
            float psychoPercent = psychoManager.currentPsycho / psychoManager.maxPsycho;
            sanityBarFill.fillAmount = psychoPercent;

            // Psikoloji 50'nin altýna indikçe mavi bar ve beyaz yazý yavaþ yavaþ kýrmýzýya dönsün
            if (psychoPercent <= 0.5f)
            {
                sanityBarFill.color = Color.Lerp(dangerColor, normalSanityColor, psychoPercent * 2f);
                subjectText.color = Color.Lerp(dangerColor, normalTextColor, psychoPercent * 2f);
            }
            else
            {
                sanityBarFill.color = normalSanityColor;
                subjectText.color = normalTextColor;
            }
        }
    }

    // Ekran yazýsýný kýsa süreliðine bozan o efsanevi zamanlayýcý
    IEnumerator GlitchRoutine()
    {
        isGlitching = true;

        string creepyMsg = scaryMessages[Random.Range(0, scaryMessages.Length)];
        subjectText.text = creepyMsg;

        yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));

        subjectText.text = normalName;

        yield return new WaitForSeconds(Random.Range(1f, 3f));

        isGlitching = false;
    }

    // Dýþarýdan Health (Can) scripti tarafýndan çaðrýlacak fonksiyon
    public void UpdateHealthBar(float currentHp, float maxHp)
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = currentHp / maxHp;
        }
    }

    public void UpdateBatteryBar(float currentBattery, float maxBattery)
    {
        if (batteryBarFill != null)
        {
            batteryBarFill.fillAmount = currentBattery / maxBattery;

            
            if (currentBattery / maxBattery <= 0.2f)
                batteryBarFill.color = Color.red;
            else
                batteryBarFill.color = Color.yellow; 
        }
    }

    public void UpdateInventoryUI(int batteryCount)
    {
        if (batteryCountText != null)
        {
            batteryCountText.text = batteryCount.ToString();
        }
    }

    public void UpdateAmmoText(int currentAmmo, int reserveAmmo)
    {
        if (ammoText != null)
        {
            // Ekranda "7 / 15" formatýnda yazdýrýr
            ammoText.text = currentAmmo + " / " + reserveAmmo;
        }
    }

}
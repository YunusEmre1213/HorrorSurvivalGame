using UnityEngine;
using System.Collections;

public class AlarmManager : MonoBehaviour
{
    [Header("Iþýk Ayarlarý")]
    public Light[] beyazIsiklar;
    public Light kirmiziAlarmIsigi;
    public float yanipSonmeHizi = 0.5f;

    [Header("Ses Ayarlarý")]
    public AudioSource sirenSesi;

  
    [Header("Kapý Kýrma Sistemi")]
    public KapiKirici kapiKiriciSistemi;

    private bool alarmDevrede = false;

    public void KaosuBaslat()
    {
        alarmDevrede = true;

     
        foreach (Light isik in beyazIsiklar)
        {
            if (isik != null) isik.enabled = false;
        }

      
        if (sirenSesi != null) sirenSesi.Play();

     
        if (kirmiziAlarmIsigi != null)
        {
            kirmiziAlarmIsigi.gameObject.SetActive(true);
            StartCoroutine(KirmiziFlasorRutin());
        }

       
        if (kapiKiriciSistemi != null)
        {
            kapiKiriciSistemi.SekansiBaslat();
        }
    }

    IEnumerator KirmiziFlasorRutin()
    {
        while (alarmDevrede)
        {
            kirmiziAlarmIsigi.enabled = !kirmiziAlarmIsigi.enabled;
            yield return new WaitForSeconds(yanipSonmeHizi);
        }
    }

    public void AlarmiSustur()
    {
        alarmDevrede = false;
        if (sirenSesi != null) sirenSesi.Stop();
        if (kirmiziAlarmIsigi != null) kirmiziAlarmIsigi.enabled = false;
    }
}
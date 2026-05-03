using UnityEngine;
using System.Collections;

public class KapiKirici : MonoBehaviour
{
    [Header("Fizik Ayarlarý")]
    public Rigidbody kapiRigidbody;
    public float firlatmaGucu = 70f; 

    [Header("Ses Ayarlarý")]
    public AudioSource sesKaynagi;
    public AudioClip gumSesi; 
    public AudioClip kirilmaSesi; 

   
    public void SekansiBaslat()
    {
        StartCoroutine(KapiKirmaRutini());
    }

    IEnumerator KapiKirmaRutini()
    {
      
        if (gumSesi != null) sesKaynagi.PlayOneShot(gumSesi);
        yield return new WaitForSeconds(1.5f); 

        
        if (gumSesi != null) sesKaynagi.PlayOneShot(gumSesi);
        yield return new WaitForSeconds(1.0f);

       
        if (gumSesi != null) sesKaynagi.PlayOneShot(gumSesi);
        yield return new WaitForSeconds(0.5f);

      
        if (kirilmaSesi != null) sesKaynagi.PlayOneShot(kirilmaSesi);

       
        kapiRigidbody.isKinematic = false;

     
        kapiRigidbody.AddForce(-transform.forward * firlatmaGucu, ForceMode.Impulse);


        kapiRigidbody.AddTorque(new Vector3(10f, 20f, 0f), ForceMode.Impulse);
    }
}
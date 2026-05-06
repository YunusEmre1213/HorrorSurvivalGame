using UnityEngine;
using System.Collections;

public class KirmiziFlasor : MonoBehaviour
{
    private Light isikBileseni;
    public float yanipSonmeHizi = 0.15f; 

    void Awake()
    {
        isikBileseni = GetComponent<Light>();
    }

    
    void OnEnable()
    {
        StartCoroutine(FlasorRutini());
    }

    IEnumerator FlasorRutini()
    {
        while (true) 
        {
            isikBileseni.enabled = !isikBileseni.enabled; 
            yield return new WaitForSeconds(yanipSonmeHizi); 
        }
    }
}
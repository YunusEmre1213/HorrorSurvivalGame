using UnityEngine;
using System.Collections;

public class TitreyenIsik : MonoBehaviour
{
    private Light isik;

    public float minYogunluk = 0f; 
    public float maxYogunluk = 50f;
    public float maksimumBekleme = 0.2f; 

    void Start()
    {
        isik = GetComponent<Light>();
        StartCoroutine(Titre());
    }

    IEnumerator Titre()
    {
        while (true)
        {
            isik.intensity = Random.Range(minYogunluk, maxYogunluk);
            yield return new WaitForSeconds(Random.Range(0.01f, maksimumBekleme));
        }
    }
}
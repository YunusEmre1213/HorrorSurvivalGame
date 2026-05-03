using UnityEngine;
using UnityEngine.Events;

public class AlanTetikleyici : MonoBehaviour
{
    [Header("Oyuncu Bu Alana Girince Ne Olsun?")]
    public UnityEvent bolgeyeGirince;

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            bolgeyeGirince.Invoke(); 
            Destroy(gameObject); 
        }
    }
}
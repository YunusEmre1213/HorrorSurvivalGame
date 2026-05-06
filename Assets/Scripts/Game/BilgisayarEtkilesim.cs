using UnityEngine;
using UnityEngine.InputSystem;

public class BilgisayarEtkilesim : MonoBehaviour
{
    [Header("Açýlacak Ekran (Canvas)")]
    public GameObject bilgisayarCanvas; 

    private bool oyuncuYakinmi = false;

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinmi = true;
        }
    }

    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oyuncuYakinmi = false;
            bilgisayarCanvas.SetActive(false); 
        }
    }

    void Update()
    {
       
        if (oyuncuYakinmi && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            bilgisayarCanvas.SetActive(true); 
        }
    }
}
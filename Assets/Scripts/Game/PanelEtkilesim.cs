using UnityEngine;
using UnityEngine.InputSystem; 

public class PanelEtkilesim : MonoBehaviour
{
    [Header("Açýlacak Arayüz")]
    public GameObject keypadCanvas; 

    private bool alandaMi = false;

   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alandaMi = true;
        }
    }

   
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            alandaMi = false;
            keypadCanvas.SetActive(false);
        }
    }

    void Update()
    {
      
        if (alandaMi && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
           
            bool suAnkiDurum = keypadCanvas.activeSelf;
            keypadCanvas.SetActive(!suAnkiDurum);
        }

       
        if (keypadCanvas.activeSelf && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            keypadCanvas.SetActive(false);
        }
    }
}
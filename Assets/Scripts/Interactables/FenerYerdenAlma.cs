using UnityEngine;
using UnityEngine.InputSystem; 

public class FenerYerdenAlma : MonoBehaviour
{
    [Header("Karakterin Feneri")]
    public GameObject oyuncununFeneri;

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
        }
    }

    void Update()
    {
        
        if (alandaMi == true && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            oyuncununFeneri.SetActive(true);

            Destroy(gameObject);
        }
    }
}
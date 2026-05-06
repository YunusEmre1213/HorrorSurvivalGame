using UnityEngine;
using UnityEngine.EventSystems;

public class BilgisayarEkrani : MonoBehaviour
{
    [Header("Oyuncu Kontrolü Dondurma")]
    public MonoBehaviour oyuncuKontrolKodu;

    void OnEnable()
    {
       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

       
        if (oyuncuKontrolKodu != null)
        {
            oyuncuKontrolKodu.enabled = false;
        }
    }

    void OnDisable()
    {
       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

       
        if (oyuncuKontrolKodu != null)
        {
            oyuncuKontrolKodu.enabled = true;
        }
    }

   
    public void EkraniKapat()
    {
        gameObject.SetActive(false); 
    }
}
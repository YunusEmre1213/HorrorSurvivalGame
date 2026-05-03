using UnityEngine;
using TMPro;
using UnityEngine.Events; // 1. EKLEME: Evrensel olaylarý kullanabilmek için bu kütüphane þart

public class NotArayuzu : MonoBehaviour
{
    [Header("Bu Kaðýdýn Hikayesi")]
    [TextArea(5, 10)]
    public string hikayeMetni;

    [Header("Arayüz Baðlantýlarý")]
    public GameObject notPaneli;
    public TextMeshProUGUI ekrandakiYaziObjesi;

    [Header("Olaylar")]
    public UnityEvent onNotKapatildi; 

    private bool acikMi = false;

    public void NotuAcKapat()
    {
        acikMi = !acikMi;

        if (acikMi == true)
        {
            if (ekrandakiYaziObjesi != null)
            {
                ekrandakiYaziObjesi.text = hikayeMetni;
            }

            notPaneli.SetActive(true);

            InventoryManager envanter = FindFirstObjectByType<InventoryManager>();
            if (envanter != null)
            {
                envanter.ToplananNotuEkle(hikayeMetni);
            }

            if (GetComponent<MeshRenderer>() != null)
            {
                GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            notPaneli.SetActive(false);

            onNotKapatildi.Invoke();

            
            Destroy(gameObject, 0.2f);
        }
    }
}
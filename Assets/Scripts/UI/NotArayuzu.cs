using UnityEngine;
using TMPro;

public class NotArayuzu : MonoBehaviour
{
    [Header("Bu Kaðýdýn Hikayesi")]
    [TextArea(5, 10)] 
    public string hikayeMetni;

    [Header("Arayüz Baðlantýlarý")]
    public GameObject notPaneli;
    public TextMeshProUGUI ekrandakiYaziObjesi; 

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
            Destroy(gameObject); 
        }
    }
}
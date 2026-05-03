using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

public enum ItemType { Battery, Bandage, PistolAmmo }

public class InventoryManager : MonoBehaviour
{
    [Header("Envanter Verisi")]
    public Dictionary<ItemType, int> inventory = new Dictionary<ItemType, int>();

    [Header("UI (Arayüz) Ayarlarý")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI batteryCountText;

    [Header("Referanslar")]
    public FlashlightController flashlight;

    [Header("Not Defteri")]
    public List<string> toplananNotlar = new List<string>();

    [Header("Not Okuma Arayüzü")]
    public TextMeshProUGUI envanterNotYazisi; 
    private int guncelNotIndeksi = 0; 

    private bool isInventoryOpen = false;

    void Start()
    {
        // Baþlangýçta cepler 0
        inventory.Add(ItemType.Battery, 0);
        inventory.Add(ItemType.Bandage, 0);
        inventory.Add(ItemType.PistolAmmo, 0);

        if (inventoryPanel != null) inventoryPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (inventoryPanel != null) inventoryPanel.SetActive(isInventoryOpen);

        if (isInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GuncelNotuGoster();
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        UpdateUI();
    }

    public void AddItem(ItemType type, int amount)
    {
        if (inventory.ContainsKey(type)) inventory[type] += amount;
        else inventory.Add(type, amount);

        Debug.Log(type.ToString() + " eklendi. Toplam: " + inventory[type]);
        UpdateUI();
    }

    
    public int GetItemCount(ItemType type)
    {
        return inventory.ContainsKey(type) ? inventory[type] : 0;
    }

   
    public bool UseItem(ItemType type)
    {
        if (inventory.ContainsKey(type) && inventory[type] > 0)
        {
            inventory[type]--;
            UpdateUI();
            return true;
        }
        return false;
    }

   

    public void OnUseBatteryButtonClicked()
    {
        if (inventory.ContainsKey(ItemType.Battery) && inventory[ItemType.Battery] > 0)
        {
            if (flashlight != null && flashlight.currentBattery < flashlight.maxBattery)
            {
                flashlight.ReloadBatteryFromUI();
                inventory[ItemType.Battery]--;
                UpdateUI();
                Debug.Log("Pil menüden kullanýldý!");
            }
            else
            {
                Debug.Log("Fenerin pili zaten dolu!");
            }
        }
        else
        {
            Debug.Log("Envanterinde hiç pil yok!");
        }
    }

    public void UpdateUI()
    {
        if (batteryCountText != null)
        {
            batteryCountText.text = "Pil: " + inventory[ItemType.Battery].ToString();
        }
    }

    public void ToplananNotuEkle(string notMetni)
    {
       
        if (!toplananNotlar.Contains(notMetni))
        {
            toplananNotlar.Add(notMetni);
            Debug.Log("Yeni bir not envantere eklendi! Toplam Not: " + toplananNotlar.Count);
        }
    }

    public void GuncelNotuGoster()
    {
        if (envanterNotYazisi != null)
        {
            if (toplananNotlar.Count > 0)
            {
                
                envanterNotYazisi.text = toplananNotlar[guncelNotIndeksi];
            }
            else
            {
                envanterNotYazisi.text = "Henüz bir not bulunamadý...";
            }
        }
    }

   
    public void SonrakiNot()
    {
        if (toplananNotlar.Count == 0) return; 

        guncelNotIndeksi++;

        
        if (guncelNotIndeksi >= toplananNotlar.Count) guncelNotIndeksi = 0;

        GuncelNotuGoster();
    }

   
    public void OncekiNot()
    {
        if (toplananNotlar.Count == 0) return;

        guncelNotIndeksi--; 

        
        if (guncelNotIndeksi < 0) guncelNotIndeksi = toplananNotlar.Count - 1;

        GuncelNotuGoster();
    }

}
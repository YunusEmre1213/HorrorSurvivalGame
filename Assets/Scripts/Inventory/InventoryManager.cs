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

    // --- ÝÞTE EKSÝK OLAN VE SÝLAHA HATA VERDÝREN FONKSÝYONLAR ---

    // Silahtaki mermiyi kontrol etmek için "Kaç tane var?" diye soran fonksiyon
    public int GetItemCount(ItemType type)
    {
        return inventory.ContainsKey(type) ? inventory[type] : 0;
    }

    // Þarjör doldururken envanterden mermi silen fonksiyon
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

    // -------------------------------------------------------------

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
}
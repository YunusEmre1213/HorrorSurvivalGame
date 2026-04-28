using UnityEngine;
using UnityEngine.InputSystem;

public class BatteryPickup : MonoBehaviour
{
    [Header("Ayarlar")]
    public float pickupRange = 2.5f;

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupRange)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        // Oyuncunun cebini (Envanteri) bul
        InventoryManager inventory = player.GetComponent<InventoryManager>();

        if (inventory != null)
        {
            // 1 adet Pil ekle
            inventory.AddItem(ItemType.Battery, 1);
            Destroy(gameObject);
        }
    }
}
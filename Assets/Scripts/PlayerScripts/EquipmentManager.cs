using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    [Header("Ekipman Objeleri (3D Modeller)")]
    public GameObject gunModel;
    public GameObject flashlightModel;

    [Header("Flashlight Iþýk Objesi")]
    public Light flashlightLight; // Inspector'dan Flashlight objesini sürükle
    public int currentEquipIndex = 1;

    [Header("Flashlight Controller")]
    public FlashlightController flashlightController; // YENÝ

    void Start()
    {
        EquipItem(1);
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            EquipItem(0);
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            EquipItem(1);
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            EquipItem(-1);
    }

    private void EquipItem(int index)
    {
        if (gunModel != null) gunModel.SetActive(false);
        if (flashlightModel != null) flashlightModel.SetActive(false);

        // Silaha veya boþ ele geçince feneri zorla kapat
        if (flashlightController != null) flashlightController.ForceTurnOff(); // YENÝ

        if (index == 0 && gunModel != null)
        {
            gunModel.SetActive(true);
        }
        else if (index == 1 && flashlightModel != null)
        {
            flashlightModel.SetActive(true);
            // Not: Feneri otomatik açmýyoruz, oyuncu F'ye bassýn
        }

        currentEquipIndex = index;
    }
}
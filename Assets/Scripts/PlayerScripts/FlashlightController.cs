using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Referanslar")]
    public Light flashlight;
    public PsychologyManager psychoManager;
    public InputActionReference toggleAction;

    [Header("Pil Ayarlarý")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float batteryDrainRate = 2.5f;

    private bool isFlashlightOn = false;
    private VitalsManager vitals;
    private EquipmentManager equipmentManager; // YENÝ

    void Start()
    {
        vitals = FindAnyObjectByType<VitalsManager>();
        equipmentManager = GetComponentInParent<EquipmentManager>(); // YENÝ
        if (psychoManager == null) psychoManager = GetComponentInParent<PsychologyManager>();
        if (flashlight != null) flashlight.enabled = isFlashlightOn;
        if (psychoManager != null) psychoManager.SetFlashlightState(isFlashlightOn);
    }

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnToggleInput;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnToggleInput;
            toggleAction.action.Disable();
        }
    }

    private void OnToggleInput(InputAction.CallbackContext context)
    {
        // YENÝ: Sadece fener elde ise toggle çalýþsýn
        if (equipmentManager != null && equipmentManager.currentEquipIndex != 1)
            return;

        ToggleLight();
    }

    void Update()
    {
        if (isFlashlightOn)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime;
            if (vitals != null) vitals.UpdateBatteryBar(currentBattery, maxBattery);
            if (currentBattery <= 0)
            {
                currentBattery = 0;
                ForceTurnOff();
            }
        }
    }

    void ToggleLight()
    {
        if (flashlight == null) return;
        if (!isFlashlightOn && currentBattery <= 0)
        {
            Debug.LogWarning("Pil bitti! TAB'a basarak envanterden pil kullan.");
            return;
        }
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn;
        if (psychoManager != null) psychoManager.SetFlashlightState(isFlashlightOn);
    }

    // EquipmentManager silaha geçince çaðýrýr
    public void ForceTurnOff()
    {
        isFlashlightOn = false;
        if (flashlight != null) flashlight.enabled = false;
        if (psychoManager != null) psychoManager.SetFlashlightState(false);
    }

    public void ReloadBatteryFromUI()
    {
        currentBattery = maxBattery;
        if (vitals != null) vitals.UpdateBatteryBar(currentBattery, maxBattery);
        Debug.Log("Fener UI üzerinden þarj edildi!");
    }
}
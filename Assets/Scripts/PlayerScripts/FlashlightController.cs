using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [Header("Referanslar")]
    public Light flashlight; // Fenerimizin ýþýk bileþeni
    public InputActionReference toggleAction; // F tuþu girdisi

    private bool isFlashlightOn = true; // Baþlangýçta açýk mý kapalý mý olsun?

    void Start()
    {
        // Fener objesini bulamadýysak hata vermemesi için Null Check
        if (flashlight != null)
        {
            flashlight.enabled = isFlashlightOn;
        }
    }

    void Update()
    {
        // Eðer F tuþuna basýldýysa
        if (toggleAction.action.WasPressedThisFrame())
        {
            ToggleLight();
        }
    }

    void ToggleLight()
    {
        if (flashlight == null) return;

        // Durumu tersine çevir (Açýksa kapat, kapalýysa aç)
        isFlashlightOn = !isFlashlightOn;
        flashlight.enabled = isFlashlightOn;

        // Staj Notu: Ýleride buraya fenerin "týk" açýlma sesi eklenebilir. (AudioSource.PlayOneShot)
    }

    private void OnEnable()
    {
        toggleAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleAction.action.Disable();
    }
}
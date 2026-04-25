using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Silah Ayarlarý")]
    public float bulletSpeed = 50f; // Merminin fýrlama hýzý

    [Header("Referanslar")]
    public GameObject bulletPrefab; // Fýrlatýlacak mermi þablonu
    public Transform muzzlePoint;  // Merminin çýkacaðý namlu ucu
    public InputActionReference fireAction;

    void Update()
    {
        // Sol týklandýysa
        if (fireAction.action.WasPressedThisFrame())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Debug.Log("Fiziksel Mermi Fýrlatýldý!");

        // 1. Mermiyi namlu ucunda oluþtur
        GameObject bullet = Instantiate(bulletPrefab, muzzlePoint.position, muzzlePoint.rotation);

        // 2. Merminin Rigidbody bileþenini al
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // 3. Mermiye namlunun baktýðý yöne (forward) doðru hýz (force) uygula
        if (rb != null)
        {
            rb.AddForce(muzzlePoint.forward * bulletSpeed, ForceMode.Impulse);
        }
    }

    private void OnEnable()
    {
        fireAction.action.Enable();
    }

    private void OnDisable()
    {
        fireAction.action.Disable();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    [Header("Silah ve Hasar Ayarlarý")]
    public float damage = 25f; // Düþmana vereceði hasar
    public float range = 100f; // Merminin gidebileceði maksimum mesafe

    [Header("Þarjör Ayarlarý")]
    public int maxMagazineSize = 7; // Þarjör kapasitesi
    public int currentAmmo; // Þu an silahtaki mermi

    [Header("Referanslar")]
    public Transform playerCamera; // Niþangahýn (ekranýn ortasýnýn) baktýðý yönü almak için
    public InputActionReference fireAction;

    // Efektler (Opsiyonel - Ýleride eklenecek)
    // public ParticleSystem muzzleFlash; 
    // public GameObject hitEffectPrefab; 

    private InventoryManager inventoryManager;
    private VitalsManager vitals;

    void Start()
    {
        inventoryManager = GetComponentInParent<InventoryManager>();
        vitals = FindAnyObjectByType<VitalsManager>();

        // Eðer kamerayý sürüklemeyi unutursan otomatik bulsun
        if (playerCamera == null) playerCamera = Camera.main.transform;

        // Oyuna baþlarken þarjör dolu baþlasýn
        currentAmmo = maxMagazineSize;
        UpdateAmmoUI();
    }

    // YENÝ INPUT SÝSTEMÝ (Týklamayý Kaçýrmamasý Ýçin Garantili Yöntem)
    private void OnEnable()
    {
        if (fireAction != null)
        {
            fireAction.action.Enable();
            fireAction.action.performed += OnShootInput;
        }
    }

    private void OnDisable()
    {
        if (fireAction != null)
        {
            fireAction.action.performed -= OnShootInput;
            fireAction.action.Disable();
        }
    }

    private void OnShootInput(InputAction.CallbackContext context)
    {
        // Eðer oyun duraklatýlmýþsa (Pause) veya envanter açýksa ateþ etme!
        if (Time.timeScale == 0) return;

        Shoot();
    }

    void Update()
    {
        // R TUÞU ÝLE ÞARJÖR DEÐÝÞTÝRME
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        // 1. Mermi var mý kontrol et
        if (currentAmmo <= 0)
        {
            Debug.Log("Mermi Bitti! Þarjör deðiþtirmek için R'ye bas.");
            // Ýleride buraya "boþ tetik (týk)" sesi gelecek
            return;
        }

        // 2. Mermiyi eksilt ve arayüzü güncelle
        currentAmmo--;
        UpdateAmmoUI();

        // if (muzzleFlash != null) muzzleFlash.Play(); // Ateþ efekti

        // 3. RAYCAST (IÞIN) SÝSTEMÝ - Niþangahýn olduðu yere anýnda ateþ et
        RaycastHit hit;

        // Kameranýn tam ortasýndan, ileriye doðru görünmez bir ýþýn yolluyoruz
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, range))
        {
            Debug.Log("Vurulan Obje: " + hit.transform.name);

            // Vurduðumuz objede Health (Can) sistemi var mý?
            Health targetHealth = hit.transform.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Hasarý vurulan objeye ilet
                targetHealth.TakeDamage(damage, hit.point);
            }

            // Vuruþ efekti (Kan veya duvardan çýkan toz)
            // if (hitEffectPrefab != null) Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private void Reload()
    {
        if (currentAmmo == maxMagazineSize)
        {
            Debug.Log("Þarjör zaten tam dolu.");
            return;
        }

        if (inventoryManager != null)
        {
            // Cebimizdeki yedek mermi kutularýnýn sayýsýný al
            int reserveAmmo = inventoryManager.GetItemCount(ItemType.PistolAmmo);

            if (reserveAmmo > 0)
            {
                // Kaç mermiye ihtiyacýmýz var? (Örn: 3 mermi kaldýysa, 4 lazým)
                int bulletsNeeded = maxMagazineSize - currentAmmo;

                // Cebimizde o kadar mermi var mý? (Mathf.Min, hangisi küçükse onu seçer)
                int bulletsToLoad = Mathf.Min(bulletsNeeded, reserveAmmo);

                // Envanterden kullandýðýmýz kadar mermiyi sil
                for (int i = 0; i < bulletsToLoad; i++)
                {
                    inventoryManager.UseItem(ItemType.PistolAmmo);
                }

                // Silahtaki mermiyi artýr
                currentAmmo += bulletsToLoad;
                Debug.Log("Þarjör Deðiþtirildi! Yeni mermi: " + currentAmmo);
                UpdateAmmoUI();
            }
            else
            {
                Debug.Log("Cebinde hiç yedek mermi yok!");
            }
        }
    }

    // Her mermi atýþýnda veya R'ye basýþta ekraný (UI) güncelleyen fonksiyon
    public void UpdateAmmoUI()
    {
        if (vitals != null && inventoryManager != null)
        {
            int reserveAmmo = inventoryManager.GetItemCount(ItemType.PistolAmmo);
            vitals.UpdateAmmoText(currentAmmo, reserveAmmo);
        }
    }
}
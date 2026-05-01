using UnityEngine;

public class SimpleDoor : MonoBehaviour, IInteractable
{
    [Header("Kapý Ayarlarý")]
    public float openAngle = 90f;
    public float smoothSpeed = 1.5f;

    [Header("Kilit Sistemi")]
    public bool isLocked = false; // Kapý baþlangýçta kilitli mi?
    public string requiredKeyName = "BekciAnahtari"; // Bu kapýyý hangi anahtar açar?

    [Header("Sesler (Opsiyonel)")]
    // Ýleride ses eklemek istersen buralarý kullanacaðýz
    public AudioClip lockedSound; // Kapý zorlama sesi
    public AudioClip unlockSound; // Kilit açýlma sesi (Þýk-þýk)

    private bool isOpen = false;
    private Quaternion defaultRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;
    private AudioSource audioSource;

    void Start()
    {
        defaultRotation = transform.localRotation;
        openRotation = Quaternion.Euler(defaultRotation.eulerAngles + new Vector3(0, openAngle, 0));
        targetRotation = defaultRotation;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // IInteractable arayüzünden gelen etkileþim fonksiyonu
    public void Interact()
    {
        // 1. ADIM: Kapý kilitli mi kontrol et
        if (isLocked)
        {
            TryToUnlock();
            return; // Eðer kilitliyse aþaðýya (açýlma koduna) geçmesini engelle
        }

        // 2. ADIM: Kapý kilitli deðilse normal þekilde Aç/Kapat
        ToggleDoor();
    }

    private void TryToUnlock()
    {
        // 1. Oyuncuyu bul
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // 2. Oyuncunun üzerindeki anahtarlýðý al
            PlayerKeyring keyring = player.GetComponent<PlayerKeyring>();

            // 3. Anahtarlýkta bu kapýnýn istediði anahtar var mý bak
            if (keyring != null && keyring.HasKey(requiredKeyName))
            {
                isLocked = false; // Kilidi aç
                Debug.Log("Kapý kilidi açýldý!");
                if (audioSource && unlockSound) audioSource.PlayOneShot(unlockSound);
                ToggleDoor();
            }
            else
            {
                // Anahtar yoksa
                Debug.Log("Bu kapý kilitli. Ýhtiyacýn olan: " + requiredKeyName);
                if (audioSource && lockedSound) audioSource.PlayOneShot(lockedSound);
            }
        }
    }

    private void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            targetRotation = openRotation;
        }
        else
        {
            targetRotation = defaultRotation;
        }
    }
}
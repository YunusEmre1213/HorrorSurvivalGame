using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [Header("Ses Ayarlarý")]
    public AudioClip[] footstepClips; // Adým sesleri dizisi
    public AudioSource audioSource;

    [Header("Zamanlama")]
    public float stepInterval = 0.5f; // Ýki adým arasýndaki saniye (Yürüme hýzýna göre ayarlanýr)
    private float stepTimer;

    [Header("Referanslar")]
    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        // Eðer AudioSource atanmadýysa otomatik bulalým
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        Vector3 horizontalVelocity = new Vector3(characterController.velocity.x, 0, characterController.velocity.z);

     
        if (characterController.isGrounded && horizontalVelocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0)
            {
                PlayFootstepSound();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0;
        }
    }

    void PlayFootstepSound()
    {
        if (footstepClips.Length == 0) return;

        // 2. Rastgele bir ses seç
        int randomIndex = Random.Range(0, footstepClips.Length);
        AudioClip clip = footstepClips[randomIndex];

        // 3. Profesyonellik Dokunuþu: Random Pitch (Sesin perdesini hafifçe deðiþtir)
        // Bu sayede ayný ses bile her seferinde farklý duyulur, robotiklik hissi kaybolur.
        audioSource.pitch = Random.Range(0.85f, 1.15f);
        audioSource.volume = Random.Range(0.7f, 1.0f);

        // Sesi çal
        audioSource.PlayOneShot(clip);
    }
}
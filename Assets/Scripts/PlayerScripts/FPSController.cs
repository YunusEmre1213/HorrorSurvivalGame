using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Kamera ve Hareket Ayarlarý")]
    public Camera playerCamera;
    public float walkSpeed = 5f;
    public float lookSpeed = 2f;
    public float lookXLimit = 85f;
    public float jumpPower = 5f;
    public float gravity = 9.81f;

    [Header("Input Action Referanslarý")]
    public InputActionReference moveAction;
    public InputActionReference lookAction;
    public InputActionReference jumpAction;

    [Header("Ses Efektleri")]
    public AudioClip landSound; // Yere basma (Landing) sesi
    public AudioSource audioSource;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    // Profesyonel Dokunuþ: Karakter bir önceki karede havada mýydý?
    private bool wasGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        // Oyun baþladýðý anki durumu hafýzaya al
        wasGrounded = characterController.isGrounded;
    }

    void Update()
    {

        if (PauseManager.IsGamePaused) return;

        // 1. HAREKET (WASD)
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float curSpeedX = walkSpeed * moveInput.y;
        float curSpeedY = walkSpeed * moveInput.x;
        float movementDirectionY = moveDirection.y;

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // 2. ZIPLAMA (Sesi buradan kaldýrdýk)
        if (jumpAction.action.triggered && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // 3. YERÇEKÝMÝ
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Karakteri Hareket Ettir
        characterController.Move(moveDirection * Time.deltaTime);

        // 4. YERE DÜÞME (LANDING) SESÝ KONTROLÜ
        // Eðer bir saniye önce havadaysak (!wasGrounded) ve þu an yerdeysek (isGrounded)
        if (!wasGrounded && characterController.isGrounded)
        {
            if (landSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(landSound);
            }
        }

        // Hafýzayý güncelle: Þu anki durumu bir sonraki kare için "eski durum" olarak kaydet
        wasGrounded = characterController.isGrounded;

        // 5. KAMERA KONTROLÜ (Mouse)
        Vector2 lookInput = lookAction.action.ReadValue<Vector2>();
        rotationX += -lookInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, lookInput.x * lookSpeed, 0);
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();
    }
}
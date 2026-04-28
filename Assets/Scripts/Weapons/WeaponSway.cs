using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway Ayarlarý (Farenin Dönüþü)")]
    public float swayAmount = 0.02f; // Silah farenin tersine ne kadar kaysýn
    public float maxSwayAmount = 0.06f; // Kayma sýnýrý
    public float swaySmoothAmount = 6f; // Geri yerine gelme yumuþaklýðý

    [Header("Bobbing Ayarlarý (Yürüme Sekmesi)")]
    public float bobbingSpeed = 14f; // Adým atma (sekme) hýzý
    public float bobbingAmount = 0.05f; // Silahýn ne kadar yukarý/aþaðý sekeceði

    private Vector3 initialPosition;
    private float timer = 0f;

    void Start()
    {
        // Objenin ilk baþladýðý yeri hafýzaya al
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // 1. SWAY (Fare hareketiyle silahýn geride kalýp savrulmasý)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float moveX = -mouseDelta.x * swayAmount;
        float moveY = -mouseDelta.y * swayAmount;

        // Abartýlý kaymalarý engellemek için sýnýrla
        moveX = Mathf.Clamp(moveX, -maxSwayAmount, maxSwayAmount);
        moveY = Mathf.Clamp(moveY, -maxSwayAmount, maxSwayAmount);

        Vector3 targetPosition = new Vector3(moveX, moveY, 0f);

        // 2. BOBBING (Yürürken silahýn adým ritmiyle sekmesi)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.sKey.isPressed ||
            Keyboard.current.aKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            // Karakter yürüyorsa sinüs dalgasý (Sonsuzluk iþareti gibi) çizerek silahý salla
            timer += Time.deltaTime * bobbingSpeed;
            float bobY = Mathf.Sin(timer) * bobbingAmount; // Yukarý aþaðý
            float bobX = Mathf.Cos(timer * 0.5f) * bobbingAmount * 0.5f; // Saða sola hafif

            targetPosition += new Vector3(bobX, bobY, 0f);
        }
        else
        {
            // Karakter duruyorsa timer'ý yavaþça sýfýrla ki silah düzgünce merkeze otursun
            timer = 0f;
        }

        // Orijinal pozisyonla hedef pozisyonu birleþtir ve Smooth (Yumuþak) bir geçiþ yap
        Vector3 finalPosition = initialPosition + targetPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * swaySmoothAmount);
    }
}
using UnityEngine;

public class SimpleDoor : MonoBehaviour, IInteractable
{
    [Header("Kapý Ayarlarý")]
    public float openAngle = 90f; // Kapýnýn ne kadar açýlacaðý (Ters açýlýyorsa -90 yapabilirsin)
    public float smoothSpeed = 1.5f; // Açýlma hýzý (Korku oyunu için düþük tutuyoruz)

    private bool isOpen = false;
    private Quaternion defaultRotation;
    private Quaternion openRotation;
    private Quaternion targetRotation;

    void Start()
    {
        // Kapýnýn sahnede durduðu ilk orijinal açýyý hafýzaya al (Kapalý hali)
        defaultRotation = transform.localRotation;

        // Açýk halinin açýsýný hesapla (Y ekseninde openAngle kadar ekle)
        openRotation = Quaternion.Euler(defaultRotation.eulerAngles + new Vector3(0, openAngle, 0));

        // Ýlk baþta kapý kapalý olduðu için hedefimiz de kapalý hali
        targetRotation = defaultRotation;
    }

    void Update()
    {
        // Kapýnýn mevcut açýsýný, hedef açýya doðru belirlediðimiz hýzda yumuþakça (Slerp) çevir
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    public void Interact()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            // Kapý açýldý, hedefi açýk açý olarak belirle
            targetRotation = openRotation;
        }
        else
        {
            // Kapý kapandý, hedefi kapalý (orijinal) açý olarak belirle
            targetRotation = defaultRotation;
        }
    }
}
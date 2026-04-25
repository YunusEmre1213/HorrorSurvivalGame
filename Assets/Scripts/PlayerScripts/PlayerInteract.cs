using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [Header("Etkileþim Ayarlarý")]
    public Camera playerCamera;
    public float interactDistance = 3f; // Oyuncunun kolunun uzanma mesafesi

    [Header("Girdi Ayarý")]
    public InputActionReference interactAction;

    void Update()
    {
        // Eðer oyuncu "E" tuþuna basarsa
        if (interactAction.action.triggered)
        {
            // Ekranýn tam ortasýndan ileriye doðru bir ýþýn (Ray) oluþtur
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Iþýn ileriye fýrlatýldýðýnda bir þeye çarpýyor mu ve menzil (3 metre) içinde mi?
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                // Çarptýðýmýz objede "IInteractable" þablonu var mý diye kontrol et
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                // Eðer varsa, o objenin Interact (Etkileþim) fonksiyonunu çalýþtýr
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }
}
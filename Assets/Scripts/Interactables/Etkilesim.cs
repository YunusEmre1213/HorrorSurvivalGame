using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events; 

public class Etkilesim : MonoBehaviour
{
    [Header("Etkileþim Ayarlarý")]
    public float etkilesimMesafesi = 2.5f;
    public UnityEvent onInteract; 

    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        if (player == null) return;

        // Senin mükemmel mesafe ölçüm kodun
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= etkilesimMesafesi)
        {
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                
                onInteract.Invoke();
            }
        }
    }
}
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float disappearTime = 0.8f;

    private TextMeshProUGUI textMesh;
    private Transform cameraTransform; 

    public void Setup(float damageAmount)
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMesh.SetText(damageAmount.ToString());

        
        cameraTransform = Camera.main.transform;

        Destroy(gameObject, disappearTime);
    }

    void Update()
    {
       
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

     
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
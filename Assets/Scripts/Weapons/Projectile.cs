using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [Header("Mermi Ayarlarý")]
    public float lifeTime = 3f; 
    public float damage = 25f; 

    void Start()
    {
        
        Destroy(gameObject, lifeTime);
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        
        Health targetHealth = collision.transform.GetComponent<Health>();

        if (targetHealth != null)
        {

            targetHealth.TakeDamage(damage, collision.contacts[0].point);
        }

       
        Destroy(gameObject);
    }
}
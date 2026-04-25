using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    // 1. Yeni durumumuzu ekledik
    public enum EnemyState { Patrolling, Chasing, Attacking }

    [Header("Durum Yönetimi")]
    public EnemyState currentState = EnemyState.Patrolling;

    [Header("Devriye Ayarlarý")]
    public List<Transform> waypoints;
    public float waitTimeAtWaypoint = 2f;
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;

    [Header("Takip Ayarlarý")]
    public float detectionRange = 8f;
    public float stopDistance = 1.5f;

    [Header("Saldýrý Ayarlarý")]
    public float attackRange = 2f; // Oyuncuya vurabilme mesafesi
    public float attackDamage = 20f; // Vereceði hasar
    public float attackCooldown = 1.5f; // Ýki vuruþ arasýndaki saniye
    private float lastAttackTime = 0f; // Son vuruþ zamanýný hafýzada tutar

    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth; // Oyuncunun can scriptini önbelleðe alýyoruz

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Oyuncunun üzerindeki Health bileþenini bul ve kaydet (Optimizasyon)
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
        }

        if (waypoints.Count > 0)
            agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {

        if (player == null)
        {
            agent.isStopped = true;
            return; 
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 2. Durum Makinesi Geçiþleri (Artýk Saldýrý da var)
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        else if (distanceToPlayer <= detectionRange)
        {
            currentState = EnemyState.Chasing;
        }
        else if (currentState == EnemyState.Chasing && distanceToPlayer > detectionRange + 2f)
        {
            currentState = EnemyState.Patrolling;
            GoToNextWaypoint();
        }

        // 3. Mevcut duruma göre ilgili fonksiyonu çalýþtýr
        switch (currentState)
        {
            case EnemyState.Patrolling:
                PatrolLogic();
                break;
            case EnemyState.Chasing:
                ChaseLogic();
                break;
            case EnemyState.Attacking:
                AttackLogic();
                break;
        }
    }

    void PatrolLogic()
    {
        if (waypoints.Count == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = 0f;
            }

            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtWaypoint)
            {
                isWaiting = false;
                GoToNextWaypoint();
            }
        }
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Count == 0) return;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void ChaseLogic()
    {
        isWaiting = false;
        agent.SetDestination(player.position);
        agent.isStopped = agent.remainingDistance <= stopDistance;
    }

    // 4. Yeni Saldýrý Mantýðý
    void AttackLogic()
    {
        // Saldýrýrken olduðu yerde dursun ve karakterimize dönsün
        agent.isStopped = true;

        // Y ekseninde bize bakmasýný saðla (Yukarý/aþaðý eðilmemesi için)
        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * 5f);

        // Bekleme süresi dolduysa VUR!
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (playerHealth != null)
            {
                // Hasarý oyuncuya ilet
                playerHealth.TakeDamage(attackDamage, transform.position);
                Debug.Log("Düþman sana vurdu! " + attackDamage + " hasar aldýn.");
            }

            // Son vuruþ zamanýný þu anki zaman olarak güncelle
            lastAttackTime = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Saldýrý menzilini kýrmýzýyla göster
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if (waypoints != null && waypoints.Count > 1)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < waypoints.Count; i++)
            {
                int next = (i + 1) % waypoints.Count;
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
            }
        }
    }
}
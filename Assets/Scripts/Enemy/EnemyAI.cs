using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
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

    [Header("Saldýrý ve Hýrçýnlýk Ayarlarý")]
    public float attackRange = 2f;
    public float baseAttackDamage = 20f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    // Dinamik hýz ayarlarý
    public float baseSpeed = 3.5f;
    public float maxSpeed = 6.5f;

    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth;
    private PsychologyManager playerPsycho;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<Health>();
            playerPsycho = playerObj.GetComponent<PsychologyManager>();
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

        // DÝNAMÝK ZORLUK (Hýrçýnlýk) HESAPLAMASI
        float aggroMultiplier = 1f;
        if (playerHealth != null && playerPsycho != null)
        {
            float healthPercent = playerHealth.currentHealth / playerHealth.maxHealth;
            float psychoPercent = playerPsycho.currentPsycho / playerPsycho.maxPsycho;

            aggroMultiplier = 1f + ((1f - healthPercent) * 0.5f) + ((1f - psychoPercent) * 0.5f);
        }

        agent.speed = Mathf.Clamp(baseSpeed * aggroMultiplier, baseSpeed, maxSpeed);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

        switch (currentState)
        {
            case EnemyState.Patrolling:
                PatrolLogic();
                break;
            case EnemyState.Chasing:
                ChaseLogic();
                break;
            case EnemyState.Attacking:
                AttackLogic(aggroMultiplier);
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

    void AttackLogic(float currentAggro)
    {
        agent.isStopped = true;

        Vector3 lookPos = player.position - transform.position;
        lookPos.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), Time.deltaTime * 5f);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (playerHealth != null)
            {
                float dynamicDamage = baseAttackDamage * currentAggro;

                playerHealth.TakeDamage(dynamicDamage, transform.position);
                Debug.Log("Düþman Hýrçýnlýðý: " + currentAggro + "x | Verilen Hasar: " + dynamicDamage);
            }

            lastAttackTime = Time.time;
        }
    }

    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

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
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Transform targetObject;
    private NavMeshAgent agent;             // NavMeshAgent để điều hướng

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5f;

    // Attacking
    public float timeBetweenAttacks = 0.5f;
    bool alreadyAttacked;
    public GameObject projectile;

    // States
    public float sightRange = 300f, attackRange = 5f;
    public bool playerInSightRange, playerInAttackRange;

    public LayerMask whatIsGround, whatIsPlayer;

    private void Start()
    {
        // Tự động tìm và gán NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Tự động tìm đối tượng PlayerPosition
        if (targetObject == null)
        {
            GameObject playerObject = GameObject.Find("PlayerPosition");
            if (playerObject != null)
            {
                targetObject = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (targetObject == null) return;

        //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        playerInSightRange = (Vector3.Distance(transform.position, targetObject.position) < sightRange);
        playerInAttackRange = (Vector3.Distance(transform.position, targetObject.position) < attackRange);


        Debug.Log("Player in attack range: " + playerInAttackRange);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(targetObject.position);
    }

    private void AttackPlayer()
    {
        // Kiểm tra nếu player đã ra ngoài phạm vi tấn công
        //if (Vector3.Distance(transform.position, targetObject.position) > attackRange)
        //{
        //    Debug.Log("Player out of attack range");
        //    playerInAttackRange = false;
        //    return; // Thoát khỏi AttackPlayer nếu player không còn trong attackRange
        //}

        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(targetObject);

        if (!alreadyAttacked)
        {
            // Code tấn công
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Debug.Log("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    
}

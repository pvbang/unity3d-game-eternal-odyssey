using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform targetObject;
    private NavMeshAgent agent;
    private Rigidbody rb;

    private EnemyDamageHandle enemyDamageHandle;
    public EnemyAtribute enemyAtribute;

    // Patroling
    private Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5f;

    // Attacking
    public float timeBetweenAttacks = 0.5f;
    private bool alreadyAttacked;

    // States
    public float sightRange = 40f;
    public float attackRange = 5f;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public float distance = 0.0f;


    private void Start()
    {
        // Initialize components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyDamageHandle = GetComponent<EnemyDamageHandle>();
        enemyAtribute = GetComponent<EnemyAtribute>();

        // Configure Rigidbody
        rb.isKinematic = false; // Để Rigidbody xử lý va chạm
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Ngăn rotation không cần thiết

        // Assign layers
        if (playerLayer == 0) playerLayer = LayerMask.GetMask("PlayerPosition");
        if (groundLayer == 0) groundLayer = LayerMask.GetMask("Default");

        // Auto-find player if not assigned
        if (targetObject == null)
        {
            GameObject playerObject = GameObject.Find("PlayerPosition");
            if (playerObject != null) targetObject = playerObject.transform;
        }
    }

    private void Update()
    {
        if (targetObject == null) return;

        distance = Vector3.Distance(transform.position, targetObject.position);

        playerInSightRange = distance < sightRange;
        playerInAttackRange = distance < attackRange;

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        //if (playerInSightRange)
        //{
        //    if (EnemyCountManage.Instance.activeEnemies.Count == 1)
        //    {
        //        EnemyAtributeUI.Instance.SetActiveEnemyHP();
        //        EnemyCountManage.Instance.SetEnemyDistance(distance);
        //        EnemyAtributeUI.Instance.UpdateEnemyHP(enemyAtribute.getCurrentHealth(), enemyAtribute.getMaxHealth());
        //    }
        //    // Nếu có nhiều quái, kiểm tra khoảng cách
        //    else if (EnemyCountManage.Instance.GetEnemyDistance() <= distance && EnemyCountManage.Instance.GetEnemyDistance() != float.MaxValue)
        //    {
        //        return; 
        //    }

        //    // Hiển thị thanh máu nếu quái gần nhất hoặc là quái duy nhất
        //    EnemyAtributeUI.Instance.SetActiveEnemyHP();
        //    EnemyCountManage.Instance.SetEnemyDistance(distance);
        //    EnemyAtributeUI.Instance.UpdateEnemyHP(enemyAtribute.getCurrentHealth(), enemyAtribute.getMaxHealth());
        //}
        //else
        //{
        //    // Reset khoảng cách nếu quái rời xa tầm nhìn
        //    EnemyAtributeUI.Instance.SetDeactiveEnemyHP();
        //    EnemyCountManage.Instance.SetEnemyDistance(float.MaxValue); // Reset lại khoảng cách lớn nhất
        //}
    }

    private void FixedUpdate()
    {
        // Sync Rigidbody with NavMeshAgent
        if (agent != null)
        {
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.MovePosition(agent.nextPosition); 
        }
    }


    // HP Controller
    public void TakeDamage(int damage)
    {
        enemyAtribute.setCurrentHealth(enemyAtribute.getCurrentHealth() - damage);
        if (enemyAtribute.getCurrentHealth() <= 0)
        {
            Destroy(gameObject);
        }
    }


    // Đi tùm lum
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }


    // Chạy tùm lum
    //private void SearchWalkPoint()
    //{
    //    // Calculate random point in range
    //    float randomZ = Random.Range(-walkPointRange, walkPointRange);
    //    float randomX = Random.Range(-walkPointRange, walkPointRange);

    //    walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    //    if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
    //        walkPointSet = true;
    //}
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 randomPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, walkPointRange, NavMesh.AllAreas))
        {
            walkPoint = hit.position;
            walkPointSet = true;
        }
    }

    // Đuổi theo player
    private void ChasePlayer()
    {
        agent.speed = enemyAtribute.getRunSpeed();
        agent.SetDestination(targetObject.position);
    }

    // Tấn công player
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(targetObject);

        if (!alreadyAttacked)
        {
            // Trigger attack logic here
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void OnEnable()
    {
        if (EnemyCountManage.Instance != null)
        {
            EnemyCountManage.Instance.AddEnemy(this);
        }
    }

    void OnDisable()
    {
        if (EnemyCountManage.Instance != null)
        {
            EnemyCountManage.Instance.RemoveEnemy(this);
        }
    }
 

}

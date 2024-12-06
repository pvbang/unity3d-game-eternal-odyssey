using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Object mà Enemy chạy tới")]
    public Transform targetObject;
    private NavMeshAgent agent;
    private Rigidbody rb;

<<<<<<< HEAD
    [Header("Quản lý Animation")]
    public EnemyAnimationController enemyAnimatorController;

    [Header("Quản lý Damage")]
    public EnemyDamageHandle enemyDamageHandle;

    [Header("Quản lý Thuộc tính")]
    public EnemyAtribute enemyAtribute;

    [Header("Quản lý Skill")]
    public EnemySkillController enemySkillController;

    // States
    [Header("Khoảng cách phát hiện TagetObject")]
    public float sightRange = 40f;
    private bool playerInSightRange;
    private bool playerInAttackRange;

    // Attacking
    [Header("Tấn công nếu có TagetObject trong tầm đánh")]
    public float attackRange = 5f;
    public float timeBetweenAttacks = 0.5f;
    private bool alreadyAttacked;

    // Patroling
    [Header("Đi tùm lum nếu không có TagetObject xung quanh")]
    public float walkPointRange = 5f;
    private Vector3 walkPoint;
    bool walkPointSet;

    private LayerMask groundLayer;
    private LayerMask playerLayer;

    // Khoảng cách hiện tại từ Enemy đến TargetObject
    [Header("Khoảng cách hiện tại từ Enemy đến TargetObject")]
=======
    public EnemyDamageHandle enemyDamageHandle;
    public EnemyAtribute enemyAtribute;

    public EnemySkillController enemySkillController;

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

>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
    public float distance = 0.0f;


    private void Start()
    {
        // Initialize components
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

<<<<<<< HEAD
        if (enemyAnimatorController == null)    enemyAnimatorController = GetComponent<EnemyAnimationController>();
        if (enemyDamageHandle == null)          enemyDamageHandle = GetComponent<EnemyDamageHandle>();
        if (enemyAtribute == null)              enemyAtribute = GetComponent<EnemyAtribute>();
        if (enemySkillController == null)       enemySkillController = GetComponent<EnemySkillController>();
=======
        if (enemyDamageHandle == null)
            enemyDamageHandle = GetComponent<EnemyDamageHandle>();
        if (enemyAtribute == null)
            enemyAtribute = GetComponent<EnemyAtribute>();
        if (enemySkillController == null)
            enemySkillController = GetComponent<EnemySkillController>();
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821

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

        if (EnemyCountManage.Instance != null)
        {
            EnemyCountManage.Instance.AddEnemy(this);
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
<<<<<<< HEAD
=======

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
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
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


    // Đi tùm lum
    private void Patroling()
    {
<<<<<<< HEAD
        enemyAnimatorController.StopAnimation("all");

=======
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

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
<<<<<<< HEAD
        enemyAnimatorController.PlayAnimation("run");
=======
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
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
            alreadyAttacked = true;
<<<<<<< HEAD
            float delayTime = enemySkillController.Attack(enemyAtribute.getDamageFrom(), enemyAtribute.getDamageTo(), enemyAtribute.getCritical());
            Invoke(nameof(ResetAttack), delayTime + enemyAtribute.getAttackSpeed());
        }
    }


=======

            enemySkillController.Attack(enemyAtribute.getDamageFrom(), enemyAtribute.getDamageTo(), enemyAtribute.getCritical());
            Invoke(nameof(ResetAttack), enemyAtribute.getAttackSpeed());
        }
    }

>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821
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
 
<<<<<<< HEAD
        
=======
>>>>>>> 4db4d9d19cf2dad80f9858d824700e79419ea821

}

using System.Collections;
using UnityEngine;

public class EnemyBehaviourFlying : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float alertDistance = 8;
    [SerializeField] private float dampTime = 1.75f;
    [SerializeField] private Transform[] navPoints;

    [Header("Combat")]
    [SerializeField] private float attackResetTime = 1.2f;
    [SerializeField] private float attackRadomVarienceRange = 0.5f;
    [SerializeField] private float attackRange = 1.3f;
    [SerializeField] private int attackPower = 50;

    public bool isFollowingPlayer = false; //Flags that the enemy is following the player
    public bool isDead;

    private Transform enemyTrans;
    private Transform playerTrans;
    private RectTransform enemyHealthTrans;
    private Animator animator;
    private Vector2 targetPos; //Targeting points to follow left and right of player 
    private Vector2 waypointTargetPos;
    private Vector2 zeroVelocity = Vector2.zero;
    private float startingScale;
    private float startingHealthScale;
    private bool isFacingRight = true;
    private bool canAttack = true;

    private void Awake()
    {
        enemyTrans = GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyHealthTrans = GetComponentInChildren<RectTransform>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        startingScale = enemyTrans.localScale.x; //Sets the local scale of the enemy for reference
        if (enemyHealthTrans) startingHealthScale = enemyHealthTrans.localScale.x; //Sets the local scaled of the enemy health for reference
    }

    //Uses the position of the player to smoothly move the enemy towards it's target
    private void Update()
    {
        if (GameManager.Instance.IsFrozen || isDead) return;

        var dist = CheckPlayerDistance(); //Calcuates the distance from the player and decides if the enemy will keep following

        if (isFollowingPlayer)
        {
            CalculateFollowPlayer(); //Calculates the target to follow player
            if (canAttack) Attack(); //If it's close enough to attack, do so
        }
        else CheckIfArrived();

        Move(); //Moves the enemy
    }

    private float CheckPlayerDistance()
    {
        var dist = Vector3.Distance(playerTrans.position, enemyTrans.position);

        if (dist < alertDistance) isFollowingPlayer = true;
        else isFollowingPlayer = false;

        return dist;
    }

    private void CheckIfArrived()
    {
        if (Vector3.Distance(waypointTargetPos, enemyTrans.position) < 0.5f)
            waypointTargetPos = navPoints[Random.Range(0, navPoints.Length)].position;
    }

    private void CalculateFollowPlayer() => targetPos = new Vector2(playerTrans.position.x, playerTrans.position.y + 0.5f);

    private void Move()
    {
        float targetX;
        if (isFollowingPlayer) targetX = targetPos.x;
        else targetX = waypointTargetPos.x;
        
        //Flips the character to the appropriate direction depending on the target position
        if (targetX - enemyTrans.position.x > 0 && !isFacingRight) Flip();
        else if (targetX - enemyTrans.position.x < 0 && isFacingRight) Flip();

        if (isFollowingPlayer)
            transform.position = Vector2.SmoothDamp(gameObject.transform.position, targetPos, ref zeroVelocity, dampTime);
        else
            transform.position = Vector2.SmoothDamp(gameObject.transform.position, waypointTargetPos, ref zeroVelocity, dampTime * 1.25f);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        if (isFacingRight)
        {
            enemyTrans.localScale = new Vector3(startingScale, startingScale, 1);
            if (enemyHealthTrans) enemyHealthTrans.localScale = new Vector3(startingHealthScale, startingHealthScale, 1);
        }
        else
        {
            enemyTrans.localScale = new Vector3(-startingScale, startingScale, 1);
            if (enemyHealthTrans) enemyHealthTrans.localScale = new Vector3(-startingHealthScale, startingHealthScale, 1);
        }
    }
    private void Attack()
    {
        StartCoroutine(WaitForNextAttack());

        if (animator != null) animator.SetTrigger("attack"); //Plays the attack animation

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(enemyTrans.position, attackRange);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].gameObject.CompareTag("Player"))
                enemiesToDamage[i].GetComponent<HealthController>().ChangeHealth(-attackPower);
        }
    }

    private IEnumerator WaitForNextAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackResetTime + Random.Range(0, attackRadomVarienceRange));
        canAttack = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var actor = collision.gameObject;

        if (actor.CompareTag("Player"))
            actor.GetComponent<HealthController>().ChangeHealth(-attackPower);
    }

    private void OnDrawGizmosSelected() //Shows attack range in scene view
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

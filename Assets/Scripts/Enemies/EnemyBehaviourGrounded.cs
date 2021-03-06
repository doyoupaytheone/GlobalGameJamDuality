using System.Collections;
using UnityEngine;

public class EnemyBehaviourGrounded : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackResetTime = 1.5f;
    [SerializeField] private float attackRadomVarienceRange = 1.5f;
    [SerializeField] private int attackPower = 50;

    public bool isDead;

    private Transform enemyTrans;
    private RectTransform enemyHealthTrans;
    private Animator animator;
    private float startingScale;
    private float startingHealthScale;
    private bool canAttack = true;
    private bool isFacingRight = true;
    private bool isStopped;

    private void Awake()
    {
        enemyTrans = GetComponent<Transform>();
        enemyHealthTrans = GetComponentInChildren<RectTransform>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        startingScale = enemyTrans.localScale.x; //Sets the local scale of the enemy for reference
        if (enemyHealthTrans) startingHealthScale = enemyHealthTrans.localScale.x; //Sets the local scaled of the enemy health for reference
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsFrozen || !isDead)
            CheckEnvironment();
    }

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen || !isDead)
        {
            if (!isStopped) MoveForward();
        }
    }

    private void CheckEnvironment()
    {
        //Sets the direction of the raycast depending on the direction that it's currently traveling
        var direction = 1;
        if (!isFacingRight) direction = -1;
        
        //Checks what object is in front of it with raycast
        var objectCheck = new Vector2(enemyTrans.position.x + 1 * direction, enemyTrans.position.y);
        RaycastHit2D hit = Physics2D.Raycast(objectCheck, Vector2.right * direction, 0.25f);
        Debug.DrawLine(enemyTrans.position, hit.point, Color.red);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                isStopped = true;
                if (canAttack) Attack();
            }
            else if (!hit.collider.gameObject.CompareTag("Projectile")) Flip();
        }
        else if (isStopped) isStopped = false;

        //Checks if there is ground front of it with raycast
        var groundCheck = new Vector2(enemyTrans.position.x + 1.5f * direction, enemyTrans.position.y);
        RaycastHit2D hitGround = Physics2D.Raycast(groundCheck, Vector2.down, 1.25f);
        Debug.DrawLine(groundCheck, hitGround.point, Color.green);
        if (hitGround.collider == null) Flip();
    }

    private void MoveForward()
    {
        if(isFacingRight) enemyTrans.Translate(Vector2.right * movementSpeed * Time.deltaTime);
        else enemyTrans.Translate(Vector2.left * movementSpeed * Time.deltaTime);
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

    private void OnDrawGizmosSelected() //Shows attack range in scene view
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

using UnityEngine;

public class EnemyBehaviourFlying : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float hoverDistance = 0.75f;
    [SerializeField] private float alertDistance = 8;
    [SerializeField] private float dampTime = 1.75f;
    [SerializeField] private Transform[] navPoints;

    [Header("Combat")]
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float attackResetTime = 1.5f;
    [SerializeField] private float attackRadomVarienceRange = 1.5f;
    [SerializeField] private int attackPower = 50;

    public bool isFollowingPlayer = false; //Flags that the enemy is following the player

    private Transform enemyTrans;
    private Transform playerTrans;
    private RectTransform enemyHealthTrans;
    private Vector2 targetPos; //Targeting points to follow left and right of player 
    private Vector2 waypointTargetPos;
    private Vector2 zeroVelocity = Vector2.zero;
    private float startingScale;
    private float startingHealthScale;
    private bool canAttack = true;
    private bool isFacingRight;

    private void Awake()
    {
        enemyTrans = GetComponent<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyHealthTrans = GetComponentInChildren<RectTransform>();
    }

    private void Start()
    {
        startingScale = enemyTrans.localScale.x; //Sets the local scale of the enemy for reference
        if (enemyHealthTrans) startingHealthScale = enemyHealthTrans.localScale.x; //Sets the local scaled of the enemy health for reference

        InvokeRepeating("AssignNewHoverHeight", 2, 2);
    }

    //Uses the position of the player to smoothly move the enemy towards it's target
    private void Update()
    {
        if (GameManager.Instance.IsFrozen) return;

        CheckPlayerDistance(); //Calcuates the distance from the player and decides if the enemy will keep following

        if (isFollowingPlayer) //If following the player
        {
            CalculateFollowPlayer(); //Calculates the target to follow player
            CalculateHoverHeight(); //Calculates hover height
        }
        else CheckIfArrived();

        Move(); //Moves the enemy
    }

    private void CheckPlayerDistance()
    {
        if (Vector3.Distance(playerTrans.position, enemyTrans.position) < alertDistance) isFollowingPlayer = true;
        else isFollowingPlayer = false;
    }

    private void CheckIfArrived()
    {
        if (Vector3.Distance(waypointTargetPos, enemyTrans.position) < 0.5f)
            waypointTargetPos = navPoints[Random.Range(0, navPoints.Length)].position;
    }

    private void CalculateFollowPlayer()
    {
        targetPos = new Vector2(playerTrans.position.x, playerTrans.position.y + 1);

        //Flips the character to the appropriate direction depending on the target position
        if (targetPos.x > 0 && !isFacingRight) Flip();
        else if (targetPos.x < 0 && isFacingRight) Flip();
    }

    private void CalculateHoverHeight() => targetPos += new Vector2(0, hoverDistance);

    private void Move()
    {
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
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var actor = collision.gameObject;

        if (actor.CompareTag("Player"))
            actor.GetComponent<HealthController>().ChangeHealth(-attackPower);
    }
}

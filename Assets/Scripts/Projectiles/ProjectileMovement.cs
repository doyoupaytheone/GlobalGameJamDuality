using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 40;
    public float maxDistance = 15f;
    public int damagePower = 25;
    public int direction = 1;
    public bool isStuck;
    public LayerMask staticsLayer;

    [SerializeField] private Sprite throwStar;
    [SerializeField] private Sprite wallStar;
    [SerializeField] private ParticleSystem poofEffect;
    [SerializeField] private Transform spriteTrans;

    private Transform projectileTrans;
    private Transform playerTrans;
    private RotateItem rotateItem;
    private SpriteRenderer spriteRenderer;
    private Collider2D stuckInStaticObject;
    private ParticleSystem thisPoof;
    private Vector2 startingPosition;
    private Vector2 relativeDirectionOfCollision;

    private void OnDisable() => StartProjectile();

    private void Awake()
    {
        projectileTrans = GetComponent<Transform>();
        spriteTrans = GetComponentInChildren<Transform>();
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rotateItem = GetComponentInChildren<RotateItem>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckIfStillStuck();
        
        if (Vector2.Distance(projectileTrans.position, playerTrans.position) > maxDistance) //Checks to see if the player has left the star (if in wall, etc.)
            TurnOffProjecile();

        if (GameManager.Instance.IsFrozen || isStuck) return;

        Collider2D checkForCollision = Physics2D.OverlapCircle(projectileTrans.position, 0.15f, staticsLayer);
        if (checkForCollision != null && !checkForCollision.isTrigger)
        {
            stuckInStaticObject = checkForCollision;
            relativeDirectionOfCollision = checkForCollision.transform.position - projectileTrans.position;
            StopProjectile();
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsFrozen || isStuck) return;

        projectileTrans.Translate(Vector3.right * Time.deltaTime * speed);
    }

    private void CheckIfStillStuck()
    {
        Collider2D checkForCollision = Physics2D.OverlapCircle(projectileTrans.position, 0.2f, staticsLayer);
        if (checkForCollision == stuckInStaticObject) return;

        StartProjectile();
    }

    public void StopProjectile()
    {
        //projectileTrans.rotation = Quaternion.Euler(Vector3.zero);
        //spriteTrans.rotation = Quaternion.Euler(Vector3.zero);
        //spriteRenderer.sprite = wallStar;
        rotateItem.isRotatingZ = false;
        isStuck = true;
    }

    public void StartProjectile()
    {
        rotateItem.isRotatingZ = true;
        isStuck = false;
        //spriteRenderer.sprite = throwStar;
        //spriteTrans.rotation = Quaternion.Euler(Vector3.zero);
        //projectileTrans.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void ResetFirePosition() => startingPosition = projectileTrans.position;

    private void TurnOffProjecile()
    {
        if (thisPoof == null) thisPoof = Instantiate(poofEffect, projectileTrans.position, Quaternion.identity);
        else thisPoof.transform.position = projectileTrans.position;

        thisPoof.Play();
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isStuck) //Puts the star back into the player's inventory if it's stuck
            TurnOffProjecile();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.GetComponent<HealthController>().ChangeHealth(-damagePower);
            TurnOffProjecile();
        }

        if (collision.gameObject.CompareTag("Trigger"))
        {
            collision.gameObject.GetComponent<ProjectileTrigger>().TriggerEffect();
            TurnOffProjecile();
        }
    }
}

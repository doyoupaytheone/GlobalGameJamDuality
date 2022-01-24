using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10;
    public float maxDistance = 0.2f;
    public int damagePower = 25;

    private Transform projectileTrans;
    private Vector2 startingPosition;

    private void Awake() => projectileTrans = GetComponent<Transform>();

    private void LateUpdate()
    {
        projectileTrans.Translate(Vector3.right * Time.deltaTime * speed);

        if (Vector2.Distance(startingPosition, projectileTrans.position) > maxDistance)
            this.gameObject.SetActive(false);
    }

    public void ResetFirePosition() => startingPosition = projectileTrans.position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var health = collision.GetComponent<HealthController>();
        if (health && !collision.CompareTag("Player")) health.ChangeHealth(-damagePower);
    }
}

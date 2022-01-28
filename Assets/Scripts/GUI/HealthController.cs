using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth = 1000;

    public float currentHealth = 1000;
    public bool isLightEnemy;
    public bool isDead;

    private Animator anim;

    private void Awake() => anim = GetComponent<Animator>();

    private void Start()
    {
        SetMaxHealth(maxHealth);
        RestoreHealth();
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        if (slider != null) slider.maxValue = maxHealth;
    }

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
        if (slider != null) slider.value = currentHealth;
    }

    public void ChangeHealth(float changeInHealth)
    {
        if (changeInHealth < 0)
        {
            if (anim != null && this.gameObject.CompareTag("Enemy")) anim.SetTrigger("hurt");
        }

        if (currentHealth + changeInHealth > maxHealth) currentHealth = maxHealth;
        else currentHealth += changeInHealth;

        if (slider != null) slider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (!isDead) DeathSequence();
            isDead = true;
        }
    }

    private void DeathSequence()
    {
        if (this.gameObject.CompareTag("Enemy"))
        {
            var e1 = GetComponent<EnemyBehaviourGrounded>();
            if (e1 != null)
            {
                e1.isDead = true;
                StartCoroutine(WaitForDeathAnimationToFinish(0.5f));
            }

            var e2 = GetComponent<EnemyBehaviourFlying>();
            if (e2 != null)
            {
                e2.isDead = true;
                StartCoroutine(WaitForDeathAnimationToFinish(1));
            }
        }
        else if (this.gameObject.CompareTag("Player"))
            GameManager.Instance.PlayerHasDied();

        if (anim != null) anim.SetTrigger("death");
    }

    private IEnumerator WaitForDeathAnimationToFinish(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        this.gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth = 1000;

    public float currentHealth = 1000;
    public bool isLightEnemy;
    public bool isDead;

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
            this.gameObject.SetActive(false);
        }
        else if (this.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponent<Animator>().SetTrigger("death");
            GameManager.Instance.PlayerHasDied();
        }
    }
}

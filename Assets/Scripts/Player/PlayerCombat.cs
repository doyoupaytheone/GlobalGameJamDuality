//Created by Justin Simmons
//Edited by Akeem Roberts

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Variables for primary attack (Shooting)
    [Header("Primary Attack")]
    [SerializeField] private float primaryAttackCooldown;
    [SerializeField] private int primaryAttackDamage;
    [SerializeField] private GameObject projectile;
    private float primaryAttackCooldownTimer;
    private bool canAttackPrimary = true;

    // All variables below is for secondary attack (Meelee) 
    [Header("Secondary Attack")]
    [SerializeField] private float secondaryAttackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int secondaryAttackDamage;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private Transform attackPos;
    private float secondaryAttackCooldownTimer;
    private bool canAttackSecondary = true;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip throwSoundClip;
    [SerializeField] private AudioClip hitSoundClip;

    [Header("Other Attributes")]
    public bool attackTypeIsLight; // Bool that changes attack type depending on if in light or dark mode

    private Transform playerTrans;
    private Animator animator;
    private PlayerDuality playerDuality;
    private HealthController healthDisplay;
    private List<GameObject> projectilePool = new List<GameObject>();
    private Quaternion lastClickAngle = Quaternion.identity;

    private void Awake()
    {
        playerTrans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        playerDuality = GetComponent<PlayerDuality>();
    }

    private void Start()
    {
        InitializeWeapon(projectile);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen)
        {
            AttackTypeCheck();
            CheckAttackTimers();

            if (canAttackPrimary && Input.GetKeyDown(PlayerPrefData.PrimaryAttack)) PrimaryAttack();
            if (canAttackSecondary && Input.GetKeyDown(PlayerPrefData.SecondaryAttack)) SecondaryAttack();
        }
    }

    private void CheckAttackTimers()
    {
        //Can attack again
        if (primaryAttackCooldownTimer <= 0)
        {
            primaryAttackCooldownTimer = primaryAttackCooldown;
            canAttackPrimary = true;
        }
        //Ticks timer if can't attack
        else if (!canAttackPrimary)
            primaryAttackCooldownTimer -= Time.deltaTime;

        //Can attack again
        if (secondaryAttackCooldownTimer <= 0)
        {
            secondaryAttackCooldownTimer = secondaryAttackCooldown;
            canAttackSecondary = true;
        }
        //Ticks timer if can't attack
        else if (!canAttackSecondary)
            secondaryAttackCooldownTimer -= Time.deltaTime;
    }

    private void InitializeWeapon(GameObject projectilePrefab)
    {
        GameObject bullet;
        for (int i = 0; i < 12; i++)
        {
            bullet = CreateBullet(projectilePrefab);
            bullet.GetComponent<ProjectileMovement>().damagePower = primaryAttackDamage;
            bullet.SetActive(false);
        }
    }

    private void PrimaryAttack()
    {
        //Toggles to can't attack
        canAttackPrimary = false;

        //Finds the direction of the mouse click, calculates the angle between it and the player and makes a proper rotation
        var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var angle = Mathf.Atan2(mp.y - playerTrans.position.y, mp.x - playerTrans.position.x) * 180 / Mathf.PI; 
        lastClickAngle = Quaternion.Euler(0, 0, angle);

        //Creates the projectile
        FireWeapon();
    }

    private void SecondaryAttack() // Enemies require layer mask for this attack 
    {
        canAttackSecondary = false; //Toggles to can't attack
        if (animator != null) animator.SetTrigger("attack"); //Plays the attack animation
        StartCoroutine(WaitToApplyDamageEffects(0.275f)); //Waits a second to let the animation "catch up" to the damage effects
    }

    private GameObject CreateBullet(GameObject bulletPrefab)
    {
        //Creates a new bullet at  the bullet spawner and grabs it's bullet controller script
        GameObject projectile = Instantiate(bulletPrefab, playerTrans.position, Quaternion.identity);

        //Adds the bullets to a pooled list to activate/deactivate and returns that bullet
        projectilePool.Add(projectile);
        return projectile;
    }

    private void FireWeapon()
    {
        //If the weapon is fired, turn on the next bullet in the pool
        foreach (GameObject projectile in projectilePool)
        {
            if (projectile.activeSelf == false)
            {
                if (animator != null) animator.SetTrigger("projectileAttack"); //Plays the projectile attack animation
                if (playerAudio != null) PlayCombatAudio(throwSoundClip); //Plays the projectile attack audio clip

                projectile.transform.position = playerTrans.position;
                projectile.transform.rotation = lastClickAngle;
                var pm = projectile.GetComponent<ProjectileMovement>();
                pm.ResetFirePosition();
                if (playerDuality.dualityTimer == 0) pm.damagePower = (int)(primaryAttackDamage * 0.05f); //Sets normal damage
                else pm.damagePower = (int)(primaryAttackDamage * playerDuality.dualityTimer * 0.05f); //Sets damage depending on duality timer
                projectile.SetActive(true);

                break;
            }
        }
    }

    private void AttackTypeCheck()
    {
        if (playerDuality.isInLightMode == true)
        {
            attackTypeIsLight = true;
        }
        else if (playerDuality.isInLightMode == false)
        {
            attackTypeIsLight = false;
        }
    }

    private void PlayCombatAudio(AudioClip sound)
    {
        if (!playerAudio.isPlaying)
        {
            playerAudio.clip = sound;
            playerAudio.Play();
        }
    }

    private IEnumerator WaitToApplyDamageEffects(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        int dualityDamage; //Stores a calculation for damage depending on the current duality time
        if (playerDuality.dualityTimer == 0) dualityDamage = (int)(secondaryAttackDamage * 0.025f); //Sets normal damage
        else dualityDamage = (int)(secondaryAttackDamage * playerDuality.dualityTimer * 0.025f); //Sets damage depending on duality timer

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<HealthController>().ChangeHealth(-dualityDamage);
            if (playerAudio != null) PlayCombatAudio(hitSoundClip); //Plays the projectile attack audio clip if an enemy is "hit"
        }
    }

    private void OnDrawGizmosSelected() //Shows attack range in scene view
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}

//Created by Justin Simmons
//Edited by Akeem Roberts

using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Variables for primary attack (Shooting)
    [Header("Primary Attack")]
    private float primaryAttackCooldownTimer;
    private bool canAttackPrimary = true;
    [SerializeField] private float primaryAttackCooldown;
    [SerializeField] private int primaryAttackDamage;
    [SerializeField] private GameObject swordProjectile;

    // All variables below is for secondary attack (Meelee) 
    [Header("Secondary Attack")]
    private float secondaryAttackCooldownTimer;
    private bool canAttackSecondary = true;
    [SerializeField] private float secondaryAttackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int secondaryAttackDamage;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private Transform attackPos;

    private Transform playerTrans;
    private PlayerDuality playerDuality;
    private HealthDisplay healthDisplay;
    private List<GameObject> projectilePool = new List<GameObject>();
    private Quaternion lastClickAngle = Quaternion.identity;

    public bool attackTypeIsLight; // Bool that changes attack type depending on if in light or dark mode
    private bool applyDamage;

    private void Awake()
    {
        playerTrans = GetComponent<Transform>();
        playerDuality = GetComponent<PlayerDuality>();
    }

    private void Start()
    {
        InitializeWeapon(swordProjectile);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsFrozen)
        {
            AttackTypeCheck();
            CheckAttackTimers();
            //ApplyDamage();

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
        //Toggles to can't attack
        canAttackSecondary = false;

        int dualityDamage; //Stores a calculation for damage depending on the current duality time
        if (playerDuality.dualityTimer == 0) dualityDamage = (int)(secondaryAttackDamage * 0.05f); //Sets normal damage
        else dualityDamage = (int)(secondaryAttackDamage * playerDuality.dualityTimer * 0.05f); //Sets damage depending on duality timer

        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);

        for (int i = 0; i < enemiesToDamage.Length; i++)
            enemiesToDamage[i].GetComponent<HealthDisplay>().ChangeHealth(-dualityDamage);
    }

    private void OnDrawGizmosSelected() //Shows attack range in scene view
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private GameObject CreateBullet(GameObject bulletPrefab)
    {
        //Creates a new bullet at  the bullet spawner and grabs it's bullet controller script
        GameObject projectile = Instantiate(bulletPrefab, playerTrans);

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

   
}

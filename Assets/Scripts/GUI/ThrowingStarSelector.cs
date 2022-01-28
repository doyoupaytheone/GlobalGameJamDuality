using System.Collections.Generic;
using UnityEngine;

public class ThrowingStarSelector : MonoBehaviour
{
    //Variables for primary attack (Shooting)
    [Header("Throw Sprite")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private float maxDistance = 10;

    [Header("Sounds")]
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private AudioClip throwSoundClip;

    private Transform playerTrans;
    private Animator animator;
    private List<GameObject> projectilePool = new List<GameObject>();
    private Quaternion lastClickAngle = Quaternion.identity;

    private void Awake()
    {
        playerTrans = GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        InitializeWeapon(projectile);
    }

    private void InitializeWeapon(GameObject projectilePrefab)
    {
        GameObject bullet;
        for (int i = 0; i < 12; i++)
        {
            bullet = CreateBullet(projectilePrefab);
            bullet.SetActive(false);
        }
    }

    public void ThrowSelectorStarToClickedPoint()
    {
        //Finds the direction of the mouse click, calculates the angle between it and the player and makes a proper rotation
        var mp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        maxDistance = Vector2.Distance(mp, playerTrans.position);
        var angle = Mathf.Atan2(mp.y - playerTrans.position.y, mp.x - playerTrans.position.x) * 180 / Mathf.PI;
        lastClickAngle = Quaternion.Euler(0, 0, angle);

        //Creates the projectile
        FireWeapon();
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
                pm.maxDistance = maxDistance;
                pm.ResetFirePosition();
                projectile.SetActive(true);

                break;
            }
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
}

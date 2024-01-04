using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBoss : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    public float timeBetweenShooting, timeBetweenShots;
    public float spread = 3f;
    public float bulletLife = 10f;
    public float bulletSpeed = 180f;
    public float damage = 10f;
    public int bulletsPerTap;
    public bool allowButtonHold;
    public bool allowInvoke = true;
    public int ammoType;

    MovePlayer MovePlayer;
    InventoryController GunController;
    public float recoilLength = 5f;
    public float recoilSpeed = 90f;
    public GameObject player;

    private AudioSource audioSource;
    public AudioClip soundClip;
    public GameObject muzzleFlash;

    int bulletsLeft = 200;
    int bulletsShot;

    bool shooting, readyToShoot;

    private void Start()
    {
        MovePlayer = player.GetComponent<MovePlayer>();
        GunController = player.GetComponent<InventoryController>();
        audioSource = player.GetComponent<AudioSource>();
    }

    private void Awake()
    {
        readyToShoot = true;
    }
    void Update()
    {
       // MyInput();
       //if(Input.GetKeyDown(KeyCode.N)) Shoot();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.N);

        if (readyToShoot && shooting)
        {
            if (bulletsLeft > 0) {
                bulletsShot = 0;
                Shoot();
            }
            else
            {
                if (!allowButtonHold || (Input.GetKeyDown(KeyCode.Mouse0))) GunController.failedToShoot();
            }
        }

    }

    public void Shoot()
    {
        readyToShoot = false;
        bulletsLeft--;
        bulletsShot++;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(90f, bulletSpawnPoint.eulerAngles.y, 0f));
        float ySpread = Random.Range(-spread, spread);
        BulletBehaviour BulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        if (BulletBehaviour != null) BulletBehaviour.InitializeBullet(bulletSpeed, true, ySpread, bulletLife, damage);
        else
        {
            BossProjectile Projectile = bullet.GetComponent<BossProjectile>();
            if (Projectile != null) Projectile.InitializeBullet(bulletSpeed, true, ySpread, bulletLife, damage);
        }

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            if (audioSource != null && soundClip != null)
            {
                audioSource.PlayOneShot(soundClip);
            }
            if (muzzleFlash != null)
            {
                Instantiate(muzzleFlash, bulletSpawnPoint.position, bulletSpawnPoint.rotation*Quaternion.Euler(0f, -90f, 0f), bulletSpawnPoint.transform);
            }
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

   


    public int getAmmo() { return bulletsLeft / bulletsPerTap; }

    public int getAmmoType() { return ammoType; }

    public void setAmmo(int ammo) {  bulletsLeft = ammo; }
}
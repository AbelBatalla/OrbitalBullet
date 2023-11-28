using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    //public float bulletSpeed = 10;
    public float timeBetweenShooting, spread, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public bool allowInvoke = true;
    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && shooting && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            bulletsLeft = magazineSize;
        }

    }

    private void Shoot()
    {
        readyToShoot = false;

        bulletsLeft--;
        bulletsShot++;
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        float ySpread = Random.Range(-spread, spread);
        BulletBehaviour BulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        if (BulletBehaviour != null)
        {
            BulletBehaviour.InitializeBullet(180f, true, ySpread, 1.5f);
        }


        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0) Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
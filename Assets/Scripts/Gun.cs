using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    //public float bulletSpeed = 10;
    public float timeBetweenShooting, timeBetweenShots;
    public float spread = 3f;
    public float bulletLife = 10f;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public bool allowInvoke = true;

    public GameObject mapMov;
    MapMovement MapMovement;
    public float recoilLength = 5f;
    public float recoilSpeed = 90f;
    public GameObject player;
    PlayerMovement PlayerMovement;

    int bulletsLeft, bulletsShot;

    bool shooting, readyToShoot;
    bool shootRight = true;

    private void Start()
    {
        MapMovement = mapMov.GetComponent<MapMovement>();
        PlayerMovement = player.GetComponent<PlayerMovement>();
    }

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
        if (!shootRight) bullet.transform.Rotate(Vector3.right, 180);
        float ySpread = Random.Range(-spread, spread);
        BulletBehaviour BulletBehaviour = bullet.GetComponent<BulletBehaviour>();
        if (BulletBehaviour != null)
        {
            BulletBehaviour.InitializeBullet(180f, shootRight, ySpread, bulletLife);
        }

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            MapMovement.giveRecoil(recoilLength, recoilSpeed, shootRight);
            PlayerMovement.stopFall();
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0) Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
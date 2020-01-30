using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float startTimebtwShots;

    [System.NonSerialized]
    public bool weaponOnBody;
    [System.NonSerialized]
    public bool shoot;


    private float timebtwShots;

    Vector3 inHand;
    Quaternion inHandRot;
    Vector3 onBody  = new Vector3(0.316f, -0.244f, 0);
    Quaternion onBodyRot = Quaternion.Euler(0, 0, -90);


    private void Start()
    {
        // for WeapononBody()
        inHand = transform.localPosition;
        inHandRot = transform.localRotation;


    }
    private void FixedUpdate()
    {
        WeaponOnBody();
        InstantiateBullet();
    }

    private void WeaponOnBody()
    {
        //When ShootingJoyStick is not touched
        if (weaponOnBody)
        {
            transform.localPosition = onBody;
            transform.localRotation = onBodyRot;
        }
        //When ShootingJoyStick is used
        else
        {
            transform.localPosition = inHand;
            transform.localRotation = inHandRot;
        }

    }
    private void InstantiateBullet()
    {
        if (timebtwShots <= 0)
        {
            if (shoot)
            {

                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                timebtwShots = startTimebtwShots;
            }
        }
        else
        {

            timebtwShots -= Time.deltaTime;
        }
    }
}

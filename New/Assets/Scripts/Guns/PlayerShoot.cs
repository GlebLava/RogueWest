using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Joystick jS;
    public float startTimebtwShots;

    private float timebtwShots;

    Vector3 inHand;
    Quaternion inHandRot;
    Vector3 onBody;
    Quaternion onBodyRot;


    private void Start()
    {
        // for WeapononBody()
        onBody = new Vector3(0.316f, -0.244f, 0);
        onBodyRot = Quaternion.Euler(0, 0, -90);
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
        if (jS.Horizontal == 0f && jS.Vertical == 0)
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
            if ((jS.Horizontal >= 0.25f || jS.Vertical >= 0.25f) || (jS.Horizontal <= -0.25f || jS.Vertical <= -0.25f))
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


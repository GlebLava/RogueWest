using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    public Joystick jS;
    public SpriteRenderer weapon;
    public Animator animator;
    public GameObject head;


    private Transform aimTransform;
    private void Awake()
    {
        aimTransform = transform.Find("Aim");
    }

    private void FixedUpdate()
    {
        HandleAiming();  
    }

    private void HandleAiming()
    {
        Vector2 dir = jS.Direction;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        
        if (angle > 90 || angle < -90) { 
            weapon.flipY = true;
            head.GetComponent<SpriteRenderer>().flipX = false;
        }
        else 
        { 
            weapon.flipY = false;
            if (angle != 0)
            head.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (angle > 170 || angle < 30) HandleLayer(true);
        else HandleLayer(false);

     }

    public void HandleLayer(bool trig)
    {
        if (trig) weapon.sortingLayerName = "WeaponInFrontOfBody";
        else weapon.sortingLayerName = "WeaponBehindBody";


    }
}

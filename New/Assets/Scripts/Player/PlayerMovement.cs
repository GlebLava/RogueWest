using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public Transform player;
    public Joystick joyStick;
    public Joystick shootStick;
    public Animator animator;
    public GameObject body;
    public GameObject head;

    [System.NonSerialized]
    public Vector2 dir;
    private void Update()
    {
        dir = transform.position;
        if (joyStick.Horizontal != 0f && joyStick.Vertical != 0)
        {
            dir.x = joyStick.Horizontal;
            dir.y = joyStick.Vertical;

        }
       
        else
        {
            dir.x = Input.GetAxis("Horizontal"); 
            dir.y = Input.GetAxis("Vertical");
        }

        animator.SetFloat("Speed", dir.sqrMagnitude);
    }
    private void FixedUpdate()
    {
        HandleMoving();
        HandleFlipWeapon();
    }

    private void HandleMoving()
    {
        if (dir.x < 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;

            if (shootStick.Horizontal == 0f && shootStick.Vertical == 0)
            {
                head.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        if (dir.x > 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = true;

            if (shootStick.Horizontal == 0f && shootStick.Vertical == 0)
            {
                head.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        player.Translate(dir * moveSpeed * Time.deltaTime);

    }
    private void HandleFlipWeapon()
    {
        Vector3 aimScale = transform.Find("Aim").localScale;
        if (dir.x < 0 && shootStick.Horizontal == 0f && shootStick.Vertical == 0)
        {
            aimScale.x = 1;
        }
        if (dir.x > 0 && shootStick.Horizontal == 0f && shootStick.Vertical == 0)
        {
            aimScale.x = -1;
        }
        else if (shootStick.Horizontal != 0f && shootStick.Vertical != 0) aimScale.x = 1;

        transform.Find("Aim").localScale = aimScale;
    }

}


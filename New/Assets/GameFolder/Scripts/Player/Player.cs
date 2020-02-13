using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float health = 100f;

    public Joystick walkStick;
    public Joystick shootStick;
    
    public Animator bodyAnimator;

    public GameObject body;
    public GameObject head;

 
    public SpriteRenderer weapon;

    public event Action DeathEvent;

    
    private Vector2 walkingDirection; public Vector2 Dir { get => walkingDirection; }

    [SerializeField] private UI_Inventory ui_Inventory_script;

    private Inventory inventory;
    private Transform aimTransform;

    private void Awake()
    {
        aimTransform = transform.Find("Aim");

        inventory = new Inventory();
        ui_Inventory_script.SetInventory(inventory);
    }
    private void Start()
    {
        inventory.AddItem(new Item { amount = 1, itemType = Item.ItemType.Coin });
    }
    private void Update()
    {
        SetDirection();
        bodyAnimator.SetFloat("Speed", walkingDirection.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        HandleMoving();
        HandleFlipWeapon();
        HandleAiming();

        if (health < 0) PlayerDie();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
      
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    
    private void SetDirection()
    {
        walkingDirection = transform.position;
        if (walkStick.Horizontal != 0f && walkStick.Vertical != 0)
        {
            walkingDirection.x = walkStick.Horizontal;
            walkingDirection.y = walkStick.Vertical;

        }
        else
        {
            walkingDirection.x = Input.GetAxis("Horizontal");
            walkingDirection.y = Input.GetAxis("Vertical");
        }
    }
    private void HandleMoving()
    {
        if (walkingDirection.x < 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = false;

            if (shootStick.Horizontal == 0f && shootStick.Vertical == 0)
            {
                head.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        if (walkingDirection.x > 0)
        {
            body.GetComponent<SpriteRenderer>().flipX = true;

            if (shootStick.Horizontal == 0f && shootStick.Vertical == 0)
            {
                head.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        transform.Translate(walkingDirection * moveSpeed * Time.deltaTime);

    }
    private void HandleFlipWeapon()
    {
        Vector3 aimScale = transform.Find("Aim").localScale;
        if (walkingDirection.x < 0 && shootStick.Horizontal == 0f && shootStick.Vertical == 0)
        {
            aimScale.x = 1;
        }
        if (walkingDirection.x > 0 && shootStick.Horizontal == 0f && shootStick.Vertical == 0)
        {
            aimScale.x = -1;
        }
        else if (shootStick.Horizontal != 0f && shootStick.Vertical != 0) aimScale.x = 1;

        transform.Find("Aim").localScale = aimScale;
    }

    private void HandleAiming()
    {
        float angle = Mathf.Atan2(shootStick.Direction.y, shootStick.Direction.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
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

    void PlayerDie()
    {
        DeathEvent?.Invoke(); // Same as: if (DeathEvent != 0) DeathEvent();
    }
}

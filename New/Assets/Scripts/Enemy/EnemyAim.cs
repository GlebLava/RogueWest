using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{

    [System.NonSerialized]
    public bool setAim = false;
    [System.NonSerialized]
    public float offset = 0;

    private GameObject aim;
    private Transform aimTransform;

    private GameObject player;
    private GameObject head;

    private GameObject weapon;
   


    void Start()
    {

        aim = transform.Find("Aim").gameObject;
        aimTransform = transform.Find("Aim");

        player = GameObject.FindWithTag("Player");
        head = player.transform.Find("Head").gameObject;

        weapon = aim.transform.Find("Weapon").gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (setAim)
        HandleAim();

    }

    void HandleAim()
    {
        Vector2 direction = new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + offset;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        if (angle > 90 || angle < -90)
        {
            weapon.GetComponent<SpriteRenderer>().flipY = true;
            head.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            weapon.GetComponent<SpriteRenderer>().flipY = false;
            if (angle != 0)
                head.GetComponent<SpriteRenderer>().flipX = true;
        }

        if (angle > 170 || angle < 30) HandleLayer(true);
        else HandleLayer(false);

    }

    void HandleLayer(bool trig)
    {
        if (trig) weapon.GetComponent<SpriteRenderer>().sortingLayerName = "WeaponInFrontOfBody";
        else weapon.GetComponent<SpriteRenderer>().sortingLayerName = "WeaponBehindBody";


    }

}

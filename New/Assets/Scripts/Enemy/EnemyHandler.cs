using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public float seeDistance;
    public float attackRange;
    public float moveSpeed;
    public Animator animator;

    enum State { notAware, aware, inAttackRange, following  }

    private EnemyAim enemyAim;
    private EnemyShoot enemyShoot;

    private GameObject player;

    private GameObject weapon;

    private float distanceToPlayer;
    private float timer;
    private float counter;


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, seeDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;

        weapon = transform.Find("Aim").gameObject.transform.Find("Weapon").gameObject;
        

        enemyAim = GetComponent<EnemyAim>();
        enemyShoot = weapon.GetComponent<EnemyShoot>();
        
        
        
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        timer = 0;
    }

    
    
    void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        Setup();

        timer += Time.deltaTime;
        counter += counter;
  

    }

    void Setup()
    {
        if (distanceToPlayer > seeDistance)    NotAwareBehaviour();


        if (distanceToPlayer < seeDistance && distanceToPlayer > attackRange) AwareBehaviour();


        if (distanceToPlayer < attackRange) InAttackRangeBehaviour();
    }


    void NotAwareBehaviour()
    {
        WanderAround();
        animator.SetFloat("Speed", 0);
    }

    void AwareBehaviour()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", 1);


        if (transform.position.x < player.transform.position.x)
        {
            FlipBody(true);
            FlipHead(true);
        }
        else 
        { 
            FlipBody(false);
            FlipHead(false);
        }

        enemyShoot.shoot = false;

    }
    void InAttackRangeBehaviour()
    {
        
        
        enemyAim.setAim = true;

        if (timer > 3)
        {
            enemyShoot.shoot = true;
            timer = 0;
        }
        else
        {
            enemyShoot.shoot = false;
        }

        animator.SetFloat("Speed", 0);

        

    }

    void WanderAround()
    {
        enemyAim.setAim = false;
        enemyShoot.shoot = false;

        
    }




    void FlipBody(bool t)
    {
        transform.Find("Body").gameObject.GetComponent<SpriteRenderer>().flipX = t;
    }

    void FlipHead (bool t)
    {
        transform.Find("Head").gameObject.GetComponent<SpriteRenderer>().flipX = t;
    }
}

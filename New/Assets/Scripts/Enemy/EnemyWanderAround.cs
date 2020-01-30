using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderAround : MonoBehaviour
{
    public float wanderingSpeed = 3f;

    
    [System.NonSerialized]
    public bool setWander = true;

    private bool isWandering = false;
    private bool isWalking = false;


    Vector2 direction = new Vector2(-1, 0);

    private EnemyShoot enemyShoot;
    private GameObject body;

    private void Awake()
    {
        enemyShoot = transform.Find("Aim").gameObject.transform.Find("Weapon").gameObject.GetComponent<EnemyShoot>();
        body = transform.Find("Body").gameObject;


    }

    private void FixedUpdate()
    {


        if (setWander)
        {  
           enemyShoot.weaponOnBody = true;
            
            
           if (!isWandering)
            {
              StartCoroutine(Wander());
            }

           
          

            if (isWalking)
            {
                transform.Translate(wanderingSpeed * direction * Time.deltaTime);
                body.GetComponent<Animator>().SetFloat("Speed", wanderingSpeed);
            }


        }
    }

    IEnumerator Wander()
    {
       

        direction = new Vector2(Random.Range(-1,1), Random.Range(-1,1));


        float walkWait = Random.Range(0,2);
        float walkTime = Random.Range(0.1f, 1.5f);

        body.GetComponent<Animator>().SetFloat("Speed", walkTime);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
               
      
        isWandering = false; 

    }
   
}

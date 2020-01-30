using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public LayerMask whatIsSolid;
    public float distance;

    private void Start()
    {
        Invoke("DestroyProjectile", lifetime);
    }
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log("hit Enemy");
            }
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
        
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

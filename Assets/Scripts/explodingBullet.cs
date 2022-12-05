using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodingBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;
    [SerializeField] GameObject explosion;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.damage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        


    }
}
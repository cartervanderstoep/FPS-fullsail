using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicAttack : MonoBehaviour
{
    [SerializeField] Rigidbody magicRB;

    [SerializeField] int magicDamage;
    [SerializeField] int magicSpeed;
    [SerializeField] int attackTimer;

    private void Start()
    {
        magicRB.velocity = transform.forward * magicSpeed;
        Destroy(gameObject, attackTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && Vector3.Distance(transform.position,other.transform.position) <= 1.5)
        {
            other.GetComponent<IDamage>().takeDamage(magicDamage);
        Destroy(gameObject);
        }

    }
}

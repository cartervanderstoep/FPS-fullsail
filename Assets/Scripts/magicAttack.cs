using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicAttack : MonoBehaviour
{
    [Header ("----- Components -----")]
    [SerializeField] Rigidbody magicRB;
    [SerializeField] int magicDamage;
    [Range(1, 5)][SerializeField] int magicElm; 
    [SerializeField] float magicSpeed;
    [SerializeField] float attackTimer;

    private Vector3 dropVelocity; 

    private void Start()
    {
        if(magicElm == 1 || magicElm == 2)
        {
            magicRB.velocity = transform.forward * magicSpeed;
        }
        else if(magicElm == 3 || magicElm == 4)
        {
            magicRB.AddForce(Vector3.down * magicSpeed); 
        }
        Destroy(gameObject, attackTimer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") /*&& Vector3.Distance(transform.position, other.transform.position) <= 4*/)
        {
            other.GetComponent<IDamage>().takeDamage(magicDamage);
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("enemy prefab")  )
        {
            Destroy(gameObject);
        }

    }
}

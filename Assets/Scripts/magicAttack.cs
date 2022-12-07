using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicAttack : MonoBehaviour
{
    [Header ("----- Components -----")]
    [SerializeField] Rigidbody magicRB;
    [SerializeField] GameObject expl; 
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
        else if(magicElm == 3)
        {
            magicRB.AddForce(Vector3.down * magicSpeed); 
        }
        else if (magicElm == 4)
        {
            StartCoroutine(timer()); 
        }
        Destroy(gameObject, attackTimer);
    }
    
    IEnumerator timer()
    {
        yield return new WaitForSeconds(attackTimer - 0.5f);
        Instantiate(expl, transform.position, transform.rotation); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (magicElm != 4)
        {
            if (other.CompareTag("Enemy") /*&& Vector3.Distance(transform.position, other.transform.position) <= 4*/)
            {
                other.GetComponent<IDamage>().takeDamage(magicDamage);
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Enemy") && !other.CompareTag("enemy prefab"))
            {
                Destroy(gameObject);
            }
        }
        else if(magicElm == 4)
        {
            if (other.CompareTag("Enemy") /*&& Vector3.Distance(transform.position, other.transform.position) <= 4*/)
            {
                other.GetComponent<IDamage>().takeDamage(magicDamage);
                Instantiate(expl, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (!other.CompareTag("Enemy") && !other.CompareTag("enemy prefab"))
            {
                Instantiate(expl, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaFloor : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float timer;
    [SerializeField] bool damageEnemy;
    bool inRange;
    bool inRangeEnemy;
    bool tookDamage;
    bool tookDamageEnemy;

    // Start is called before the first frame update
    void Start()
    {
        tookDamage = false;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (!tookDamage)
            {
                StartCoroutine(inflictDamage());
            }
        }
        else if (damageEnemy)
        {
            if (other.CompareTag("Enemy"))
            {
                inRangeEnemy = true;
                if (!tookDamageEnemy)
                {
                    StartCoroutine(inflictDamageEnemy(other));
                }
            }
        }
    }

    IEnumerator inflictDamage()
    {
        if (inRange)
        {
            tookDamage = true;
            gameManager.instance.playerScript.damage(damage);
            yield return new WaitForSeconds(timer);
            tookDamage = false;
        }
    }
    IEnumerator inflictDamageEnemy(Collider other)
    {
        if (inRangeEnemy)
        {
            tookDamageEnemy = true;
            other.GetComponent<IDamage>().takeDamage(damage);
            yield return new WaitForSeconds(timer);
            tookDamageEnemy = false;
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}

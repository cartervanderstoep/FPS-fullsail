using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaFloor : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] int timer;
    bool inRange;
    bool tookDamage;

    // Start is called before the first frame update
    void Start()
    {
        tookDamage = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            if (!tookDamage)
            {
                StartCoroutine(inflictDamage());
            }
        }
    }

    IEnumerator inflictDamage()
    {
        while (inRange)
        {
            tookDamage = true;
            gameManager.instance.playerScript.damage(damage);
            yield return new WaitForSeconds(timer);
            tookDamage = false;
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

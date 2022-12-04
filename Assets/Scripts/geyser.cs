using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geyser : MonoBehaviour
{
    [SerializeField] float timer;
    bool inRange;
    bool tookDamage;

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
    }

    IEnumerator inflictDamage()
    {
        if (inRange)
        {
            tookDamage = true;
            gameManager.instance.playerScript.damage(1);
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

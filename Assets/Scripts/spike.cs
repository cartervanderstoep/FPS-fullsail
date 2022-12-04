using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class spike : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] int damage;
    bool spiked;
    bool tookDamage;
    bool inRange;

    Vector3 origPos;
    Vector3 move;
    Vector3 lastPos;

    void Start()
    {
        tookDamage = false;
        origPos = this.transform.position;
        move = new Vector3(0, 2, 0);
        lastPos = this.transform.position + move;
    }
    // Update is called once per frame
    void Update()
    {
        if (!spiked)
        {
            StartCoroutine(spikeMovement());
        }
    }

    IEnumerator spikeMovement()
    {
        spiked = true;
        this.gameObject.transform.position = lastPos;
        yield return new WaitForSeconds(timer);
        this.gameObject.transform.position = origPos;
        yield return new WaitForSeconds(timer);
        spiked=false;
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

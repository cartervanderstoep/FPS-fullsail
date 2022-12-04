using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class spikeTrigger : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] int damage;
    [SerializeField] bool timed;
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
        if (timed)
        {
            if (!spiked)
            {
                StartCoroutine(spikeMovement());
            }
        }
        //else if (triggered)
        //{
        //    if (!spiked)
        //    {
        //        StartCoroutine(spikeTrigger());
        //    }
        //}
    }

    IEnumerator spikeMovement()
    {
        spiked = true;
        this.gameObject.transform.position = lastPos;
        yield return new WaitForSeconds(timer);
        this.gameObject.transform.position = origPos;
        yield return new WaitForSeconds(timer);
        spiked = false;
    }

    //IEnumerator spikeTrigger()
    //{
    //    spiked = true;
    //    yield return new WaitForSeconds(0.2f);
    //    this.gameObject.transform.position = lastPos;
    //    yield return new WaitForSeconds(timer);
    //    this.gameObject.transform.position = origPos;
    //    yield return new WaitForSeconds(timer);
    //    spiked = false;
    //}
    //public void OnTriggerEnter(Collider other)
    //{
    //    if (triggered)
    //    {
    //        trigger = true;
    //    }
    //}
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

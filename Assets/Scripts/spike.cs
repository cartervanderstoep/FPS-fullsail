using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;

public class spike : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] int damage;
    [SerializeField] bool timed;
    [SerializeField] AudioSource aud;

    [Header("--------------sound---------------")]
    [SerializeField] AudioClip poke;
    [SerializeField] float volume;

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
        move = new Vector3(0, 5, 0);
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
    }

    IEnumerator spikeMovement()
    {
        spiked = true;
        aud.PlayOneShot(poke, volume);
        this.gameObject.transform.position = lastPos;
        yield return new WaitForSeconds(timer);
        aud.PlayOneShot(poke, volume);
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

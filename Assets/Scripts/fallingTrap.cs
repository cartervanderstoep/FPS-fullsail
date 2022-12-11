using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class fallingTrap : MonoBehaviour
{

    [SerializeField] float forceAmount;
    [SerializeField] int damage;
    [SerializeField] bool pull;
    [SerializeField] AudioSource aud;

    [Header("--------------sound---------------")]
    [SerializeField] AudioClip fall;
    [SerializeField] float volume;

    private void OnTriggerEnter(Collider other)
    {
        aud.PlayOneShot(fall, volume);
        if (other.CompareTag("Player"))
        {
            if (damage != 0)
            {
                gameManager.instance.playerScript.damage(damage);
            }
            if (pull)
                gameManager.instance.playerScript.pushBack = (transform.position - other.transform.position).normalized * forceAmount;
            else
                gameManager.instance.playerScript.pushBack = (other.transform.position - transform.position).normalized * forceAmount;
        }
    }
}

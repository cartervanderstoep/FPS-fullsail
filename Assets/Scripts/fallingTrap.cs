using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class fallingTrap : MonoBehaviour
{

    [SerializeField] int forceAmount;
    [SerializeField] int damage;
    [SerializeField] bool pull;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.damage(damage);
            if (pull)
                gameManager.instance.playerScript.pushBack = (transform.position - other.transform.position).normalized * forceAmount;
            else
                gameManager.instance.playerScript.pushBack = (other.transform.position - transform.position).normalized * forceAmount;
        }
    }
}

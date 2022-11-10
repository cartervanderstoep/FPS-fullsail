using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerupPickup : MonoBehaviour
{
    [SerializeField] powerupStats powerupStat;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.powerupActPlayer(powerupStat);
            Destroy(gameObject);
        }
    }
}


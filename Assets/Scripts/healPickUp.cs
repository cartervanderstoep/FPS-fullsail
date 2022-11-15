using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPickUp : MonoBehaviour
{
    [SerializeField] healStats healStat;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.healPickUp(healStat);
            Destroy(gameObject);
        }
    }
}

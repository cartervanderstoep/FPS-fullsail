using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDestroy : MonoBehaviour
{
    public GameObject sound;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.keyCount += 1;
            Instantiate(sound, this.transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}

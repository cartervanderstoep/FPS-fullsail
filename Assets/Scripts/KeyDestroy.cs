using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDestroy : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        gameManager.instance.keyCount += 1;
        Destroy(gameObject);
    }
}

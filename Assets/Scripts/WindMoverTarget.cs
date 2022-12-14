using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMoverTarget : MonoBehaviour
{
    public GameObject destroy;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(destroy);
        }
    }
}

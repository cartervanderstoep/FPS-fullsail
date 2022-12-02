using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelChangeBoxS : MonoBehaviour
{
    [SerializeField] levelChangeBox box;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.nextScene();
            Destroy(gameObject);
        }
    }
}

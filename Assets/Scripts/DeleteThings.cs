using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThings : MonoBehaviour
{
    public GameObject[] Things;

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < Things.Length; i++)
            {
                Things[i].SetActive(false);
            }
        }
    }
}

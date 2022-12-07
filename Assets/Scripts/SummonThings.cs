using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonThings : MonoBehaviour
{
    public GameObject[] Things;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < Things.Length; i++)
            {
                Things[i].SetActive(true);
            }
        }
    }
}

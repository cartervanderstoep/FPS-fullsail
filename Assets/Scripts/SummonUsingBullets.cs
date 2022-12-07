using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUsingBullets : MonoBehaviour
{
    public GameObject[] Fire;
    public GameObject[] Ice;
    public GameObject[] Lightning;
    public GameObject[] Meteor;
    public int timer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            for (int i = 0; i < Fire.Length; i++)
            {
                Fire[i].SetActive(true);
            }
            
        }
        if (other.CompareTag("Ice"))
        {
            for (int i = 0; i < Ice.Length; i++)
            {
                Ice[i].SetActive(true);
            }

        }
        if (other.CompareTag("Lightning"))
        {
            for (int i = 0; i < Lightning.Length; i++)
            {
                Lightning[i].SetActive(true);
            }

        }
        if (other.CompareTag("Meteor"))
        {
            for (int i = 0; i < Meteor.Length; i++)
            {
                Meteor[i].SetActive(true);
            }

        }

    }


}

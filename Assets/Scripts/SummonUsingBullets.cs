using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUsingBullets : MonoBehaviour
{
    public GameObject[] Fire;
    public GameObject[] Ice;
    public GameObject frozen1;
    public GameObject frozen2;
    public GameObject frozen3;
    public GameObject[] Lightning;
    public GameObject[] Meteor;
    public GameObject enemy;
    int freeze;
    Vector3 enemyPos;
    public int timer;

    private void Update()
    {
        

    }
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
            if (freeze == 3) // if the enemy is hit 3 times with an ice spell they freeze. this will disable the enemy and place a frozen 3d model of them instead.
            {
                stuff();
                
            }
           
        }
        if (other.CompareTag("Ice"))
        {
            if(freeze >= 3) // if the enemy is hit 3 times with an ice spell they freeze. this will disable the enemy and place a frozen 3d model of them instead.
            {
                enemyPos = this.transform.position;
                frozen1.transform.position = enemyPos;
                frozen2.transform.position = enemyPos;
                frozen3.transform.position = enemyPos;
                frozen1.SetActive(true);
                frozen2.SetActive(true);
                frozen3.SetActive(true);
                for (int i = 0; i < Ice.Length; i++)
                {
                    Ice[i].SetActive(false);
                }
                Destroy(enemy);
            }
            
            if(freeze < 3)
            {
                Ice[freeze].SetActive(true); // this will increase the number of freeze effects each time they are hit. 

                freeze += 1;
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
    public void stuff()
    {
        
    }


}

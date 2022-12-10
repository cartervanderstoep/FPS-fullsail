using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonUsingBullets : MonoBehaviour
{
    public GameObject[] Fire;
    public GameObject[] Ice;
    public GameObject frozen;
    public GameObject[] Lightning;
    public GameObject[] Meteor;
    public GameObject enemy;
    int freeze;
    Vector3 enemyPos;
    public int fireTimer;
    public int lightningTimer;
    public int FrozenPos;

    private void Update()
    {
        

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            StartCoroutine(delayFire());
            
        }

        
        if (other.CompareTag("Ice"))
        {
            if(freeze >= 3) // if the enemy is hit 3 times with an ice spell they freeze. this will disable the enemy and place a frozen 3d model of them instead.
            {
                enemyPos = this.transform.position;
                frozen.transform.position = enemyPos;
                GameObject fz = Instantiate(frozen, enemyPos, transform.rotation);
                //frozen.SetActive(true);
                fz.transform.position += Vector3.down * FrozenPos;
                for (int i = 0; i < Ice.Length; i++)
                {
                    Ice[i].SetActive(false);
                }
                Destroy(enemy);
            }
            
            if(freeze < 3&& Ice.Length > 0)
            {
                Ice[freeze].SetActive(true); // this will increase the number of freeze effects each time they are hit. 

                freeze += 1;
            }
            
        }
        if (other.CompareTag("Lightning"))
        {
            StartCoroutine(delayLightning());

        }
        if (other.CompareTag("Meteor"))
        {
            for (int i = 0; i < Meteor.Length; i++)
            {
                Meteor[i].SetActive(true);
            }

        }

    }

    public IEnumerator delayFire()
    {
        for (int i = 0; i < Fire.Length; i++)
        {
            Fire[i].SetActive(true);
        }
        yield return new WaitForSeconds(fireTimer);
        for (int i = 0; i < Fire.Length; i++)
        {
            Fire[i].SetActive(false);
        }

    }
    public IEnumerator delayLightning()
    {
        for (int i = 0; i < Lightning.Length; i++)
        {
            Lightning[i].SetActive(true);
        }
        yield return new WaitForSeconds(lightningTimer);
        for (int i = 0; i < Lightning.Length; i++)
        {
            Lightning[i].SetActive(false);
        }

    }



}

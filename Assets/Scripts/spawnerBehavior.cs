using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerBehavior : MonoBehaviour
{
    [SerializeField] GameObject enemy1;
    [SerializeField] GameObject enemy2;
    [SerializeField] GameObject enemy3;
    [SerializeField] GameObject enemy4;
    [SerializeField] GameObject enemy5;

    [SerializeField] GameObject spawn1;
    [SerializeField] GameObject spawn2;
    [SerializeField] GameObject spawn3;
    [SerializeField] GameObject spawn4;
    [SerializeField] GameObject spawn5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemy1 != null && enemy1.GetComponent<IDamage>() != null) ;
            {
                Instantiate(enemy1, spawn1.transform.position, transform.rotation);
            }
            if (enemy2 != null && enemy2.GetComponent<IDamage>() != null) ;
            {
                Instantiate(enemy2, spawn2.transform.position, transform.rotation);
            }
            if (enemy3 != null && enemy3.GetComponent<IDamage>() != null) ;
            {
                Instantiate(enemy3, spawn3.transform.position, transform.rotation);
            }
            if (enemy4 != null && enemy4.GetComponent<IDamage>() != null) ;
            {
                Instantiate(enemy4, spawn4.transform.position, transform.rotation);
            }
            if (enemy5 != null && enemy5.GetComponent<IDamage>() != null) ;
            {
                Instantiate(enemy5, spawn5.transform.position, transform.rotation);
            }


        }
    }
}

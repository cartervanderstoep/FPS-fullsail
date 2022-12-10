using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnBehavior : MonoBehaviour
{
    //[SerializeField] GameObject enemy1;
    //[SerializeField] GameObject enemy2;
    //[SerializeField] GameObject enemy3;
    //[SerializeField] GameObject enemy4;
    //[SerializeField] GameObject enemy5;
    [SerializeField] List<GameObject> spawnList = new List<GameObject>();

    //[SerializeField] GameObject spawn1;
    //[SerializeField] GameObject spawn2;
    //[SerializeField] GameObject spawn3;
    //[SerializeField] GameObject spawn4;
    //[SerializeField] GameObject spawn5;
    [SerializeField] List<Transform> spawnTransformList = new List<Transform>();
    [SerializeField] GameObject spawnFX;

    [SerializeField] bool isBomb;


    int spawnTarget = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (!isBomb)
        {
            gameManager.instance.enemiesToKill += spawnList.Count;
            gameManager.instance.updateUI();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            spawn();

        }
        if (isBomb)
        {
            if (other.GetComponent<IDamage>() != null && Vector3.Distance(transform.position, other.transform.position) <= 2)
            {
                spawn();
                Destroy(gameObject);
            }
        }
    }
    void spawn()
    {
        if (spawnTarget < spawnList.Count)
        {
            Instantiate(spawnList[spawnTarget], spawnTransformList[spawnTarget].transform.position, transform.rotation);
            if (!isBomb)
            {
                Instantiate(spawnFX, spawnTransformList[spawnTarget].transform.position, transform.rotation);
            }


            spawnTarget++;

            spawn();

        }
    }
}


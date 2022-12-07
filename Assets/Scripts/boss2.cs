using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boss2 : MonoBehaviour, IDamage
{
    [SerializeField] List<MeshRenderer> models;
    [SerializeField] int Hp;

    [Header("offensive components")]
  
    [SerializeField] List<Transform> chainGuns;
    [SerializeField] List<GameObject> gunAmount;
    [SerializeField] List<GameObject> spawnSpots;
    [SerializeField] GameObject enemySpawn;
    [SerializeField] bool phase2;
    [SerializeField] float spawnTimer;
    [SerializeField] float spawnBreak;

    int maxHp;
    bool guns;
    bool spawning;

    
    // Start is called before the first frame update
    void Start()
    {
           phase2 = false;
        maxHp = Hp;
        gameManager.instance.enemiesToKill++;
        gameManager.instance.updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Hp <= (maxHp/2))
        {
            phase2 = true;
        }

        if(!guns && phase2)
        {
            guns = true;
           // StartCoroutine(sideGuns());
           for (int i = 0; i < chainGuns.Count; i++)
            {
                gunAmount[i].SetActive(true);
            }
        }

        if (!spawning)
        {
            spawning = true;
            StartCoroutine(spawn());
        }

    }

    public void takeDamage(int dmg)
    {
           Hp -= dmg;
        StartCoroutine(flashDamage()); 
        if (Hp<=0)
        {
            Destroy(gameObject);
            gameManager.instance.updateEnemyNumber();
            gameManager.instance.updateUI();
        }
    }
    IEnumerator flashDamage()
    {
        for (int i = 0; i < models.Count; i++)
        {
            models[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < models.Count; i++)
        {
            models[i].material.color = Color.white;
        }
    }

    //IEnumerator sideGuns()
    //{
    //    for (int i = 0; i < chainGuns.Count;i++)
    //    {
           
           

    //        Instantiate(gunAmount[i], chainGuns[i].position, chainGuns[i].rotation);
           

    //    }
    //    yield return new WaitForSeconds(gunActiveTime);

    //    for (int i = 0; i < gunAmount.Count; i++)
    //    {
            
           
            
    //    }

    //    yield return new WaitForSeconds(gunDownTime);

    //    guns = false;

    //}

    IEnumerator spawn()
    {
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            Instantiate(enemySpawn, spawnSpots[i].transform.position, spawnSpots[i].transform.rotation);
        }
        yield return new WaitForSeconds(spawnTimer);
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            Instantiate(enemySpawn, spawnSpots[i].transform.position, spawnSpots[i].transform.rotation);
        }

        yield return new WaitForSeconds(spawnBreak);

        spawning = false;

    }
}

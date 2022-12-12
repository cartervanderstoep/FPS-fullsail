using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boss2 : MonoBehaviour, IDamage
{
    [Header("----normal components------")]
    [SerializeField] GameObject model;
    [SerializeField] GameObject spawnFx;
    [SerializeField] int Hp;
    [SerializeField] AudioSource aud;
    [SerializeField] float volume;
    [SerializeField] AudioClip hurt;

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
    int spawnCount;
    
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

        if (spawnCount <1)
        {
            spawnCount++;
            StartCoroutine(spawn());
        }
        else if(spawnCount <2  && phase2)
        {
            spawnCount++;
            StartCoroutine(spawn());
        }

    }

    public void takeDamage(int dmg)
    {
           Hp -= dmg;

        if (Hp<=0)
        {
            Destroy(gameObject);
            gameManager.instance.youWin();
            gameManager.instance.updateEnemyNumber();
            gameManager.instance.updateUI();
        }
        StartCoroutine(flashDamage());
        aud.PlayOneShot(hurt, volume);
    }
    IEnumerator flashDamage()
    {
        model.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        model.SetActive(false);
        
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
            Instantiate(spawnFx, spawnSpots[i].transform.position, spawnSpots[i].transform.rotation);
        }
        yield return new WaitForSeconds(spawnTimer);
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            Instantiate(enemySpawn, spawnSpots[i].transform.position, spawnSpots[i].transform.rotation);
            Instantiate(spawnFx, spawnSpots[i].transform.position, spawnSpots[i].transform.rotation);
        }

        yield return new WaitForSeconds(spawnBreak);

        spawning = false;

    }
    public void blackHole()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class birdScript : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int damage;
   

    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;


    bool playerInRange;
    Vector3 playerDir;
    Vector3 playerPos;
    float mobSpeed;
    bool isFighting;
    bool ascending;
   
    


    // Start is called before the first frame update
    void Start()
    {
        mobSpeed = agent.speed;
        gameManager.instance.enemiesToKill++;
        gameManager.instance.updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsTargeted && !isFighting && !ascending)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
        
        playerDir =gameManager.instance.player.transform.position;

        if (isFighting == false)
        {
           transform.position = new Vector3(transform.position.x, gameManager.instance.player.transform.position.y + 6.0f, transform.position.z);
        }
        else if (ascending == false)
        {
            Debug.Log("swooping");
            
            diveBomb();
        }
        else if(ascending)
        {
            ascension();
        }

        facePlayer();

       diveBombCheck();
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * playerFaceSpeed);
    }
    public void takeDamage(int dmg)
    {
        HP -= dmg;
        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.updateEnemyNumber();


        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    void diveBombCheck()
    {

       
        if (Vector3.Distance(transform.position, playerPos) <= 2)
        {
            if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= 2 && isFighting)
            {
                gameManager.instance.playerScript.damage(damage);
                isFighting = false;
            }
            ascension();
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPos = gameManager.instance.player.transform.position;
            playerInRange = true;
            isFighting = true;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isFighting = false;
            ascending = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = true;


        }
    }

    void ascension ()
    {
        ascending = true;
        Debug.Log("ascending");
        Vector3 ascend = new Vector3(playerPos.x * 2, playerPos.y +6, playerPos.z *2);
        transform.Translate(transform.forward * agent.speed * Time.deltaTime);
        transform.Translate(transform.up * agent.speed * Time.deltaTime);
        
    }

    void diveBomb()
    {
        transform.position= Vector3.Slerp(transform.position, playerPos, agent.speed * Time.deltaTime); 

    }
  
}
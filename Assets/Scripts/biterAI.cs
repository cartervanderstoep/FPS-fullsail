using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class biterAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int damage;
    [SerializeField] float attackRate;


    bool playerInRange;
    Vector3 playerDir;
    float mobSpeed;
    bool isFighting;
   

    // Start is called before the first frame update
    void Start()
    {
        mobSpeed = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
      
        agent.SetDestination(gameManager.instance.player.transform.position);

        playerDir = (gameManager.instance.player.transform.position - transform.position);

        
            facePlayer();
        if (playerInRange && !isFighting)
        {
            StartCoroutine(attack());

        }






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
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            agent.speed = 0;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.speed = mobSpeed;
        }
    }
   IEnumerator attack()
    
    {
        isFighting = true;
            gameManager.instance.playerScript.damage(damage);

           
        model.material.color = Color.yellow;
            yield return new WaitForSeconds(attackRate);
        model.material.color= Color.white;
        isFighting=false;
           
        
    }
}
                                                   
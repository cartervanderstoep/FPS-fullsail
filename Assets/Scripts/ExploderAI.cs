using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExploderAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;

    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;

   
    bool playerInRange;
    public GameObject explosion;
    int warningCount;
    bool isExploding;
    bool isFlashing;
    float mobSpeed;
    Vector3 playerDir;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.enemiesToKill++;
        gameManager.instance.updateUI();
        isExploding = false;
        isFlashing = false;
        mobSpeed = agent.speed;
        warningCount = 0;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsTargeted)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }


        playerDir = (gameManager.instance.player.transform.position - transform.position);

        if (playerInRange)
        {
            facePlayer();


        }
        if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < 4 && isFlashing == false)
        {
                isFlashing = true;
                StartCoroutine(warningFlash());
            
                warningCount++;
            
            
            if (warningCount >=3 && !isExploding)
            {
                explode();
            }
            Debug.Log(warningCount.ToString());
        }
        else if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) > 4)
        {
            if (warningCount > 0)
            {
                warningCount--;
            }
            agent.speed = mobSpeed;
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


            explode();

        }
    }

    IEnumerator flashDamage()
    {

        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }

    IEnumerator warningFlash()
    {
       
        agent.speed = 0;
        model.material.color = Color.yellow;
        yield return new WaitForSeconds(.3f);
        model.material.color= Color.white;
        yield return new WaitForSeconds(.3f);
        isFlashing = false;

    }
    void explode()
    {
        isExploding = true;
        gameManager.instance.updateEnemyNumber();
        Instantiate(explosion,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
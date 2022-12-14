using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class birdScript : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int damage;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int roamDist;
    [SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;
    [SerializeField] int attackSpeed;

    [Header("--------------sound---------------")]
    [SerializeField] AudioClip hurt;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip attack;
    [SerializeField] float volume;


    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;


    bool playerInRange;
    Vector3 playerDir;
    Vector3 playerPos;
    float mobSpeed;
    bool isFighting;
    bool ascending;
    float angleToPlayer;
    float breakDist;
    Vector3 startingPos;
    bool isDead;

    int diveTimes;



    // Start is called before the first frame update
    void Start()
    {
        mobSpeed = agent.speed;
       
        gameManager.instance.updateUI();
        breakDist = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsTargeted && !isFighting && !ascending  && agent.enabled)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

        playerDir = (gameManager.instance.player.transform.position - headPos.transform.position);

        if (isFighting == false && agent.enabled)
        {
            transform.position = new Vector3(transform.position.x, gameManager.instance.player.transform.position.y + 6.0f, transform.position.z);
            agent.enabled = true;

        }

        if (anim.GetBool("Dead") == false  )  
        {
            if (playerInRange)
            {
                canSeePlayer();
            }
            else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
            {
                roam();
            }
        }

        


       

        diveBombCheck();

        if (HP <= 0 )
        {
            transform.position = Vector3.Lerp(transform.position,Vector3.down, agent.speed * Time.deltaTime);   
        }
    }
    void canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position) - headPos.transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = breakDist;
                playerIsTargeted = true;
                // agent.SetDestination(gameManager.instance.player.transform.position);
               
                if (agent.enabled && agent.remainingDistance < agent.stoppingDistance)

                    facePlayer();
                if (ascending == false)
                {
                    
                    Debug.Log("swooping");
                    

                    diveBomb();
                }
                else if (ascending)
                {
                    ascension();
                }



            }

        }
    }
    void roam()
    {
        agent.stoppingDistance = 0;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;

        randomDir += startingPos;

        NavMeshHit hit;
       bool foundPath= NavMesh.SamplePosition(new Vector3(randomDir.x, randomDir.y, randomDir.z), out hit, 3, NavMesh.AllAreas);
        NavMeshPath path = new NavMeshPath();
        if (foundPath)
        {
            agent.CalculatePath(hit.position, path);
            agent.SetPath(path);
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
        if (!isDead)
        {

            HP -= dmg;
            if (agent.enabled)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
            StartCoroutine(flashDamage());
            agent.stoppingDistance = 0;
            if (HP <= 0)
            {
                
                gameManager.instance.updateEnemyNumber();
                agent.enabled = false;
                anim.SetBool("Dead", true);
                isDead = true;
                gameObject.GetComponent<Collider>().enabled = false;
                aud.PlayOneShot(dead, volume);
                
                 



                //  Destroy(gameObject);

            }
            else
            {
                aud.PlayOneShot(hurt,volume);
            }
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
          
             if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) <= 3 && isFighting)
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
            agent.enabled= false;

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            isFighting = false;
            ascending = false;
            agent.enabled= true;


        }
    }

    void ascension()
    {
        ascending = true;
        Debug.Log("ascending");
        Vector3 ascend = new Vector3(playerPos.x * 2, playerPos.y + 6, playerPos.z * 2);
        transform.Translate(transform.forward * attackSpeed * Time.deltaTime);
        transform.Translate(transform.up * attackSpeed * Time.deltaTime);
        diveTimes = 0;

    }

    void diveBomb()
    {
        if (diveTimes < 1)
        {
            diveTimes++;
            aud.PlayOneShot(attack, volume);
        }
        transform.position = Vector3.Slerp(transform.position, playerPos, attackSpeed * Time.deltaTime);
        
         StartCoroutine(playAnimation());
    }
    IEnumerator playAnimation()
    {
        if (!ascending)
        {
            anim.SetTrigger("Dive");
        }
        yield return new WaitForSeconds(.3f);
    }

    public void blackHole()
    {

    }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] AudioSource aud;


    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int speedChase;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int roamDist;
    [SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;

    [Header("--------------sound---------------")]
    [SerializeField] AudioClip[] hurt;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip attack;
    [SerializeField] float volume;

    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;

    
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float angleToPlayer;
    float breakDistance;
    Vector3 startingPos;
    float speedOrig;
    bool isDead;

    // Start is called before the first frame update
    void Start()
    {
       
        gameManager.instance.updateUI();
        breakDistance = agent.stoppingDistance;
        startingPos = transform.position;

        speedOrig = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (playerIsTargeted && agent.enabled)
        {
            agent.SetDestination(gameManager.instance.player.transform.position );
        }


        
        if (agent.enabled)
        {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"),agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

            if (playerInRange)
            {
                canSeePlayer();
            }
            else if (agent.remainingDistance < 0.1f && agent.destination!= gameManager.instance.player.transform.position)
            {
                roam();
            }
        }
    }
    void canSeePlayer()
    {
       playerDir = (gameManager.instance.player.transform.position) - headPos.transform.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(headPos.transform.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player")&& angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = breakDistance;
                agent.SetDestination(gameManager.instance.player.transform.position);
                playerIsTargeted = true;
                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();
                if (!isShooting)
                    StartCoroutine(shoot());
            }
        }
    }
    void roam()
    {
        agent.stoppingDistance = 0;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;

        randomDir += startingPos;

        NavMeshHit hit;
        bool foundPath = NavMesh.SamplePosition(new Vector3(randomDir.x, randomDir.y, randomDir.z), out hit, 3, NavMesh.AllAreas);
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
            if (agent.enabled == true)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
            }
            StartCoroutine(flashDamage());
            
            agent.stoppingDistance = 0;
            if (HP <= 0)
            {
                isDead = true;
                gameManager.instance.updateEnemyNumber();
                agent.enabled = false;
                anim.SetBool("Dead", true);
                aud.PlayOneShot(dead,volume);
                gameObject.GetComponent<Collider>().enabled = false;
                
            }
            if (HP > 0)
            {
                int randomHurt = Random.Range(0, 2);
                aud.PlayOneShot(hurt[randomHurt], volume);
            }
        }
    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    IEnumerator shoot()
    {
       
        isShooting = true;
        anim.SetTrigger("Shoot");

        Instantiate(bullet, shootPos.position, transform.rotation);
        aud.PlayOneShot(attack, volume);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public int getHP()
    {
        return HP;
    }
    public void blackHole()
    {
        
    }
}

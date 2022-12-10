using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class biterAI : MonoBehaviour, IDamage
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
    [SerializeField] float attackRate;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int roamDist;
    [SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;

    [Header("--------------sound---------------")]
    [SerializeField] AudioClip hurt;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float volume;

    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;


    bool playerInRange;
    Vector3 playerDir;
    float mobSpeed;
    bool isFighting;
    float breakDist;
    float angleToPlayer;
    Vector3 trackingPos;
    Vector3 startingPos;
    Color colorOrig;
   

    // Start is called before the first frame update
    void Start()
    {
        mobSpeed = agent.speed;
       
        gameManager.instance.updateUI();
        breakDist = agent.stoppingDistance;
        startingPos = transform.position;
        colorOrig = model.material.color;
      
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsTargeted)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
        }
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

        playerDir = (gameManager.instance.player.transform.position - trackingPos);
        trackingPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        if (agent.enabled)
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

    }
    void canSeePlayer()
    {
        playerDir = (gameManager.instance.player.transform.position - trackingPos);
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(trackingPos,playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                playerIsTargeted = true;
                facePlayer();
                if (agent.remainingDistance < breakDist && !isFighting)
                {
                    StartCoroutine(attack());

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
        NavMesh.SamplePosition(new Vector3(randomDir.x, 0, randomDir.z), out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
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
        if (!anim.GetBool("Dead"))
        {


            if (HP <= 0)
            {
                // Destroy(gameObject);
                gameObject.GetComponent<Collider>().enabled = false;
                gameManager.instance.updateEnemyNumber();
                aud.PlayOneShot(dead,volume);
                anim.SetBool("Dead", true);
                agent.enabled = false;



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
        model.material.color = colorOrig;
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
   IEnumerator attack()
    
    {
        isFighting = true;
            gameManager.instance.playerScript.damage(damage);
        anim.SetTrigger("Attack");
        aud.PlayOneShot(attackSound,volume);

           
        model.material.color = Color.yellow;
            yield return new WaitForSeconds(attackRate);
        model.material.color= colorOrig;
        isFighting=false;
           
        
    }
}
                                                   
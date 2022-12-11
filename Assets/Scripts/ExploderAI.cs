using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ExploderAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int HP;
    [SerializeField] int playerFaceSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int sightAngle;
    [SerializeField] int roamDist;
    [SerializeField] int animLerpSpeed;
    [SerializeField] GameObject headPos;

    [Header("---------test bool--------")]
    [SerializeField] bool playerIsTargeted;

   
    bool playerInRange;
    public GameObject explosion;
    int warningCount;
    bool isExploding;
    bool isFlashing;
    float mobSpeed;
    float angleToPlayer;
    float breakDist;
    int explosionCount;
    Vector3 playerDir;
    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
       
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
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

        playerDir = (gameManager.instance.player.transform.position - headPos.transform.position);


        if (agent.enabled)
        {
            if (playerInRange)
            {

                canSeePlayer();

            }
            else if(agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
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
            if (hit.collider.CompareTag("Player") && angleToPlayer <= sightAngle)
            {
                agent.stoppingDistance = breakDist;      
                agent.SetDestination(gameManager.instance.player.transform.position);
                playerIsTargeted = true;
                if (agent.remainingDistance < agent.stoppingDistance)
                    facePlayer();
                if (Vector3.Distance(transform.position, gameManager.instance.player.transform.position) < 4 && isFlashing == false)
                {
                    isFlashing = true;
                    StartCoroutine(warningFlash());

                    warningCount++;


                    if (warningCount >= 3 && !isExploding)
                    {
                        warningCount = 0;
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

        if (HP <= 0 && !isExploding)
        {
            Debug.Log("ouch");
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
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(.3f);
        model.material.color= Color.white;
        yield return new WaitForSeconds(.3f);
        isFlashing = false;

    }
    void explode()
    {
        if (!isExploding)
        {
            Debug.Log("bang");
            isExploding = true;
            gameManager.instance.updateEnemyNumber();
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
          playerInRange = true;
    }
    public void blackHole()
    {

    }
}
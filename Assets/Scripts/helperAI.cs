using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class helperAI : MonoBehaviour, IDamage
{
    [Header("-----------------components-------------")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Collider helperColl;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;

    [Header("---------stats---------------")]
    [SerializeField] int Hp;
    [SerializeField] int damage;
    [SerializeField] float rotationSpeed;
    [SerializeField] float attackDist;
    [SerializeField] float attackInterval;
    [SerializeField] float timer;
    [SerializeField] int animLerpSpeed;


    float speed;
    int targetCount;
    GameObject target;
    Vector3 targetDir;
    bool attacking;
    List<GameObject> targetList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        target = gameManager.instance.player;
        targetCount = 0;
        attacking = false;
        StartCoroutine(die());
        speed = agent.speed;
        gameManager.instance.minionList.Add(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), agent.velocity.normalized.magnitude, Time.deltaTime * animLerpSpeed));

        if (target == null)
        {
            target = gameManager.instance.player;
            targetCount -= 1;
        }
        else
        {
            targetDir = target.transform.position - transform.position;
        }

        agent.SetDestination(target.transform.position);

        if (!attacking)
        {
            attacking = true;
            StartCoroutine(attack());

        }

        if (target.GetComponent<NavMeshAgent>() != null)
        {
            if (!target.GetComponent<NavMeshAgent>().enabled)
            {
                target = null;
            }
        }

        faceTarget();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<NavMeshAgent>() != null)
        {
            if (other.CompareTag("enemy prefab") && targetCount < 1 && other.GetComponent<NavMeshAgent>().enabled /*&& Vector3.Distance(transform.position,target.transform.position ) < helperColl.bounds.size.magnitude*/)
            {
                targetCount += 1;
                target = other.gameObject;
                //agent.SetDestination(target);
            }
        }



    }
    IEnumerator attack()
    {
        if (target.GetComponent<IDamage>() != null && Vector3.Distance(transform.position, target.transform.position) < attackDist)
        {
            anim.SetTrigger("Attack");
            agent.speed = 0;
            target.GetComponent<IDamage>().takeDamage(damage);
        }
        yield return new WaitForSeconds(attackInterval);
        attacking = false;
        agent.speed = speed;
    }

    void faceTarget()
    {
        targetDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    public void takeDamage(int dmg)
    {
        Hp -= damage;
        StartCoroutine(flashDamage());
        if (Hp <= 0)
        {
            gameManager.instance.minionList.Remove(gameObject);
            Destroy(gameObject);
        }
    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        model.material.color = Color.white;
    }
    IEnumerator die()
    {
        yield return new WaitForSeconds(timer);
        gameManager.instance.minionList.Remove(gameObject);
        Destroy(gameObject);
    }
    public void blackHole()
    {

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class shieldSpawn : MonoBehaviour, IDamage
{
    [Header("-------------------components---------------------------")]
    [SerializeField] Renderer model;
    [SerializeField] Collider collision;
    [SerializeField] GameObject host;
    [SerializeField] Animator hostAnim;
    [SerializeField] NavMeshAgent agent;


    [Header("-----------stats-------------")]
    [SerializeField] int armor;
    [SerializeField] float respawnTime;

    int armorCount;

    // Start is called before the first frame update
    void Start()
    {

        armorCount = armor;
        model.material.color = Color.cyan;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int dmg)
    {
        armor -= dmg;
        if (armor < armorCount)
        {
            model.material.color = Color.blue;
        }

        else if (armor <= (armorCount * .6666))
        {
            model.material.color = Color.magenta;
        }
        else if(armor <= (armorCount * .3333))
        {
            model.material.color = Color.red;
        }
        

        if (armor <= 0)
        {
            model.enabled = false;
            collision.enabled = false;
            agent.enabled = false;
            StartCoroutine(respawn());
            hostAnim.SetBool("Stunned", true);
        }
        
    }

    IEnumerator respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        armor = armorCount;
        model.material.color = Color.cyan;
        model.enabled = true;
        collision.enabled = true;
        hostAnim.SetBool("Stunned", false);
        agent.enabled = true;


    }
}

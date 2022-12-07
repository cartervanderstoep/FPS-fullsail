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
    [SerializeField] enemyAI hostScript;
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
        model.material.color = new Color(Color.cyan.r,Color.cyan.g,Color.cyan.b, .5f);
        
        
    }

    // Update is called once per frame
    void Update()
    {
      if (hostScript.getHP() <=0)
        {
            hostAnim.SetBool("Stunned", false);
            Destroy(gameObject);
        }
      
    }

    public void takeDamage(int dmg)
    {
        armor -= dmg;
        if (armor < armorCount)
        {
            model.material.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, .5f);
        }

        else if (armor <= (armorCount * .6666))
        {
            model.material.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b,.5f);
        }
        else if(armor <= (armorCount * .3333))
        {
            model.material.color = new Color(Color.red.r, Color.red.g, Color.red.b, .5f);
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
        model.material.color = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, .5f);
        model.enabled = true;
        collision.enabled = true;
        hostAnim.SetBool("Stunned", false);
        agent.enabled = true;


    }
}

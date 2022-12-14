using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionBehavior : MonoBehaviour
{
    [SerializeField] int explosionDamage;
    [SerializeField] float volume;
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip boom;
    [SerializeField] int forceAmount;
    public GameObject explosion;
    bool isExploding = false;
   
    // Start is called before the first frame update
    private void Start()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        aud.PlayOneShot(boom, volume);
        explode(gameObject.transform.position, explosionDamage * 2);
        StartCoroutine(wait());
    }

   



    



 


    void explode(Vector3 center, float radius)
    {
        Collider[] Colliders = Physics.OverlapSphere(center, radius);
        
        foreach (var entity in Colliders)
        {
            if (entity.CompareTag("Player"))
            {
                if (explosionDamage - Vector3.Distance(gameObject.transform.position, entity.transform.position) >= 0)
                {
                    gameManager.instance.playerScript.damage(explosionDamage - (int)Vector3.Distance(gameObject.transform.position, entity.transform.position));
                    gameManager.instance.playerScript.pushBack = (entity.transform.position - transform.position).normalized * forceAmount;
                }
            }
            if (entity.GetComponent<IDamage>() != null)
            {
                if (explosionDamage - Vector3.Distance(gameObject.transform.position, entity.transform.position) >= 0)
                {
                    entity.GetComponent<Collider>().GetComponent<IDamage>().takeDamage(explosionDamage - (int)Vector3.Distance(gameObject.transform.position, entity.transform.position));
                    
                }
            }



        }
        isExploding = false;
        StartCoroutine(wait());
    }



    IEnumerator wait()
    {

        yield return new WaitForSeconds(.2f);
        if (isExploding == false)
        {
            Destroy(gameObject);
        }
    }
}

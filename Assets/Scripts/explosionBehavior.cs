using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionBehavior : MonoBehaviour
{
    [SerializeField] int explosionDamage;
    public GameObject explosion;
    bool isExploding = false;
    // Start is called before the first frame update

    private void Update()
    {
        
        StartCoroutine(wait());
    }



    private void OnTriggerEnter(Collider other)
    {
        isExploding = true;
        Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
        if (other.CompareTag("Player"))
        {
            if (explosionDamage - Vector3.Distance(gameObject.transform.position, other.transform.position) >= 0)
            {
                gameManager.instance.playerScript.damage(explosionDamage - (int)Vector3.Distance(gameObject.transform.position, other.transform.position));
            }
        }
          if(other.GetComponent<IDamage>() != null)
        {
            if (explosionDamage - Vector3.Distance(gameObject.transform.position, other.transform.position) >= 0)
            {
                other.GetComponent<Collider>().GetComponent<IDamage>().takeDamage(explosionDamage - (int)Vector3.Distance(gameObject.transform.position, other.transform.position));
            }
        }

          isExploding = false;
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {

        yield return new WaitForSeconds(1f);
        if (isExploding == false)
        {
            Destroy(gameObject);
        }
    }
}
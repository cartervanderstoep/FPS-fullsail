using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class blackHole : MonoBehaviour
{
    [SerializeField] float blackHoleTime;
    [SerializeField] float blackHoleRadius;
    [SerializeField] float damageInterval;
    [SerializeField] int damage;


    bool hurt;
    

    // Start is called before the first frame update
    void Start()
    {
          Destroy(gameObject,blackHoleTime);
    }

    // Update is called once per frame
    void Update()
    {
        suction(transform.position, blackHoleRadius);
    }

    void suction(Vector3 center, float radius)
    {
        Collider[] Colliders = Physics.OverlapSphere(center, radius);

        foreach (var entity in Colliders)
        {
            
            if (entity.GetComponent<IDamage>() != null)
            {
                if (radius - Vector3.Distance(gameObject.transform.position, entity.transform.position) >= 0)
                {
                    entity.GetComponent<IDamage>().blackHole();
                    entity.GetComponent<NavMeshAgent>().speed = (entity.GetComponent<NavMeshAgent>().speed / 2);
                    entity.GetComponent<NavMeshAgent>().SetDestination(transform.position);
                    if (!hurt)
                    {
                        hurt =true;
                        StartCoroutine(damageOverTime(entity.gameObject));
                    }

                }
            }



        }

    }
    IEnumerator damageOverTime(GameObject victim)
    {
        victim.GetComponent<IDamage>().takeDamage(damage);
        yield return new WaitForSeconds(damageInterval);
        hurt=false;
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class magicBullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int timer;

    bool homingChanged;

    private void Start()
    {
        homingChanged = false;
        //rb.velocity = transform.forward * speed;
        Destroy(gameObject, timer);
    }

    private void Update()
    {
           if (!homingChanged)
         {
            StartCoroutine(homing());

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            gameManager.instance.playerScript.damage(damage);
        }

        Destroy(gameObject);
    }

     IEnumerator homing()
    {
        homingChanged = true;
        transform.Translate(Vector3.Lerp(gameObject.transform.position, gameManager.instance.player.transform.position, speed *Time.deltaTime));
       transform.Translate(Vector3.MoveTowards(transform.position,gameManager.instance.player.transform.position, speed * Time.deltaTime));

        yield return new WaitForSeconds(.1f);
        homingChanged = false;
    }
}


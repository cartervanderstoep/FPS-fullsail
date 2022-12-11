using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hurtBox : MonoBehaviour , IDamage
{
    [SerializeField] GameObject childEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      //  transform.position = childEnemy.transform.position; 
    }
    public void takeDamage(int dmg)
    {
        Debug.Log("hit");
        //if (childEnemy.GetComponent<IDamage>() != null)
        //{
            childEnemy.GetComponent<IDamage>().takeDamage(dmg);
      //  }

    }
    public void blackHole()
    {

    }
}


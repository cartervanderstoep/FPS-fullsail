using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveBarrel : MonoBehaviour , IDamage
{
    [SerializeField] int HP;
    [SerializeField] GameObject explosion;
    [SerializeField] Transform barrelTransform;

   public void takeDamage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0)
        {
             Instantiate(explosion,barrelTransform.position, transform.rotation);
            Destroy(gameObject);
        }

    }
    public void blackHole()
    {

    }

}

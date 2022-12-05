using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2Lever : MonoBehaviour,IDamage
{
    [SerializeField] GameObject door;

    Vector3 leverPos;
    Vector3 doorPos;
    Vector3 newLeverPos;
    Vector3 newDoorPos;


    // Start is called before the first frame update
    void Start()
    {
        leverPos = transform.position;
        doorPos = door.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, leverPos) > 0.1f)
        {
            //  transform.Translate(Vector3.Lerp(newLeverPos, leverPos, 1 * Time.deltaTime));
            transform.Translate(0, .45f * Time.deltaTime, 0);
        }
        if (Vector3.Distance(door.transform.position, doorPos) > 0.1f)
        {
          //  door.transform.Translate(Vector3.Lerp(door.transform.position, doorPos, .5f * Time.deltaTime));
           door.transform.Translate(-.3f*Time.deltaTime, 0, 0);
        }


    }
    public void takeDamage(int dmg)
    {
        
        transform.Translate( 0, ((float)(-dmg) * .75f), 0);
        door.transform.Translate( (float)(dmg * .5f),0,0);
        newDoorPos = door.transform.position;
        newLeverPos = transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleDestroy : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(gameManager.instance.keyCount == 3)
        {
            Destroy(gameObject);
        }
    }
}

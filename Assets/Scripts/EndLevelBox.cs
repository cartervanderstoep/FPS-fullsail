using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelBox : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        gameManager.instance.youWin();
        Destroy(gameObject);
    }

}

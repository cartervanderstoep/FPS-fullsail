using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDialougeBoxTrigger : MonoBehaviour
{
    [SerializeField] levelDialougeBox box;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(box.levelNum == 1)
            {
                gameManager.instance.townLevelDialogue();
            }
            if (box.levelNum == 2)
            {
                gameManager.instance.treeLevelDialogue();
            }
            if (box.levelNum == 3)
            {
                gameManager.instance.volcanoLevelDialogue();
            }
            if (box.levelNum == 4)
            {
                gameManager.instance.castleLevelDialogue();
            }
           
            Destroy(gameObject);
        }
    }
}

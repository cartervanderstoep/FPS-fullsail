using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpawn : MonoBehaviour
{
    public GameObject spawn;
    public GameObject checkpointFlash;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawn.transform.position = this.transform.position;
        }
    }
    public IEnumerator checkpointFlashScreen()
    {
        checkpointFlash.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        checkpointFlash.SetActive(false);
    }
}

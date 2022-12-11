using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogePopper : MonoBehaviour
{
    public GameObject Things;
    public float time;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(timer());
        }
    }
    public IEnumerator timer()
    {
        Things.SetActive(true);
        yield return new WaitForSeconds(time);
        Things.SetActive(false);
    }
}

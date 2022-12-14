using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMover : MonoBehaviour
{
    public GameObject Player;
    public GameObject Target;
    public float time;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LerpPosition(Target.transform.position, time));
        }
    }
    
    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = this.transform.position;
        while (time < duration)
        {
            Player.transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        Player.transform.position = targetPosition;
    }

}

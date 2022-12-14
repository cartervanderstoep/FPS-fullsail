using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public GameObject face;
    void Update()
    {
        this.transform.rotation = Quaternion.LookRotation(this.transform.position - face.transform.position);
    }
}

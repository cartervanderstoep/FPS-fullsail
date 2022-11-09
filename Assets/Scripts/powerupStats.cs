using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class powerupStats : ScriptableObject
{
    public float speedMultiplier;
    public float jumpHeightIncrease;
    public int jumpNumberIncrease;
    public float duration;
    public GameObject powerupModel;
    public GameObject pickupEffect;
    public AudioClip powerupSound;
}

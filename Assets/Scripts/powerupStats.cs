using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class powerupStats : ScriptableObject
{
    public int speedMultiplier;
    public int jumpHeightIncrease;
    public int jumpNumberIncrease;
    public GameObject powerupModel;
    public GameObject pickupEffect;
    public AudioClip powerupSound;
}

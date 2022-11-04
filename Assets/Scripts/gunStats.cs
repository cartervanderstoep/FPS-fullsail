using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public float gunShootRate;
    public int gunDist;
    public int gunShootDmg;
    public int gunAmmo;
    public GameObject gunModel;
    public GameObject hitEffect;
    public GameObject muzzleFlash;
    public AudioClip gunSound;
}

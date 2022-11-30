using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class gunStats : ScriptableObject
{
    public int wandAmmo;
    public float castRate;
    public int magicElement; 
    public GameObject wandModel;
    public GameObject magicType; 
    public GameObject hitEffect;
    public GameObject castFlash;
    public AudioClip spellSound;
}

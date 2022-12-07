using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class sheild : MonoBehaviour
{
    [Header("------ Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] Collider collisionDet;
    [Header("----- Stats -----")]
    [SerializeField] int defense;
    [Range(0, 60)][SerializeField] float rebuildTime; 

    int defenseOG;

    // Start is called before the first frame update
    void Start()
    {
        defenseOG = defense;
        model.material.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.2f); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator rebuild()
    {
        yield return new WaitForSeconds(rebuildTime);
        defense = defenseOG;
        model.enabled = true;
        collisionDet.enabled = true; 
        model.material.color = new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.5f);
    }
}

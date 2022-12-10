using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossenable : MonoBehaviour
{
    [Header("----------components---------")]
    [SerializeField] GameObject boss;
    [SerializeField, Range(1f, 2f)] int bossIntro;
    [SerializeField] AudioSource aud;

    [Header("------------- boss intros---------")]
    [SerializeField] AudioClip boss1;
    [SerializeField] AudioClip boss2;

    [SerializeField] float bossVolume;


    int bossesSpawned;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (bossesSpawned < 1 && !other.CompareTag("Enemy")&& !other.CompareTag("enemy prefab" ))
        {
            boss.SetActive(true);

            switch (bossIntro)
            {
                case (1):
                    {
                        aud.PlayOneShot(boss1, bossVolume);
                        break;
                    }
                case (2):
                    {
                        aud.PlayOneShot(boss2, bossVolume);
                        break;
                    }
            }
            bossesSpawned++;
        }

    }
}

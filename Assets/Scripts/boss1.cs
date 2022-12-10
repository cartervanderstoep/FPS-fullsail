using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class boss1 : MonoBehaviour, IDamage
{
    [Header("---------components----------")]
    [SerializeField] List<Transform> jumpSpots = new List<Transform>();
    [SerializeField] GameObject vertWave;
    [SerializeField] GameObject wave;
    [SerializeField] AudioSource aud;

    [Header("----------stats--------")]
    [SerializeField] int Health;
    [SerializeField] float rotationSpeed;
    [SerializeField] float speedSpeed;

    [Header("---------- sound parts---------")]
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip[] hurt;
    [SerializeField] AudioClip vertShot;
    [SerializeField] AudioClip dead;
    [SerializeField] AudioClip waveSound;
    [SerializeField] float volume;


    Transform target;
    float jumpDist;
    Vector3 jumpDir;
    Vector3 playerDir;
    Vector3 fallposition;
    bool falling;
    bool waiting;
    bool shootWave;
    bool shootThree;

    int attackChoice;
    int waveCount;
    int jumpCount;
    int fallCount;

    // Start is called before the first frame update
    void Start()
    {
        
        jumpCount = 0;
        jumpDist = Vector3.Distance(gameObject.transform.position, jumpSpots[jumpCount + 1].transform.position);
        target = jumpSpots[jumpCount + 1];
        waiting = false;

        falling = false;
        gameManager.instance.enemiesToKill++;
        gameManager.instance.updateUI();


    }

    // Update is called once per frame
    void Update()
    {
        jumpDir = target.position - transform.position;
        playerDir = (gameManager.instance.player.transform.position) - transform.position;

        

        if (!falling && Vector3.Distance(transform.position, target.position) > (jumpDist / 2))
        {
            jumpRotation();
            jump();
        }
        else if (Vector3.Distance(transform.position, target.position) <= (jumpDist / 2) && Vector3.Distance(transform.position, target.position) > 0)
        {
            if (fallCount < 1)
            {
                fallCount++;
                fallposition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            jumpRotation();
            fall();
        }

        else
        {
            facePlayer();

            if (shootWave)
            {
                damageWave();
                cycleJump();
            }
            else if (shootThree)
            {
                shootThree=false;
                StartCoroutine(tripleShot());
            }
            else if (waiting == false)
            {
                chooseAttack();
                waiting = true;
                StartCoroutine(wait());

            }


        }




    }
    void jump()
    {

        transform.position = Vector3.Slerp(transform.position, target.position, speedSpeed * Time.deltaTime);
        transform.Translate(Vector3.up * speedSpeed * Time.deltaTime);

    }
    void fall()
    {
        if (!falling)
        {
            falling = true;
        }
        
        //transform.position = Vector3.Lerp(fallposition, target.position, speedSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, target.position, (speedSpeed*10) * Time.deltaTime);

    }
    public void takeDamage(int dmg)
    {
        Health -= dmg;
        int randNoise = Random.Range(0,hurt.Length);
        aud.PlayOneShot(hurt[randNoise], volume);
        if (Health <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.youWin();
            gameManager.instance.updateEnemyNumber();
            gameManager.instance.updateUI();
        }

    }
    void cycleJump()
    {


        if (jumpCount + 1 < jumpSpots.Count)
        {
            jumpCount++;
            target = jumpSpots[jumpCount];
        }
        else
        {
            jumpCount = 0;
            target = jumpSpots[0];
        }
        jumpDist = Vector3.Distance(gameObject.transform.position, target.position);
        falling = false;
    }
    void facePlayer()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    void quickFace()
    {
        playerDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(playerDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed);
    }
    void jumpRotation()
    {
        jumpDir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(jumpDir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        cycleJump();

        waiting = false;
    }

    void chooseAttack()
    {
        attackChoice = Random.Range(1, 4);
       

        switch (attackChoice)
        {
            case 1:
                shootThree = true;
                break;
            case 2:
                shootWave = true;
                break;

            //case 3:
            //    slam();
            //    break;
            default:
                break;

        }

    }
    IEnumerator tripleShot()
    {

        quickFace();  
        Vector3 spawnPos = new Vector3(transform.position.x, gameManager.instance.player.transform.position.y, transform.position.z);
        aud.PlayOneShot(attack, volume);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(.3f);
            Quaternion rotation = transform.rotation;
            Instantiate(vertWave, spawnPos, rotation);
            aud.PlayOneShot(vertShot, volume);
        }

    }
    void damageWave()
    {
        if (waveCount < 3)
        {
            quickFace();
            Vector3 spawnPos = new Vector3(transform.position.x, gameManager.instance.player.transform.position.y, transform.position.z);
            aud.PlayOneShot(waveSound, volume);
            Instantiate(wave, spawnPos, transform.rotation);
            waveCount++;
        }
        else
        {
            shootWave = false;
            waveCount = 0;
        }

    }
    //void slam()
    //{
    //    target = gameManager.instance.player.transform;

    //    jumpDist = Vector3.Distance(gameObject.transform.position, target.position);
    //    falling = false;
    //    if (jumpCount +1 < jumpSpots.Count)
    //    {
    //        jumpCount++;
    //    }
    //    else
    //    {
    //        jumpCount = 0;
    //    }
    //}
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 100)][SerializeField] float playerSpeed;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(8, 20)][SerializeField] float jumpHeight;
    [Range(0, 35)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpsMax;

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> guns = new List<gunStats>();
    [SerializeField] GameObject hitEffect;

    Vector3 move;
    private Vector3 playerVelocity;
    int jumpsTimes;
    int HPOrig;
    int selectedGun; 
    bool isSprinting;
    bool isShooting;
    float playerSpeedOrig;
    int jumpsMaxOrig;
    float jumpHeightOrig;
    

    private void Start()
    {
        
        jumpHeightOrig = jumpHeight;
        jumpsMaxOrig = jumpsMax;
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        respawn(); 
    }

    void Update()
    {
        movement();
        sprint();
        StartCoroutine(shoot());
        gunSelect(); 
    }

    void movement()
    {

        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpsTimes = 0;
            playerVelocity.y = 0f;
        }

        move = transform.right * Input.GetAxis("Horizontal") +
               transform.forward * Input.GetAxis("Vertical");

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpsTimes < jumpsMax)
        {
            jumpsTimes++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            playerSpeed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed /= sprintMod;
            isSprinting = false;
        }
    }

    IEnumerator shoot()
    {
        if (guns.Count > 0 && !isShooting && Input.GetButton("Shoot"))
        {
            isShooting = true;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                {
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
                }
                Instantiate(hitEffect, hit.point, hitEffect.transform.rotation);
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void damage(int dmg)
    {
        HP -= dmg;
            StartCoroutine(gameManager.instance.playerDamageFlash());
        if(HP <= 0)
        {
                gameManager.instance.playerDeadMenu.SetActive(true);
                gameManager.instance.pause();   
        }
    }
    public void gunPickup(gunStats gunStatx)
    {
        shootRate = gunStatx.gunShootRate;
        shootDamage = gunStatx.gunShootDmg;
        shootDist = gunStatx.gunDist;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunStatx.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStatx.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        guns.Add(gunStatx);
    }
    public void powerupPickup(powerupStats powerupstat)
    {
        if(powerupstat.speedMultiplier != 0)
        {
            playerSpeed *= powerupstat.speedMultiplier;
            
        }
        if(powerupstat.jumpHeightIncrease!= 0)
        {
            jumpHeight += powerupstat.jumpHeightIncrease;
        }
        if(powerupstat.jumpNumberIncrease !=0)
        {
            jumpsMax += powerupstat.jumpNumberIncrease;
        }
        StartCoroutine(gameManager.instance.playerPowerupFlash(powerupstat.duration));
        
        //if(isSprinting == true)
        //{
        //    playerSpeed = playerSpeedOrig * sprintMod;
        //}
        //else
        //{
        //    playerSpeed = playerSpeedOrig;
        //}
        //jumpHeight = jumpHeightOrig;
        //jumpsMax = jumpsMaxOrig;

    }
    public IEnumerator powerupActivate(powerupStats Pup)
    {
        playerSpeed *= Pup.speedMultiplier;
        StartCoroutine(gameManager.instance.playerPowerupFlash(Pup.duration));
        yield return new WaitForSeconds(Pup.duration);
        playerSpeed = playerSpeedOrig;
    }
    public void powerupActPlayer(powerupStats alt)
    {
        StartCoroutine(powerupActivate(alt));
    }
    void gunSelect()
    {
        if (guns.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < guns.Count - 1)
            {
                selectedGun++;
                changeGuns();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
            {
                selectedGun--;
                changeGuns();
            }
        }
    }
    void changeGuns()
    {
        shootRate = guns[selectedGun].gunShootRate;
        shootDamage = guns[selectedGun].gunShootDmg;
        shootDist = guns[selectedGun].gunDist;
        gunModel.GetComponent<MeshFilter>().sharedMesh = guns[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = guns[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void respawn()
    {
        controller.enabled = false;
        HP = HPOrig;
        transform.position = gameManager.instance.spawnPos.transform.position;
        gameManager.instance.playerDeadMenu.SetActive(false);
        controller.enabled = true;
    }
    public void healPickUp(healStats healStat)
    {
        if (HP + healStat.healValue > HPOrig && HP != HPOrig)
        {
            int netHeal = HPOrig - HP;

            HP += netHeal;
        }
        else if (HP + healStat.healValue <= HPOrig)
        {
            HP += healStat.healValue;
        }
    }
   
}
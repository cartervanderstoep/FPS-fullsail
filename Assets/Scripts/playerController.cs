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
    [Range(1, 60)][SerializeField] float playerStamina;
    [Range(1, 60)][SerializeField] float hoverTime;

    [Header("----- Player Physics -----")]
    [SerializeField] int pushBackTime; 

    [Header("----- Gun Stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> guns = new List<gunStats>();
    [SerializeField] GameObject hitEffect;

    Vector3 move;
    public Vector3 pushBack; 
    private Vector3 playerVelocity;
    int jumpsTimes;
    int HPOrig;
    int selectedGun; 
    int jumpsMaxOrig;
    float playerSpeedOrig;
    float jumpHeightOrig;
    float pStaminaOG;
    float gravityOG;
    float hoverTimeOG; 
    bool isSprinting;
    bool isShooting;
    bool isJumping;
    bool isHovering; 
    

    private void Start()
    {
        hoverTimeOG = hoverTime; 
        gravityOG = gravityValue; 
        pStaminaOG = playerStamina; 
        jumpHeightOrig = jumpHeight;
        jumpsMaxOrig = jumpsMax;
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        respawn(); 
    }

    void Update()
    {
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime); 
        movement();
        if (isHovering)
        {
            hoverTime = Mathf.Lerp(hoverTime, 0, Time.deltaTime * 3);
        }
        if (hoverTime < hoverTimeOG && !isHovering)
        {
            hoverTime = Mathf.Lerp(hoverTime, hoverTimeOG + 0.1f, Time.deltaTime);
        }

        sprint();
        if(isSprinting)
        {
            playerStamina = Mathf.Lerp(playerStamina, 0, Time.deltaTime * 3);
        }
        if(playerStamina < pStaminaOG && !isSprinting)
        {
            playerStamina = Mathf.Lerp(playerStamina, pStaminaOG, Time.deltaTime);
        }
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

        controller.Move((move + pushBack) * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && jumpsTimes < jumpsMax)
        {
            isJumping = true; 
            jumpsTimes++;
            playerVelocity.y = jumpHeight;
        }

        if (Input.GetButtonDown("Hover") && hoverTime > 0.1)
        {
            if(hoverTime > 0)
            {
                isHovering = true;
                gravityValue = 0;
                playerVelocity.y = 0;
            }
        }
        else if (Input.GetButtonUp("Hover"))
        {
            isHovering = false;
            gravityValue = gravityOG;
            playerVelocity.y -= gravityValue * Time.deltaTime;
        }
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        isJumping = false;
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && playerStamina > 0.1)
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
        updatePlayerHPBar();
            StartCoroutine(gameManager.instance.playerDamageFlash());
        if(HP <= 0)
        {
                gameManager.instance.playerDeadMenu.SetActive(true);
                gameManager.instance.pause();   
        }
    }
    void updatePlayerHPBar()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;
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
    
    public IEnumerator powerupActivate(powerupStats Pup)
    {
        playerSpeed *= Pup.speedMultiplier;
        jumpsMax += Pup.jumpNumberIncrease;
        jumpHeight = Pup.jumpHeightIncrease;
        StartCoroutine(gameManager.instance.playerPowerupFlash(Pup.duration));
        yield return new WaitForSeconds(Pup.duration);
        if(isSprinting == true)
        {
            playerSpeed = playerSpeedOrig * sprintMod;
        }
        else
        {
            playerSpeed = playerSpeedOrig;
        }
        jumpHeight = jumpHeightOrig;
        jumpsMax = jumpsMaxOrig;
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
        updatePlayerHPBar();
        transform.position = gameManager.instance.spawnPos.transform.position;
        gameManager.instance.playerDeadMenu.SetActive(false);
        controller.enabled = true;
    }
    public void healPickUp(healStats healStat)
    {
        StartCoroutine(gameManager.instance.playerHealthupFlash());

        if (HP + healStat.healValue > HPOrig && HP != HPOrig)
        {
            int netHeal = HPOrig - HP;

            HP += netHeal;
        }
        else if (HP + healStat.healValue <= HPOrig)
        {
            HP += healStat.healValue;
        }
        updatePlayerHPBar();
    }
   
}
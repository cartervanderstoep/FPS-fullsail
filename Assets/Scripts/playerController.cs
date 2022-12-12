using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;

    [Header("----- Player Stats -----")]
    [Range(0, 10)][SerializeField] int HP;
    [Range(1, 100)][SerializeField] float playerSpeed;
    [Range(1.5f, 5)][SerializeField] float sprintMod;
    [Range(8, 20)][SerializeField] float jumpHeight;
    [Range(0, 35)][SerializeField] float gravityValue;
    [Range(1, 3)][SerializeField] int jumpsMax;
    [Range(1, 60)][SerializeField] float playerStamina;
    [Range(1, 60)][SerializeField] float utilityTime;
    [SerializeField] float dashDist;
    [SerializeField] bool canFloat;
    [SerializeField] bool canTeleport;
    [SerializeField] bool canDefend; 

    [Header("----- Player Physics -----")]
    [SerializeField] int pushBackTime; 

    [Header("----- Spell Stats -----")]
    [SerializeField] float castingRate;
    [SerializeField] float castingDist; 
    [SerializeField] GameObject magicAttk;
    
    [Header("----- Spell Utilities -----")]
    [Range(1, 5)][SerializeField] int magicElem; 
    [SerializeField] GameObject equipable;
    [SerializeField] GameObject playerSheild; 
    [SerializeField] List<gunStats> spells = new List<gunStats>();
    [SerializeField] Transform castOrigin;

    [Header("----- Sound -----")]
    [SerializeField] AudioClip[] jumps;
    [SerializeField] AudioClip[] hurts;
    [SerializeField] float volume;

    [Header("----- Special FX -----")]
    [SerializeField] Transform fXOrigin;
    [SerializeField] Transform utilityFXOrigin; 
    [SerializeField] GameObject castEffect;
    [SerializeField] GameObject hoverEffect;
    [SerializeField] GameObject teleportEffect;
    [SerializeField] GameObject sheildEffect;
    [SerializeField] GameObject teleportFlash; 
    [SerializeField] AudioClip spellAud;
    [SerializeField] AudioClip floatAud; 
    [SerializeField] AudioClip teleportAud;
    [SerializeField] AudioClip shieldAud; 

    Vector3 move;
    Vector3 dashDir;
    Transform utilityPos; 
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
    float utilityTimeOG;
    float magicDropSpeed; 
    bool isSprinting;
    bool isShooting;
    bool isJumping;
    bool isHovering;
    bool isTeleporting;
    bool isDefending; 
    bool canF;
    bool canT;
    bool canD; 
    

    private void Start()
    {
        utilityTimeOG = utilityTime; 
        gravityOG = gravityValue; 
        pStaminaOG = playerStamina; 
        jumpHeightOrig = jumpHeight;
        jumpsMaxOrig = jumpsMax;
        playerSpeedOrig = playerSpeed;
        HPOrig = HP;
        canF = canFloat;
        canT = canTeleport;
        canD = canDefend; 
        respawn(); 
    }

    void Update()
    {
        utilityPos = utilityFXOrigin;
        pushBack = Vector3.Lerp(pushBack, Vector3.zero, Time.deltaTime * pushBackTime); 
        movement();
        //Hover bar
        float utilitySub = Time.deltaTime * 4; 
        if (isHovering)
        {
            utilitySub = -Time.deltaTime * 6; 
        }
        
        //Teleport Bar
        else if (isTeleporting)
        {
            utilitySub = -utilityTimeOG / 2;
        }
        
        //Shield bar
        else if (isDefending)
        {
            utilitySub = -Time.deltaTime * 6;
        }
        utilityTime += utilitySub; 
        if(utilityTime > utilityTimeOG)
        {
            utilityTime = utilityTimeOG; 
        }

        updatePlayerHoverBar();
        StartCoroutine(sprint());
        if(isSprinting)
        {
            playerStamina -= Time.deltaTime * 6;
            updatePlayerStaminaBar();
        }
        if(playerStamina < pStaminaOG && !isSprinting)
        {
            playerStamina += Time.deltaTime * 4;
            updatePlayerStaminaBar();
        }
        StartCoroutine(shoot());
        gunSelect(); 
    }

    void movement()
    {
        Debug.Log("Jumps Remaining: " + jumpsTimes);
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            jumpsTimes = 0;
            playerVelocity.y = 0f;
        }
        //Normal Movement
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move((move + pushBack) * Time.deltaTime * playerSpeed);

        StartCoroutine(utility()); 
        
        //Jump
        if (Input.GetButtonDown("Jump") && jumpsTimes < jumpsMax)
        {
            isJumping = true; 
            jumpsTimes++;
            playerVelocity.y = jumpHeight;
            aud.PlayOneShot(jumps[Random.Range(0,jumps.Length)], volume);
        }
        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
        isJumping = false;
    }

    IEnumerator sprint()
    {
        if(Input.GetButtonDown("Sprint")&& playerStamina > 0.1f)
        {
            if(playerStamina > 0.1f)
            {
                playerSpeed *= sprintMod;
                isSprinting = true;
            }
        }
        if(playerStamina <= 0.1)
        {
            isSprinting = false;
            playerSpeed = playerSpeedOrig;
            yield return new WaitForSeconds(2f);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            playerSpeed = playerSpeedOrig;
            isSprinting = false;
            yield return new WaitForSeconds(2f);
        }
    }
    

    IEnumerator shoot()
    { 
        if (spells.Count > 0 && !isShooting && Input.GetButton("Shoot"))
        {
            if(magicElem == 1 || magicElem == 2)
            {
                isShooting = true;
                aud.PlayOneShot(spellAud, volume);
                Instantiate(castEffect, fXOrigin.position, transform.rotation);
                Instantiate(magicAttk, castOrigin.position, Camera.main.transform.rotation);
                yield return new WaitForSeconds(castingRate);
                isShooting = false;
            }
            else if (magicElem == 3 || magicElem == 4)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, castingDist))
                {
                    isShooting = true; 
                    Vector3 dropLoc = hit.point;
                    dropLoc.y += 20f;
                    aud.PlayOneShot(spellAud, volume); 
                    Instantiate(castEffect, dropLoc, transform.rotation); 
                    Instantiate(magicAttk, dropLoc, transform.rotation);
                    yield return new WaitForSeconds(castingRate); 
                    magicAttk.transform.Translate(Vector3.down * magicDropSpeed);
                    isShooting = false; 
                }
            }
            else if (magicElem == 5)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, castingDist))
                {
                    isShooting = true;
                    aud.PlayOneShot(spellAud, volume);
                    Instantiate(castEffect, hit.point, transform.rotation);
                    Instantiate(magicAttk, hit.point, transform.rotation);
                    yield return new WaitForSeconds(castingRate);
                    isShooting = false;
                }
            }
        }
    }
    IEnumerator utility()
    {
        if (canF && !canT && !canD)
        {
            //Floating
            if (Input.GetButtonDown("Hover") && utilityTime > 0.1f && canF)
            {
                if (utilityTime > 0.1f)
                {
                    isHovering = true;
                    aud.PlayOneShot(floatAud, volume); 
                    gravityValue = 0;
                    playerVelocity.y = 0;
                    yield return new WaitForSeconds(2f);
                }
            }
            if (utilityTime <= 0.1f)
            {
                isHovering = false;
                gravityValue = gravityOG;
                playerVelocity.y -= gravityValue * Time.deltaTime;
                yield return new WaitForSeconds(2f);
            }
            else if (Input.GetButtonUp("Hover"))
            {
                isHovering = false;
                gravityValue = gravityOG;
                playerVelocity.y -= gravityValue * Time.deltaTime;
            }
        }
        if (canT && !canF && !canD)
        {
            //Teleport
            if (Input.GetButtonDown("Hover") && utilityTime > 0.1f && canT)
            {
                if (utilityTime > 0.1f)
                {
                    isTeleporting = true;
                    aud.PlayOneShot(teleportAud, volume);
                    StartCoroutine(utilityFlashF()); 
                    dashDir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                    controller.Move((dashDir + pushBack) * Time.deltaTime * (playerSpeed * dashDist));
                    yield return new WaitForSeconds(2f);
                }
                move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            }
            if (utilityTime <= 0.1f)
            {
                isTeleporting = false;
                controller.Move((move + pushBack) * Time.deltaTime * playerSpeed);
                yield return new WaitForSeconds(2f);
            }
            else if (Input.GetButtonUp("Hover"))
            {
                isTeleporting = false;
                move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
                yield return new WaitForSeconds(2f);
            }
        }
        if(canD && !canF && !canT)
        {
            //Sheild
            if (Input.GetButtonDown("Hover") && utilityTime > 0.1f && canD)
            {
                if (utilityTime > 0.1f)
                {
                    isDefending = true;
                    aud.PlayOneShot(shieldAud, volume); 
                    playerSheild.SetActive(true);
                    yield return new WaitForSeconds(2f);
                }
            }
            if (utilityTime <= 0.1f)
            {
                isDefending = false;
                playerSheild.SetActive(false);
                yield return new WaitForSeconds(2f);
            }
            else if (Input.GetButtonUp("Hover"))
            {
                isDefending = false;
                playerSheild.SetActive(false);
                yield return new WaitForSeconds(2f);
            }
        }
        if (canT && canF && canD)
        {
            canFloat = false;
            canTeleport = false;
            canDefend = false; 
        }
    }

    public IEnumerator utilityFlashF()
    {
        teleportFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        teleportFlash.SetActive(false); 
    }
    public void damage(int dmg)
    {
        HP -= dmg;
        updatePlayerHPBar();
            StartCoroutine(gameManager.instance.playerDamageFlash());
        aud.PlayOneShot(hurts[Random.Range(0, hurts.Length)], volume);
        if (HP <= 0)
        {
                gameManager.instance.playerDeadMenu.SetActive(true);
                gameManager.instance.pause();   
        }
    }
    void updatePlayerHPBar()
    {
        gameManager.instance.HPBar.fillAmount = (float)HP / (float)HPOrig;
    }
    void updatePlayerStaminaBar()
    {
        gameManager.instance.staminaBar.fillAmount = (float)playerStamina / (float)pStaminaOG;
    }
    void updatePlayerHoverBar()
    {
        gameManager.instance.hoverBar.fillAmount = (float)utilityTime / (float)utilityTimeOG;
    }

    public void gunPickup(gunStats gunStatx)
    {
        magicAttk = gunStatx.magicType;
        castingRate = gunStatx.castRate;
        magicElem = gunStatx.magicElement;
        castEffect = gunStatx.castFlash;
        spellAud = gunStatx.spellSound; 
        equipable.GetComponent<MeshFilter>().sharedMesh = gunStatx.wandModel.GetComponent<MeshFilter>().sharedMesh;
        equipable.GetComponent<MeshRenderer>().sharedMaterial = gunStatx.wandModel.GetComponent<MeshRenderer>().sharedMaterial;
        spells.Add(gunStatx);
    }
    
    public IEnumerator powerupActivate(powerupStats Pup)
    {
        playerSpeed *= Pup.speedMultiplier;
        jumpsMax += Pup.jumpNumberIncrease;
        jumpHeight += Pup.jumpHeightIncrease;
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
        if (spells.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < spells.Count - 1)
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
        magicAttk = spells[selectedGun].magicType;
        magicElem = spells[selectedGun].magicElement;
        castingRate = spells[selectedGun].castRate;
        castEffect = spells[selectedGun].castFlash;
        spellAud = spells[selectedGun].spellSound; 
        equipable.GetComponent<MeshFilter>().sharedMesh = spells[selectedGun].wandModel.GetComponent<MeshFilter>().sharedMesh;
        equipable.GetComponent<MeshRenderer>().sharedMaterial = spells[selectedGun].wandModel.GetComponent<MeshRenderer>().sharedMaterial;
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
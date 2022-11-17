using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("----- Player Stuff -----")]
    public GameObject player;
    public playerController playerScript;

    [Header("----- UI -----")]
    public GameObject pauseMenu;
    public GameObject playerDeadMenu;
    public GameObject winMenu;
    public GameObject playerDamageScreen;
    public GameObject playerPowerupScreen;
    public GameObject playerHealthupScreen;
    public TextMeshProUGUI enemiesLeft;
    public Image HPBar;
    public Image staminaBar;
    public Image hoverBar;

    public int enemiesToKill;

    public GameObject spawnPos;

    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf && !playerPowerupScreen.activeSelf && !playerHealthupScreen.activeSelf)
        {
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                pause();
            else
                unpause();
        }
    }

    public void pause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void unpause()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator playerDamageFlash()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }
    public IEnumerator playerPowerupFlash(float timer)
    {
        playerPowerupScreen.SetActive(true);
        yield return new WaitForSeconds(timer);
        playerPowerupScreen.SetActive(false);

    }
    public IEnumerator playerHealthupFlash()
    {
        playerHealthupScreen.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        playerHealthupScreen.SetActive(false);
    }

    
    public void youWin()
    {
        winMenu.SetActive(true);
        pause();
    }

    public void updateEnemyNumber()
    {
        enemiesToKill--;
        updateUI();

        if (enemiesToKill <= 0)
            youWin();
    }

    public void updateUI()
    {
        enemiesLeft.text = enemiesToKill.ToString("F0");
    }
}

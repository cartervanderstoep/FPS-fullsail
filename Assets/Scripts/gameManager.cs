using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public GameObject confirmMenuPause; //Quit confirmation menu from the pause screen
    public GameObject confirmMenuDead; //Quit confirmation menu from the death screen
    public GameObject playerDamageScreen;
    public GameObject playerPowerupScreen;
    public GameObject playerHealthupScreen;
    public TextMeshProUGUI enemiesLeft;
    public TextMeshProUGUI keysCollected;
    public Image HPBar;
    public Image staminaBar;
    public Image hoverBar;

    [Header("----- Main Menu -----")]
    public GameObject mainMenu;
    public GameObject creditsMenu1;
    public GameObject creditsMenu2;
    public GameObject loadingMenu;
    public GameObject controlsMenu;
    public Image loadingBar;

    [Header("----- Dialoge Boxes -----")]
    public GameObject townDialogue;
    public GameObject treeDialogue;
    public GameObject volcanoDialogue;
    public GameObject castleDialogue;

    [Header("----- Level Assets -----")]
    public GameObject magicWall;
    public GameObject NextLevelPortal;
    

    public int enemiesToKill;
    public int keyCount;

    public GameObject spawnPos;

    public bool isPaused;
    public bool isMainMenu;

    [Header("------spawn list--------")]
    public List<GameObject> minionList;
    


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        spawnPos = GameObject.FindGameObjectWithTag("Spawn Pos");
        //NextLevelPortal.SetActive(false);
       
        

    }

    // Update is called once per frame
    void Update()
    {
        if (instance != this)
        {
            instance = this;
        }
        
            playerScript = player.GetComponent<playerController>();
        
      
        if (mainMenu.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
        }
        if (Input.GetButtonDown("Cancel") && !playerDeadMenu.activeSelf && !winMenu.activeSelf && !playerPowerupScreen.activeSelf && !mainMenu.activeSelf && !loadingMenu.activeSelf && !treeDialogue.activeSelf && !townDialogue.activeSelf && !volcanoDialogue.activeSelf && !castleDialogue.activeSelf && !confirmMenuPause.activeSelf && !controlsMenu.activeSelf && !creditsMenu1.activeSelf && !creditsMenu2.activeSelf)
        {
            if (playerHealthupScreen.activeSelf)
            {
                playerHealthupScreen.SetActive(false);
            }
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);

            if (isPaused)
                pause();
            else
                unpause();
        }
        if(keyCount == 3)
        {
            Destroy(magicWall);
        }
        updateUI();
       
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

        if (enemiesToKill <= -100)
            youWin();
    }


    public void updateUI()
    {
        enemiesLeft.text = enemiesToKill.ToString("F0");
        keysCollected.text = keyCount.ToString("F0");
    }
    

    public void nextScene()
    {
        gameManager.instance.loadingMenu.SetActive(true);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        if (loadingOperation.isDone)
        {
            gameManager.instance.loadingMenu.SetActive(false);
            gameManager.instance.mainMenu.SetActive(false);

        }
    }
    public void townLevelDialogue()
    {
        pause();
        townDialogue.SetActive(true);
    }
    public void treeLevelDialogue()
    {
        pause();
        isPaused = true;
        treeDialogue.SetActive(true);
    }
    public void volcanoLevelDialogue()
    {
        pause();
        volcanoDialogue.SetActive(true);
    }
    public void castleLevelDialogue()
    {
        pause();
        castleDialogue.SetActive(true);
    }
    public void CloseTownDialogue()
    {
        townDialogue.SetActive(false);
        isPaused = false;
    }
    public void CloseTreeDialogue()
    {
        treeDialogue.SetActive(false);
        isPaused = false;

    }
    public void CloseVolcanoDialogue()
    {
        volcanoDialogue.SetActive(false);
        isPaused = false;
    }
    public void CloseCastleDialogue()
    {
        castleDialogue.SetActive(false);
        isPaused = false;
    }
    
    public int getHelperCount()
    {
        int minionCount = minionList.Count;

        return minionCount;

    }
    
    
}

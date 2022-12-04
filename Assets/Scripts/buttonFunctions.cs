using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.unpause();
        gameManager.instance.isPaused = false;
        gameManager.instance.pauseMenu.SetActive(false);
    }
    public void closeTownD()
    {
        gameManager.instance.unpause();
        gameManager.instance.CloseTownDialogue();
    }
    public void closeTreeD()
    {
        gameManager.instance.unpause();
        gameManager.instance.CloseTreeDialogue();
    }
    public void closeVolcanoD()
    {
        gameManager.instance.unpause();
        gameManager.instance.CloseVolcanoDialogue();
    }
    public void closeCastleD()
    {
        gameManager.instance.unpause();
        gameManager.instance.CloseCastleDialogue();
    }
    public void restart()
    {
        gameManager.instance.unpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void quit()
    {
        Application.Quit();
    }
    public void respawn()
    {
        gameManager.instance.unpause();
        gameManager.instance.playerScript.respawn();
    }
    public void newGame()
    {
        gameManager.instance.unpause();
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.loadingMenu.SetActive(true);
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        float loadProgress = loadingOperation.progress;
        
        gameManager.instance.loadingBar.fillAmount = loadProgress;
        if (loadingOperation.isDone)
        {
            gameManager.instance.loadingMenu.SetActive(false);
            gameManager.instance.mainMenu.SetActive(false);

        }    
    }
    
    // Credits Buttons
    public void CreditsOpen()
    {
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.creditsMenu1.SetActive(true);
    }
    public void CreditsNext()
    {
        gameManager.instance.creditsMenu1.SetActive(false);
        gameManager.instance.creditsMenu2.SetActive(true);
    }
    public void CreditsPrev()
    {
        gameManager.instance.creditsMenu2.SetActive(false);
        gameManager.instance.creditsMenu1.SetActive(true);
    }
    public void ReturnFromCredits()
    {
        gameManager.instance.creditsMenu2.SetActive(false);
        gameManager.instance.creditsMenu1.SetActive(false);
        gameManager.instance.mainMenu.SetActive(true);
    }
}
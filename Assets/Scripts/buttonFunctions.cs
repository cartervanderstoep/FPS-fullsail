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
    public void ControlsOpen()
    {
        gameManager.instance.mainMenu.SetActive(false);
        gameManager.instance.controlsMenu.SetActive(true);
    }
    public void ReturnFromControls()
    {
        gameManager.instance.controlsMenu.SetActive(false);
        gameManager.instance.mainMenu.SetActive(true);
    }
    public void ConfirmationOpenPause() // Confirmation menu for quitting from the pause menu. 
    {
        gameManager.instance.pauseMenu.SetActive(false);
        gameManager.instance.confirmMenuPause.SetActive(true);
    }
    public void ConfirmationOpenDead() // Confirmation menu for quitting from the death menu. 
    {
        gameManager.instance.playerDeadMenu.SetActive(false);
        gameManager.instance.confirmMenuPause.SetActive(true);
    }
    public void ConfirmationClosePause() // Sends player back to the Pause menu from the confirmation window. 
    {
        gameManager.instance.confirmMenuPause.SetActive(false);
        gameManager.instance.pauseMenu.SetActive(true);
    }
    public void ConfirmationCloseDead() // Sends player back to the Death menu from the confirmation window. 
    {
        gameManager.instance.confirmMenuDead.SetActive(false);
        gameManager.instance.playerDeadMenu.SetActive(true);
    }
    public void ReturnToMainMenuPause()// Sends player back to the main menu from the pause menu
    {
        gameManager.instance.pauseMenu.SetActive(false);
        SceneManager.LoadSceneAsync(0);

    }
    public void ReturnToMainMenuDead()// Sends player back to the main menu from the death menu
    {
        gameManager.instance.playerDeadMenu.SetActive(false);
        SceneManager.LoadSceneAsync(0);
    }
    public void ReturnToMainMenuWin()// Sends player back to main menu from the win screen
    {
        gameManager.instance.winMenu.SetActive(false);
        SceneManager.LoadSceneAsync(0);
    }
}
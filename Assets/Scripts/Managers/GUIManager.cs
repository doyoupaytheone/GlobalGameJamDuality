using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private CanvasFadeEffect pauseMenu;
    
    private void Update()
    {
        //Pauses the game if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();
    }

    #region InGameMenus

    //Toggles the visibility of a canvas
    public void ToggleMenu(CanvasFadeEffect canvasToToggle) => canvasToToggle.ToggleFade(0);

    //Tells the GameManager to exit the game entirely
    public void ExitGame() => GameManager.Instance.ExitGame();

    //Resumes the game and tells the GameManager to change scenes to the main menu
    public void ExitToMain()
    {
        PauseGame();
        GameManager.Instance.ChangeScene(0);
    }

    //Opens the death menu
    public void OpenPlayerDiedMenu()
    {
        PauseGame();
    }

    //Restarts the current scene
    public void RestartLevel()
    {
        PauseGame();
        GameManager.Instance.RefreshScene();
    }

    public void PauseGame()
    {
        //Checks to see if the game is currently paused
        bool isGamePaused = GameManager.Instance.currentGameState != GameManager.GameState.Paused;

        //Pause or resume the game depending on the current state
        GameManager.Instance.Pause(isGamePaused);

        //Opens or closes the pause menu and makes sure it is reset for use
        if (isGamePaused)
        {

        }
        else
        {

        }
    }

    #endregion
}

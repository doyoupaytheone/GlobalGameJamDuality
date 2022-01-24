using System.Collections;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private CanvasFadeEffect pauseMenu;
    [SerializeField] private CanvasFadeEffect maxDarkness;
    [SerializeField] private CanvasFadeEffect maxLight;

    private void Update()
    {
        //Pauses the game if the escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
    }

    #region InGameMenus

    //Toggles the visibility of a canvas
    public void ToggleMenu(CanvasFadeEffect canvasToToggle) => canvasToToggle.ToggleFade(0);

    //Allows outside scripts to toggle the max darkness image
    public void ToggleMaxDarkness() => ToggleMenu(maxDarkness);

    //Allows outside scripts to toggle the max light image
    public void ToggleMaxLight() => ToggleMenu(maxLight);

    //Tells the GameManager to exit the game entirely
    public void ExitGame() => GameManager.Instance.ExitGame();

    //Resumes the game and tells the GameManager to change scenes to the main menu
    public void ExitToMain()
    {
        PauseGame();
        GameManager.Instance.ChangeScene(0);
    }

    //Restarts the current scene
    public void RestartLevel()
    {
        PauseGame();
        GameManager.Instance.RefreshScene();
    }

    public void PauseGame()
    {
        bool isGamePaused = GameManager.Instance.currentGameState != GameManager.GameState.Paused; //Checks to see if the game is currently paused
        GameManager.Instance.Pause(isGamePaused); //If the game is paused, start the time first
        ToggleMenu(pauseMenu); //Opens or closes the pause menu
    }

    private IEnumerator WaitForFade(bool isGamePaused)
    {
        if (isGamePaused)
        {
            GameManager.Instance.Pause(isGamePaused); //If the game is paused, start the time first
            ToggleMenu(pauseMenu); //Opens or closes the pause menu
            yield return new WaitForSeconds(2); //Waits for fade
        }
        else
        {
            ToggleMenu(pauseMenu); //Opens or closes the pause menu
            yield return new WaitForSeconds(2); //Waits for fade
            GameManager.Instance.Pause(isGamePaused); //If the game is not pause, stop the time after the fade
        }
    }

    #endregion
}

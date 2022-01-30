using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    #region SerializeField Variables

    [Header("Save Objects")]
    [SerializeField] private TextMeshProUGUI[] _playTimesText;
    [SerializeField] private TextMeshProUGUI[] _playTimesTextData;
    [SerializeField] private GameObject _enemyEasterEgg;

    [Header("Video Selections")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _qualityDropdown;

    [Header("Audio Selections")]
    [SerializeField] private Slider _mainVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Slider _uiVolumeSlider;

    [Header("Player Input Customization Objects")]
    [SerializeField] private TextMeshProUGUI _leftButton;
    [SerializeField] private TextMeshProUGUI _rightButton;
    [SerializeField] private TextMeshProUGUI _upButton;
    [SerializeField] private TextMeshProUGUI _downButton;
    [SerializeField] private TextMeshProUGUI _interactButton;
    [SerializeField] private TextMeshProUGUI _jumpButton;
    [SerializeField] private TextMeshProUGUI _primaryAttackButton;
    [SerializeField] private TextMeshProUGUI _secondaryAttackButton;

    #endregion

    private AudioManager _audioManager;
    private Resolution[] _resolutions;
    private int _selectedSaveSlot = 0;

    private void Awake() => _audioManager = FindObjectOfType<AudioManager>();

    private void Start() => ResetResolutionOptions();

    //Deactivates the button just used
    public void DeactivateButton(Button button) => button.interactable = false;

    //Tells the game manager to begin the game
    public void StartGame(int slotIndex) => GameManager.Instance.ToFurthestScene(slotIndex);

    //Tells the game manager to quit the game
    public void ExitGame() => GameManager.Instance.ExitGame();

    #region AccessSaveData

    //Tells the game manager to delete the selected save slot
    public void DeleteSaveSlot(int slotIndex) => GameManager.Instance.DeleteSaveData(slotIndex);

    //Tells the game manager to select a specific save slot to manipulate
    public void SelectCurrentSaveSlot(int slotIndex)
    {
        //Select the save slot in GM
        _selectedSaveSlot = slotIndex;
        GameManager.Instance.SelectSaveSlot(slotIndex);
    }

    public void LoadMenuSettings(float[] playTimes)
    {
        for (int i = 0; i < playTimes.Length; i++)
        {
            if (playTimes[i] < 1)
            {
                _playTimesText[i].text = "Empty";
                _playTimesTextData[i].text = "Empty";
            }
            else
            {
                string minutes = String.Format("{0:00}", playTimes[i] % 60);
                _playTimesText[i].text = (int)(playTimes[i] / 60) + ":" + minutes;
                _playTimesTextData[i].text = (int)(playTimes[i] / 60) + ":" + minutes;
            }

            if (GameManager.Instance.SaveSlots[i].FurthestLevel == SceneManager.sceneCountInBuildSettings - 1)
                _enemyEasterEgg.SetActive(true);
        }

        //Sets the values of the sliders for settings
        ResetVolumeValues();
        //Sets the text on the custom input buttons
        ResetCustomControls();
    }

    public void SavePreferences()
    {
        //Saves the pref values as player pref data
        PlayerPrefData.MainVolume = _mainVolumeSlider.value;
        PlayerPrefData.MusicVolume = _musicVolumeSlider.value;
        PlayerPrefData.SfxVolume = _sfxVolumeSlider.value;
        PlayerPrefData.UIVolume = _uiVolumeSlider.value;
        PlayerPrefData.ResolutionIndex = _resolutionDropdown.value;
        PlayerPrefData.VideoQualityIndex = _qualityDropdown.value;
        PlayerPrefData.IsFullscreen = Screen.fullScreen;
    }

    #endregion

    #region ChangeValues

    public void SetMainVolume(float value) => _audioManager.SetGroupVolume(value, "MainVolume");

    public void SetMusicVolume(float value) => _audioManager.SetGroupVolume(value, "MusicVolume");

    public void SetSFXVolume(float value) => _audioManager.SetGroupVolume(value, "SfxVolume");

    public void SetUIVolume(float value) => _audioManager.SetGroupVolume(value, "UiVolume");
    
    public void SetVideoQuality(int value) => QualitySettings.SetQualityLevel(value);

    public void SetFullscreen(bool isFullscreen) => Screen.fullScreen = isFullscreen;

    public void SetResolution(int value)
    {
        Resolution resolution = _resolutions[value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion

    #region ResetDisplays

    private void ResetResolutionOptions()
    {
        //Gets access to all possible screen resolutions of the current computer
        _resolutions = Screen.resolutions;
        //Clears the default resolutions options
        _resolutionDropdown.ClearOptions();

        //Creates an array of strings for each resolution option
        List<string> options = new List<string>();
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            //Checks to see if the available resolution is what the player is currently using and if so, set it to that
            if (_resolutions[i].width == Screen.currentResolution.width && _resolutions[i].height == Screen.currentResolution.height)
                PlayerPrefData.ResolutionIndex = i;
        }

        //Adds the array of resolution option strings to the dropdown
        _resolutionDropdown.AddOptions(options);

        //Sets the displayed value to the one currently being used
        _resolutionDropdown.value = PlayerPrefData.ResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
    }

    public void ToggleMenu(CanvasFadeEffect canvasToToggle) => canvasToToggle.ToggleFade(0.75f);

    //Resets the current values for the custom player inputs
    public void ResetCustomControls()
    {
        _leftButton.text = PlayerPrefData.Left.ToString();
        _rightButton.text = PlayerPrefData.Right.ToString();
        _upButton.text = PlayerPrefData.Up.ToString();
        _downButton.text = PlayerPrefData.Down.ToString();
        _interactButton.text = PlayerPrefData.Interact.ToString();
        _jumpButton.text = PlayerPrefData.Jump.ToString();
        _primaryAttackButton.text = PlayerPrefData.PrimaryAttack.ToString();
        _secondaryAttackButton.text = PlayerPrefData.SecondaryAttack.ToString();
    }

    public void ResetVolumeValues()
    {
        //Updates the position of the sliders based on pref values
        _mainVolumeSlider.value = PlayerPrefData.MainVolume;
        _musicVolumeSlider.value = PlayerPrefData.MusicVolume;
        _sfxVolumeSlider.value = PlayerPrefData.SfxVolume;
        _uiVolumeSlider.value = PlayerPrefData.UIVolume;

        //Sets the pref values in the audio manager
        _audioManager.SetGroupVolume(_mainVolumeSlider.value, "MainVolume");
        _audioManager.SetGroupVolume(_musicVolumeSlider.value, "MusicVolume");
        _audioManager.SetGroupVolume(_sfxVolumeSlider.value, "SfxVolume");
        _audioManager.SetGroupVolume(_uiVolumeSlider.value, "UiVolume");
    }

    #endregion
}

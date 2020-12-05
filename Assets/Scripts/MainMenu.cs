using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _levelsMenu;
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _levelsButton;
    [Space]
    [SerializeField] private TMP_Text _resumeBtnText;

    private void UpdatePlayText()
    {
        if (ServiceManager.GetLastLevelIndex() == 0)
        {
            _resumeBtnText.text = "New game".ToUpperInvariant();
            _levelsButton.GetComponent<Button>().interactable = false;
        }
    }

    private void Start()
    {
        UpdatePlayText();
    }

    public void Play()
    {
        int levelToLoad = ServiceManager.GetLastLevelIndex();
        levelToLoad = levelToLoad == 0 ? 1 : levelToLoad;

        ServiceManager.ChangeLevel((Level)levelToLoad);
    }

    public void ToggleSettingsMenu()
    {
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
    }


    public void ToggleLevelsMenu()
    {
        _levelsMenu.SetActive(!_levelsMenu.activeSelf);
    }

    public void ResetProgress()
    {
        ServiceManager.ResetProgress();
        UpdatePlayText();
    }

    public void Exit()
    {
        ServiceManager.Quit();
    }

}
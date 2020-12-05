using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{

    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _settingsMenu;

    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        bool menuEnabled = _menu.activeInHierarchy;

        if (menuEnabled)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }

        _settingsMenu.SetActive(false);
        _menu.SetActive(!menuEnabled);
    }

    public void Resume()
    {
        ToggleMenu();
    }

    public void ToggleSettingsMenu()
    {
        _settingsMenu.SetActive(!_settingsMenu.activeSelf);
    }

    public void Menu()
    {
        ServiceManager.ChangeLevel(Level.MainMenu);
    }

    public void Restart()
    {
        ServiceManager.Restart();
    }

    public void Quit()
    {
        ServiceManager.Quit();
    }

}
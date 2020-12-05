using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level
{
    MainMenu,
    Level1,
    Level2,
    Level3
}

public class ServiceManager : MonoBehaviour
{
    private const string LastLevelPref = "LastLevel";

    private void Start()
    {
        TryUpdateNewLevel();
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("RESET!");
    }

    public static int GetLastLevelIndex()
    {
        if (PlayerPrefs.HasKey(LastLevelPref) == false)
        {
            PlayerPrefs.SetInt(LastLevelPref, 0);
            return 0;
        }
        else return PlayerPrefs.GetInt(LastLevelPref);
    }

    public static int TryUpdateNewLevel()
    {
        int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int lastSceneIndex = GetLastLevelIndex();

        if (lastSceneIndex < curSceneIndex)
        {
            PlayerPrefs.SetInt(LastLevelPref, curSceneIndex);
            return curSceneIndex;
        }
        else return lastSceneIndex;
    }

    public static bool CheckLevelUnlocked(Level level)
    {
        int lastIndex = GetLastLevelIndex();
        return (lastIndex >= (int)level);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void ChangeLevel(Level level)
    {
        SceneManager.LoadScene((int)level);
    }
    public static void ChangeLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public static int GetNextLevel()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        bool exists = Application.CanStreamedLevelBeLoaded(current + 1);

        return exists ? current + 1 : 0;
    }

    public static void Quit()
    {
        Application.Quit();
        Debug.Log("QUIT!");
    }


}
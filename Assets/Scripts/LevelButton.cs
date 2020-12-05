using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Level _levelToLoad;
    [SerializeField] private GameObject _lockSprite;
    [SerializeField] private TMP_Text _numberText;

    private void Awake()
    {
        Button btn = GetComponent<Button>();

        _numberText.text = ((int)_levelToLoad).ToString();
        btn.interactable = ServiceManager.CheckLevelUnlocked(_levelToLoad);
        if (btn.interactable == false)
        {
            _lockSprite.SetActive(true);
        }
        else
        {
            btn.onClick.AddListener(() => { ServiceManager.ChangeLevel(_levelToLoad); });
        }
    }

}
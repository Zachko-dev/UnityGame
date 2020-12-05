using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Toggle _fullscreen;
    [SerializeField] private TMP_Dropdown _resolution;
    [SerializeField] private TMP_Dropdown _quality;

    private Resolution[] _availableResolutions;
    private string[] _availableQuality;

    private void Start()
    {
        _availableResolutions = Screen.resolutions;
        _resolution.ClearOptions();
        var resOptions = _availableResolutions
            .Where(r => r.width > 800)
            .Select(r => $"{r.width}x{r.height}")
            .ToList();
        _resolution.AddOptions(resOptions);

        _availableQuality = QualitySettings.names;
        _quality.ClearOptions();
        _quality.AddOptions(_availableQuality.ToList());

        _fullscreen.onValueChanged.AddListener((bool value) => {
            Screen.fullScreen = !value;
        });

        _resolution.onValueChanged.AddListener((int value) =>
        {
            var resolution = new Resolution();
            for (int i = 0; i < _availableResolutions.Length; i++)
            {
                if (i == value)
                {
                    resolution = _availableResolutions[i];
                    Screen.SetResolution(resolution.width, resolution.height, !_fullscreen.isOn);
                    break;
                }
            }
        });
    }
}
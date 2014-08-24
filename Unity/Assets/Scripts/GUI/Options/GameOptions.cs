using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour 
{
    public static GameOptions Instance { get; private set; }

    public float Sensitivity { get; private set; }

    void Awake()
    {
        if (GameOptions.Instance == null)
        {
            GameOptions.Instance = this;
            LoadOptions();
        }
        else
        {
            this.enabled = false;
        }
    }

    private void LoadOptions()
    {
        SetVolume(PlayerPrefs.GetFloat("Volume", 1.0f));
        SetSensitivity(PlayerPrefs.GetFloat("Sensitivity", 0.5f));
        SetGraphics(PlayerPrefs.GetString("Graphics", "good"));
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetSensitivity(float value)
    {
        this.Sensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void SetGraphics(string quality)
    {
        int qIndex = 0;
        for (; qIndex < QualitySettings.names.Length; ++qIndex)
        {
            if (QualitySettings.names[qIndex].ToLower() == quality.ToLower())
            {
                break;
            }
        }

        if (qIndex == QualitySettings.names.Length)
        {
            return;
        }

        QualitySettings.SetQualityLevel(qIndex);
        PlayerPrefs.SetString("Graphics", quality.ToLower());
    }
}

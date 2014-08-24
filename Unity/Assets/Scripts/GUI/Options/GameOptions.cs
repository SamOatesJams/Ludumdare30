using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour 
{
    public static GameOptions Instance { get; private set; }

    public float Sensitivity { get; private set; }

    public string PlayerName { get; private set; }

    void Awake()
    {
        if (GameOptions.Instance == null)
        {
            GameOptions.Instance = this;
            AudioListener.volume = 0.0f;
            LoadOptions();
        }
        else
        {
            this.enabled = false;
        }
    }

    private void LoadOptions()
    {
        SetVolume(GetVolume());
        SetSensitivity(GetSensitivity());
        SetGraphics(GetGraphics());
        SetPlayerName(GetPlayerName());
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 1.0f);
    }

    public float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity", 0.5f);
    }

    public string GetGraphics()
    {
        return PlayerPrefs.GetString("Graphics", "good");
    }

    public string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName", "Enter your name...");
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
        PlayerPrefs.SetString("Graphics", quality);
    }

    public void SetPlayerName(string name)
    {
        this.PlayerName = name;
        PlayerPrefs.SetString("PlayerName", name);
    }
}

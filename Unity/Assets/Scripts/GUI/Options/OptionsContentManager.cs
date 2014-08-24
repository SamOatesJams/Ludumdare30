using UnityEngine;
using System.Collections;

public class OptionsContentManager : MonoBehaviour {

    private bool m_setup = false;

    public UISlider Volume = null;
    public UISlider Sensitivity = null;
    public UIPopupList Graphics = null;
    public UILabel PlayerName = null;

    void Start()
    {
        Volume.sliderValue = GameOptions.Instance.GetVolume();
        Sensitivity.sliderValue = GameOptions.Instance.GetSensitivity();
        Graphics.selection = GameOptions.Instance.GetGraphics();
        PlayerName.text = GameOptions.Instance.GetPlayerName();
    }
}

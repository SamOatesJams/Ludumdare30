using UnityEngine;
using System.Collections;

public class VolumeSlider : MonoBehaviour {
    public void OnValueChange(float value)
    {
        GameOptions.Instance.SetVolume(value);
    }
}

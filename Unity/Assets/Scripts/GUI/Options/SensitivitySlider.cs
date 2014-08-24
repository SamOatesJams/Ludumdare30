using UnityEngine;
using System.Collections;

public class SensitivitySlider : MonoBehaviour {

    public void OnValueChange(float value)
    {
        GameOptions.Instance.SetSensitivity(value);
    }
}

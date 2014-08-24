using UnityEngine;
using System.Collections;

public class QualityListener : MonoBehaviour 
{
    public void OnSelectionChange(string item)
    {
        GameOptions.Instance.SetGraphics(item);
    }
}

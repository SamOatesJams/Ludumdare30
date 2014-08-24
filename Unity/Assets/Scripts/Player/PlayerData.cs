using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public float Health = 100.0f;

    public UISlider HealthSlider = null;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        this.HealthSlider.sliderValue = this.Health / 100.0f;
	}
}

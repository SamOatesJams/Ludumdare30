using UnityEngine;
using System.Collections;

public class BackgroundAudio : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        this.GetComponent<AudioSource>().Play();
	}

}

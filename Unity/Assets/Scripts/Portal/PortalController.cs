using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

    public Vector3 ExitLocation;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.position = ExitLocation;
            Debug.Log("Portaled " + collider.name);
        }
    }
}

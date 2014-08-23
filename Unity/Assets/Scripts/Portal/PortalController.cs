using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    public Vector3 ExitLocation;

    /// <summary>
    /// 
    /// </summary>
    public int TextureSize = 512;

    /// <summary>
    /// 
    /// </summary>
    public Camera TargetCamera = null;

	// Use this for initialization
	void Start () 
    {
        var texture = new RenderTexture(this.TextureSize, this.TextureSize, 32);
        this.renderer.material.SetTexture("_MainTex", texture);
        this.TargetCamera.targetTexture = texture;
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.position = this.ExitLocation;
        }
    }
}

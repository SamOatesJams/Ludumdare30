using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour 
{
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
            var offset = collider.transform.position - this.transform.position;
            collider.transform.position = this.TargetCamera.transform.position + offset + (this.TargetCamera.transform.forward * 4.0f);

            var network = collider.gameObject.GetComponent<PlayerNetwork>();
            if (network != null)
            {
                network.HasTeleported = true;
            }
        }
    }
}

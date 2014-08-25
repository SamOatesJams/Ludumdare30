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
        Transform playerXform = null;
        Transform parent = collider.transform;
        while (parent != null)
        {
            if (parent.tag == "Player")
            {
                playerXform = parent;
                break;
            }
            parent = parent.parent;
        }
        
        if (playerXform != null)
        {
            var data = playerXform.GetComponent<PlayerData>();
            if (data != null)
            {
                if (Time.time - data.LastPortal > 1.0f)
                {
                    playerXform.position = this.TargetCamera.transform.position + (this.TargetCamera.transform.forward * 4.0f);

                    var network = playerXform.GetComponent<PlayerNetwork>();
                    if (network != null)
                    {
                        network.HasTeleported = true;
                    }

                    data.LastPortal = Time.time;
                }
            }
        }
    }
}

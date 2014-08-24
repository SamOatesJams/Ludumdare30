using UnityEngine;
using System.Collections;

public class PlayerNameTag : MonoBehaviour 
{
    public UILabel Label = null;
    private bool m_setup = false;

    private Transform m_localCamera = null;

    void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            if (player.name == "LocalPlayer")
            {
                var camera = player.gameObject.GetComponentInChildren<Camera>();
                if (camera != null)
                {
                    m_localCamera = camera.transform;
                }                
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (m_localCamera != null)
        {
            this.transform.LookAt(m_localCamera);
        }
      
        if (!m_setup)
        {
            var photon = this.transform.parent.GetComponent<PhotonView>();
            if (photon != null && photon.owner != null)
            {
                Label.text = photon.owner.name;
            }            
            m_setup = true;
        }
	}
}
